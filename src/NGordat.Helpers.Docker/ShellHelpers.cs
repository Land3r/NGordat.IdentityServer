using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Helpers.Docker
{
    public static class ShellHelpers
    {
        public static string Bash(this string cmd)
        {
            string text = cmd.Replace("\"", "\\\"");
            if (File.Exists("/bin/bash"))
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + text + "\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                process.Start();
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return result;
            }

            return string.Empty;
        }
    }
}
