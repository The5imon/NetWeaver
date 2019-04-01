using System;

namespace NetWeaverServer.Datastructure
{
    public class Client
    {
        private string MAC { get; }

        private string RoomName { get; }

        private string HostName { get; }

        private string IPAddress { get; }

        private bool IsOnline { get; }
        
        private string LastSeen { get; }

        public Client(string mac, string roomName, string hostName, string ipAddress)
        {
            MAC = mac;
            RoomName = roomName;
            HostName = hostName;
            IPAddress = ipAddress;
        }
        
        public override string ToString()
        {
            return $"Client: {HostName} MAC: {MAC} IP: {IPAddress} isOnline: {IsOnline} lastSeen: {LastSeen}";
        }
    }
}