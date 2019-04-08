using System.Collections.Generic;

namespace NetWeaverServer.Datastructure
{
    public class Room
    { //TODO: erweitern,Bugfixing,featureset erweitern, testen
        public int RoomNumber { get; }
        private string Roomname { get; }
        private List<Client> Clients { get; }
        private string Netmask { get; set; }
        private string Subnetmask { get; set; }

        public Room(int roomnumber, string roomname)
        {
            RoomNumber = roomnumber;
            Roomname = roomname;
        }
        
        public Room(int roomnumber)
        {
            RoomNumber = roomnumber;
            Roomname = "Room " + roomnumber;
        }

        public Room(int roomNumber, string roomname, string netmask, string subnetmask)
        {
            RoomNumber = roomNumber;
            Roomname = roomname;
            Netmask = netmask;
            Subnetmask = subnetmask;
        }

        public override string ToString()
        {
            return $"Room: {RoomNumber} Name: {Roomname} Netmask: {Netmask} Subnetmask: {Subnetmask}";
        }
    }
}