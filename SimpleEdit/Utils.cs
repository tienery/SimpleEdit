using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SimpleEdit
{
    public class Utils
    {

        public static string ExecuteCommand(string cmd, bool showWindow = false)
        {
            string results = "";

            ProcessStartInfo info = new ProcessStartInfo("cmd.exe", "/c " + cmd);
            info.WorkingDirectory = Environment.CurrentDirectory;
            info.CreateNoWindow = !showWindow;
            info.RedirectStandardError = !showWindow;
            info.RedirectStandardOutput = !showWindow;
            info.UseShellExecute = false;

            Process p = Process.Start(info);
            if (!showWindow)
            {
                results = p.StandardOutput.ReadToEnd();
                results += p.StandardError.ReadToEnd();

                p.WaitForExit();
            }

            return results;
        }

    }
}
