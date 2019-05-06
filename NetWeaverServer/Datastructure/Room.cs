using System.Collections.Generic;

namespace NetWeaverServer.Datastructure
{
    public class Room
    {
        public int RoomNumber { get; }
        public string Roomname { get; }
        public string Netmask { get; }
        public string Subnetmask { get; }

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