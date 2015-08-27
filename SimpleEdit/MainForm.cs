using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FastColoredTextBoxNS;

namespace SimpleEdit
{
    public partial class MainForm : Form
    {

        private string previousDoc;
        private List<string> dirs;
        private List<string> prevCommands;
        private bool saved;
        private int currentPrevIndex;

        public MainForm()
        {
            InitializeComponent();

            dirs = new List<string>();
            prevCommands = new List<string>();

            RebuildMenu();
        }

        private void RebuildMenu(string value = "")
        {
            txtCommand.AutoCompleteCustomSource.Clear();
            switch (value)
            {
                case "cd":
                    txtCommand.AutoCompleteCustomSource.AddRange(dirs.ToArray());
                    break;
                case "cmd":
                    txtCommand.AutoCompleteCustomSource.AddRange(prevCommands.ToArray());
                    break;
            }
        }

        private string RootDir(string path)
        {
            try
            {
                return Directory.GetParent(path).FullName;
            }
            catch (Exception ex)
            {
                return "C:\\";
            }
        }

        private void OpenFile(string file)
        {
            if (MainEditor.Text != "" && !saved)
            {
                var response = MessageBox.Show("Would you first like to save the current document?", "Save", MessageBoxButtons.YesNoCancel);
                if (response == System.Windows.Forms.DialogResult.Yes)
                {
                    if (previousDoc != "")
                        File.WriteAllText(previousDoc, MainEditor.Text);
                    else
                    {
                        var sfd = new SaveFileDialog();
                        sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        sfd.Filter = "All Files (*.*)|*.*";
                        if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            File.WriteAllText(sfd.FileName, MainEditor.Text);
                        }
                        else
                            return;
                    }
                }
                else if (response == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }

            var path = Environment.CurrentDirectory + "/" + file;
            if (File.Exists(path))
            {
                var contents = File.ReadAllText(path);

                SetEditorLang(path, contents);

                MainEditor.Text = contents;
                previousDoc = path;
            }
            else
            {
                WriteResult("The file you are attempting to open does not exist");
            }
        }

        private string CheckPath(string path)
        {
            return path.Substring(path.LastIndexOf('.'), path.Length - path.LastIndexOf('.'));
        }

        private void SetEditorLang(string path, string contents)
        {
            if (CheckPath(path) == ".cpp" || CheckPath(path) == ".c" || CheckPath(path) == ".h")
                MainEditor.SetLang("cpp");
            else if (CheckPath(path) == ".hx")
                MainEditor.SetLang("haxe");
            else if (CheckPath(path) == ".sql")
                MainEditor.SetLang("sql");
            else if (CheckPath(path) == ".html" || CheckPath(path) == ".htm" || CheckPath(path) == ".asp" || CheckPath(path) == ".aspx")
                MainEditor.SetLang("html");
            else if (CheckPath(path) == ".cs")
                MainEditor.SetLang("cs");
            else if (CheckPath(path) == ".js")
                MainEditor.SetLang("js");
            else if (CheckPath(path) == ".lua")
                MainEditor.SetLang("lua");
            else if (CheckPath(path) == ".php")
            {
                if (contents.StartsWith("<?php"))
                    MainEditor.SetLang("php");
                else
                    MainEditor.SetLang("html");
            }
            else if (CheckPath(path) == ".vb")
                MainEditor.SetLang("vb");
            else if (CheckPath(path) == ".xml")
                MainEditor.SetLang("xml");
            else
                MainEditor.SetLang("");
        }

        private void SaveFile()
        {
            if (previousDoc != "")
            {
                saved = true;
                File.WriteAllText(previousDoc, MainEditor.Text);
            }
        }

        private void NewFile(string file)
        {
            var path = Environment.CurrentDirectory + "/" + file;
            if (!File.Exists(path))
            {
                try
                {
                    previousDoc = path;
                    File.WriteAllText(path, "");

                    SetEditorLang(path, "");
                }
                catch (Exception ex)
                {
                    AppendResult(ex.Message);
                }
            }
            
        }

        private void WriteResult(string result)
        {
            txtResults.Text = result;
        }

