using System;

namespace NetWeaverServer.Datastructure
{
    public class Client
    {
        public string MAC { get; }
        public string RoomName { get; }
        public string HostName { get; }
        public string IPAddress { get; }
        public bool IsOnline { get; }
        public string LastSeen { get; }

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