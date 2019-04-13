using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;

namespace NetWeaverClient.MQTT
{
    public static class Netshare
    {
        public static void ExecuteScript(string name)
        {
            Process process = new Process
            {
                StartInfo =
                {
                    Verb = "runas",
                    FileName = "powershell.exe",
                    CreateNoWindow = true,
                    Arguments = name
                }
            };

            process.Start(); //TODO: Redirect Output to verify successfullness
        }
        
        public static void OpenNetShare(string path, string name)
        {
            Process process = new Process
            {
                StartInfo =
                {
                    Verb = "runas",
                    FileName = "powershell.exe",
                    CreateNoWindow = true,
                    Arguments = "New-SMBShare -Path " + path + " -Name " + name
                }
            };

            process.Start();
        }

        public static void CloseNetShare(string name)
        {
            Process process = new Process
            {
                StartInfo =
                {
                    Verb = "runas",
                    FileName = "powershell.exe",
                    CreateNoWindow = true,
                    Arguments = "New-SMBShare -Name " + name + " -Force"
                }
            };

            process.Start();
        }
    }
}