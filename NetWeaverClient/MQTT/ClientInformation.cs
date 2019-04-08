using System;
using System.Diagnostics;

namespace NetWeaverClient.MQTT
{
    public class ClientInformation
    {
        private readonly string _mac;
        private readonly string _ip;
        private readonly string _name;
        private readonly string _adapter;
        public string Info => $"{_mac}&{_ip}&{_name}";   

        public ClientInformation()
        {
            this._mac = GetMacAddress();
            this._ip = GetIpAddress();
            this._name = Environment.MachineName;
            this._adapter = GetAdapterName();
        }

        private string GetAdapterName()
        {
            string name = string.Empty;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "Get-NetAdapter -physical",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            Process process = new Process {StartInfo = startInfo};
            process.Start();
            
            string line;
            while((line = process.StandardOutput.ReadLine()) != null)
            {
                if (!line.Contains("InterfaceAlias")) continue;
                name = line.Split(':')[1].Trim();
                process.Kill();
                break;
            }

            return name;
        }

        private string GetIpAddress()
        {
            return "IP";
        }

        private string GetMacAddress()
        {
            return "MAC";
        }
    }
}