        private void AppendResult(string result)
        {
            txtResults.Text += "\n" + result;
        }

        private void MainEditor_Load(object sender, EventArgs e)
        {

        }

        private void MainEditor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            saved = false;
        }

        private void MainEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender == txtCommand)
            {
                var value = txtCommand.Text;

                if (e.KeyCode == Keys.Enter)
                {
                    txtCommand.Text = "";
                    bool canAdd = true;
                    foreach (var i in prevCommands)
                    {
                        if (value == i)
                        {
                            canAdd = false;
                            break;
                        }
                    }
                    if (canAdd)
                        prevCommands.Add(value);

                    currentPrevIndex = prevCommands.Count - 1;

                    if (value.StartsWith("cmds"))
                    {
                         WriteResult(Utils.ExecuteCommand(value.Substring(5), true));
                    }
                    else if (value.StartsWith("cmd"))
                    {
                        WriteResult(Utils.ExecuteCommand(value.Substring(4)));
                    }
                    else if (value.StartsWith("open"))
                    {
                        OpenFile(value.Substring(5));
                    }
                    else if (value.StartsWith("save"))
                    {
                        SaveFile();
                    }
                    else if (value.StartsWith("new"))
                    {
                        NewFile(value.Substring(4));
                    }
                    else if (value.StartsWith("cd"))
                    {
                        var sub = value.Substring(3);
                        if (sub != "..")
                        {
                            if (!Regex.IsMatch(sub, @"^[A-Z]+?:\\") && Directory.Exists(Environment.CurrentDirectory + "/" + sub))
                                Environment.CurrentDirectory = sub;
                            else if (Regex.IsMatch(sub, @"^[A-Z]+?:\\") && Directory.Exists(sub))
                                Environment.CurrentDirectory = sub;
                            else
                                AppendResult("Attempt to change directory failed because the path does not exist.");
                        }
                        else
                        {
                            Environment.CurrentDirectory = RootDir(Environment.CurrentDirectory);
                        }
                        lblCurrentDir.Text = Environment.CurrentDirectory;
                    }
                }

                if (e.KeyCode == Keys.Up)
                {
                    if (currentPrevIndex - 1 < 0)
                        currentPrevIndex = 0;
                    else
                        --currentPrevIndex;

                    if (prevCommands.Count > 0)
                    {
                        txtCommand.Text = prevCommands[currentPrevIndex];
                    }
                }

                if (e.KeyCode == Keys.Down)
                {
                    if (currentPrevIndex + 1 > prevCommands.Count - 1)
                        currentPrevIndex = prevCommands.Count - 1;
                    else
                        ++currentPrevIndex;

                    if (prevCommands.Count > 0)
                    {
                        txtCommand.Text = prevCommands[currentPrevIndex];
                    }
                }
            }

            if (e.Control && e.KeyCode == Keys.Tab)
            {
                if (MainEditor.Focused)
                    txtCommand.Focus();
                else
                    MainEditor.Focus();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists("autoCompleteDirs"))
                ReadAutoCompleteSource("autoCompleteDirs", "dir");
            if (File.Exists("autoCompleteCmd"))
                ReadAutoCompleteSource("autoCompleteCmd", "cmd");

            Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            lblCurrentDir.Text = Environment.CurrentDirectory;
        }

        private void ReadAutoCompleteSource(string file, string type)
        {
            var split = File.ReadAllText(file).Split('\n');

            if (type == "dir")
                dirs.AddRange(split.AsEnumerable());
            else if (type == "cmd")
                prevCommands.AddRange(split.AsEnumerable());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            WriteAutoCompleteSource();
        }

        private void WriteAutoCompleteSource()
        {
            {
                string result = "";
                foreach (var s in dirs)
                {
                    if (dirs.IndexOf(s) == 0)
                        result += s;
                    else
                        result += "\n" + s;
                }

                File.WriteAllText("autoCompleteDirs", result);
            }
            
            {
                string result = "";
                foreach (var s in prevCommands)
                {
                    if (prevCommands.IndexOf(s) == 0)
                        result += s;
                    else
                        result += "\n" + s;
                }

                File.WriteAllText("autoCompleteCmd", result);
            }
        }

    }
}
