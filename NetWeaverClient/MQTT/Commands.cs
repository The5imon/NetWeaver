using System;
using System.Diagnostics;
using System.IO;
using System.Security.Policy;
using PcapDotNet.Base;

namespace NetWeaverClient.MQTT
{
    public static class Commands
    {    
        private static readonly string Scripts = "\"E:\\NetWeaver\\Scripts\"";

        public static int OpenNetShare()
        {
            string command = "net share Scripts=" + Scripts + "/grant:jeder,FULL";
            Process p = Process.Start("cmd.exee", command);
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
            Process p = Process.Start("cmd.exe", "net share Scripts /delete");
            return 0;
        }
        public static int ExecuteScript(string filename)
        {
            Process process = Process.Start("powershell.exe", filename);
            return -1;
        } 
        
        public static int RunPowershellScript(string ps)
        {
            var processInfo = new ProcessStartInfo("powershell.exe", "-File " + ps);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;

            var process = Process.Start(processInfo);
            process.WaitForExit();

            var errorLevel = process.ExitCode;
            process.Close();

            return errorLevel;
        }
    }
}