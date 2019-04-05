using System;

namespace NetWeaverServer.Datastructure
{
    public class Client
    {
        public string MAC { get; }
        public int RoomNumber { get; }
        public string HostName { get; }
        public string IPAddress { get; }
        public bool IsOnline { get; }
        public string LastSeen { get; }

        public Client(string mac, int roomNumber, string hostName, string ipAddress, bool isOnline, string lastSeen)
        {
            MAC = mac;
            RoomNumber = roomNumber;
            HostName = hostName;
            IPAddress = ipAddress;
            IsOnline = isOnline;
            LastSeen = lastSeen;
        }

        public override string ToString()
        {
            return $"Client: {HostName} MAC: {MAC} IP: {IPAddress} isOnline: {IsOnline} lastSeen: {LastSeen}";
        }
    }
}