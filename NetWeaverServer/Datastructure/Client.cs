using System;

namespace NetWeaverServer.Datastructure
{
    public class Client
    { //TODO:Bug fixing/testen verinfachen? features für 5mion
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
        
        public Client(string mac, int roomNumber, string hostName, string ipAddress)
        {
            MAC = mac;
            RoomNumber = roomNumber;
            HostName = hostName;
            IPAddress = ipAddress;
            IsOnline = true;
            LastSeen = DateTime.Today.ToString("dd-MM-yyyy");
        }

        /// <summary>Get the ClientData as a String</summary>
        /// <value>Returns a String to display a Client</value>
        public override string ToString()
        {
            return $"Client: {HostName} MAC: {MAC} IP: {IPAddress} isOnline: {IsOnline} lastSeen: {LastSeen}";
        }
    }
}