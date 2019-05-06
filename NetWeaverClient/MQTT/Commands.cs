using System.Diagnostics;
using System.IO;
using System;

namespace NetWeaverClient.MQTT
{
    public static class Commands
    {
        private static readonly string Name = "Scripts";
        private static readonly string Scripts = "\"C:\\NetWeaver\\Scripts\\";

        public static int OpenNetShare()
        {
            string command = "New-SmbShare -Name " + Name + " -Path " + Scripts + "\"";
            ProcessStartInfo startInfo = new ProcessStartInfo("powershell.exe", command) 
                {CreateNoWindow = true, };

            Process p = Process.Start(startInfo);
            return 0;
        }
        public static int SeeFile(string filename)
        {
            if (File.Exists(filename))
            {
                return 0;
            }
            return -1;
        }
        public static int CloseNetShare()
        {
            Process p = Process.Start("powershell.exe", "Remove-SmbShare -Name " + Name + " -Force");
            return 0;
        }
        public static int ExecuteScript(string filename)
        {
            Process process = Process.Start("powershell.exe", filename);
            return -1;
        } 
        
        public static int RunPowershellScript(string ps)
        {
            var processInfo = new ProcessStartInfo("Powershell.exe")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                Verb = "runas",
                Arguments = "-Exec Bypass -File " + Scripts + ps + "\""
            };

            var process = Process.Start(processInfo);
            
            /*
            if (process == null) return 0;
            process.WaitForExit();

            var errorLevel = process.ExitCode;
            process.Close();

            return errorLevel;
            */
            return 0;
        }
    }
}