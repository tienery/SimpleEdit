using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using FastColoredTextBoxNS;

namespace SimpleEdit
{
    public partial class MainForm : Form
    {

        private List<string> dirs;
        private List<string> prevCommands;
        private string currentFindValue;
        private string currentReplaceValue;
        private SynchronizationContext _sync;
        private bool saved;
        private int currentPrevIndex;
        private Editor currentEditor { 
            get 
            {
                return (Editor)tcMain.SelectedTab.Controls[0];
            } 
        }

        public MainForm()
        {
            InitializeComponent();

            dirs = new List<string>();
            prevCommands = new List<string>();

            _sync = SynchronizationContext.Current;
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

        private void OpenFile(string file, bool newTab = false)
        {
            if (currentEditor.Text != "" && !saved)
            {
                var response = MessageBox.Show("Would you first like to save the current document?", "Save", MessageBoxButtons.YesNoCancel);
                if (response == System.Windows.Forms.DialogResult.Yes)
                {
                    if (currentEditor.currentDoc != "")
                    {
                        File.WriteAllText(currentEditor.currentDoc, currentEditor.Text);
                        AppendResult(currentEditor.currentDoc + " saved successfully.");
                    }
                    else
                    {
                        OpenOFD();
                    }
                }
                else if (response == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }

            var path = Environment.CurrentDirectory + "/" + file;
            if (File.Exists(path))
            {
                var contents = File.ReadAllText(path);

                if (newTab)
                    NewTab(file);

                SetEditorLang(path, contents);

                currentEditor.Text = contents;
                currentEditor.currentDoc = path;
            }
            else
            {
                WriteResult("The file you are attempting to open does not exist");
            }
        }

        private void NewTab(string file = "")
        {
            TabPage t = new TabPage(file);
            var editor = new Editor() { Dock = DockStyle.Fill };
            editor.TextChanged += currentEditor_TextChanged;
            editor.KeyUp += currentEditor_KeyUp;
            t.Controls.Add(editor);
            tcMain.TabPages.Add(t);
            tcMain.SelectedTab = t;
        }

        private string CheckPath(string path)
        {
            return path.Substring(path.LastIndexOf('.'), path.Length - path.LastIndexOf('.'));
        }

        private void SetEditorLang(string path, string contents)
        {
            if (CheckPath(path) == ".cpp" || CheckPath(path) == ".c" || CheckPath(path) == ".h")
                currentEditor.SetLang("cpp");
            else if (CheckPath(path) == ".hx")
                currentEditor.SetLang("haxe");
            else if (CheckPath(path) == ".sql")
                currentEditor.SetLang("sql");
            else if (CheckPath(path) == ".html" || CheckPath(path) == ".htm" || CheckPath(path) == ".asp" || CheckPath(path) == ".aspx")
                currentEditor.SetLang("html");
            else if (CheckPath(path) == ".cs")
                currentEditor.SetLang("cs");
            else if (CheckPath(path) == ".js")
                currentEditor.SetLang("js");
            else if (CheckPath(path) == ".lua")
                currentEditor.SetLang("lua");
            else if (CheckPath(path) == ".php")
            {
                if (contents.StartsWith("<?php"))
                    currentEditor.SetLang("php");
                else
                    currentEditor.SetLang("html");
            }
            else if (CheckPath(path) == ".vb")
                currentEditor.SetLang("vb");
            else if (CheckPath(path) == ".xml")
                currentEditor.SetLang("xml");
            else
                currentEditor.SetLang("");
        }

        private void SaveFile()
        {
            if (currentEditor.currentDoc != "" || currentEditor.currentDoc != null)
            {
                saved = true;
                File.WriteAllText(currentEditor.currentDoc, currentEditor.Text);
                AppendResult(currentEditor.currentDoc + " successfully saved.");
            }
            else
                OpenOFD();
        }

        private void OpenOFD()
        {
            var sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd.Filter = "All Files (*.*)|*.*";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, currentEditor.Text);
            }
            else
                return;
        }

        private void NewFile(string file, bool newTab = false)
        {
            var path = Environment.CurrentDirectory + "/" + file;
            if (!File.Exists(path))
            {
                try
                {
                    if (newTab)
                        NewTab(file);

                    currentEditor.currentDoc = path;
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

        private void currentEditor_Load(object sender, EventArgs e)
        {

        }

        private void currentEditor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            saved = false;
        }

        private void currentEditor_KeyUp(object sender, KeyEventArgs e)
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
                        AsyncExecuteCommand(value.Substring(5));
                    }
                    else if (value.StartsWith("cmd"))
                    {
                        WriteResult(Utils.ExecuteCommand(value.Substring(4), false));
                    }
                    else if (value.StartsWith("opent"))
                    {
                        OpenFile(value.Substring(6), true);
                    }
                    else if (value.StartsWith("open"))
                    {
                        OpenFile(value.Substring(5));
                    }
                    else if (value.StartsWith("save"))
                    {
                        SaveFile();
                    }
                    else if (value.StartsWith("newt"))
                    {
                        NewFile(value.Substring(5), true);
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
                            if (!Regex.IsMatch(sub, @"^[A-Z]+?:(\\)?(/)?") && Directory.Exists(Environment.CurrentDirectory + "/" + sub))
                                Environment.CurrentDirectory = sub;
                            else if (Regex.IsMatch(sub, @"^[A-Z]+?:(\\)?(/)?") && Directory.Exists(sub))
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
                    else if (value.StartsWith("find"))
                    {
                        currentEditor.ShowFindDialog(value.Substring(5));
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
                if (currentEditor.Focused)
                    txtCommand.Focus();
                else
                    currentEditor.Focus();
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

        public void AsyncExecuteCommand(string cmd)
        {
            txtResults.Text = "Executing " + cmd + " ...\n";

            ProcessStartInfo info = new ProcessStartInfo("cmd.exe", "/c " + cmd);
            info.WorkingDirectory = Environment.CurrentDirectory;
            info.CreateNoWindow = true;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;

            Process p = new Process();
            p.StartInfo = info;

            p.OutputDataReceived += p_OutputDataReceived;
            p.ErrorDataReceived += p_ErrorDataReceived;

            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();

            p.WaitForExit();
        }

        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            DisplayResults(e.Data);
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            DisplayResults(e.Data);
        }

        private void DisplayResults(string output)
        {
            _sync.Post(_ =>  {
                try
                {
                    txtResults.AppendText(output);
                }
                catch (Exception ex)
                {

                }
            }, null);
        }

    }
}
