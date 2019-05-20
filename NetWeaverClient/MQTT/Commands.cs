using System.Diagnostics;
using System.IO;
using System.Security.Policy;

namespace NetWeaverClient.MQTT
{
    public static class Commands
    {
        private static readonly string Name = "Scripts";
        private static readonly string Scripts = "\"C:\\NetWeaver\\Scripts";

        public static int OpenNetShare()
        {
            string command = "New-SmbShare -Name " + Name + " -Path " + Scripts + "\"";
            ProcessStartInfo startInfo = new ProcessStartInfo("powershell.exe", command) 
                {CreateNoWindow = true, UseShellExecute = false};
    
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
            ProcessStartInfo startInfo = new ProcessStartInfo(
                "powershell.exe", "Remove-SmbShare -Name " + Name + " -Force")
            {
                CreateNoWindow = true, UseShellExecute = false, RedirectStandardError = true
            };
            
            var process = Process.Start(startInfo);
            string error = process?.StandardError.ReadToEnd();

            if (error != null) return -1;
            return 0;
        }
        
        public static int RunPowershellScript(string ps)
        {
            var processInfo = new ProcessStartInfo("powershell.exe")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                Verb = "runas",
                Arguments = "-Exec Bypass -File " + Scripts + "\\" + ps + "\""
            };
            
            var process = Process.Start(processInfo);
            string error = process?.StandardError.ReadToEnd();
            
            if (error != null) return -1;
            return 0;
        }
    }
}
