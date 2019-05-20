using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace NetWeaverClient.MQTT
{
    public class ClientInformation
    {
        private readonly string _mac;
        private readonly string _ip;
        private readonly string _adapter;
        private readonly string _interface;
        
        public string Name { get; }
        public string Info => $"{Name}&{_mac}&{_ip}&{_interface}";

        public ClientInformation()
        {
            //this._interface = DeviceDiscovery.StartSniffing();
            this._adapter = GetAdapterName();
            this._mac = GetMacAddress();
            this._ip = GetIpAddress();
            this.Name = Environment.MachineName;
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
            while ((line = process.StandardOutput.ReadLine()) != null)
            {
                if (!line.Contains("Realtek")) continue; // "Realtek" is local port.
                name += Regex.Split(line, "  +")[0];
                process.Kill();
                break;
            }
            return name;
        }

        private string GetIpAddress()
        {
            string ip = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.Name.Equals(_adapter))
                {
                    ip = nic.GetIPProperties().UnicastAddresses.FirstOrDefault(
                            ipa => ipa.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        ?.Address
                        .ToString();
                }
            }
            return ip;
        }

        private string GetMacAddress()
        {
            string mac = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.Name.Equals(_adapter))
                {
                    mac = nic.GetPhysicalAddress().ToString();
                }
            }
            return mac;
        }
    }
}