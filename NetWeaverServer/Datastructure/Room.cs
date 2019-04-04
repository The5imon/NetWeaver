using System.Collections.Generic;

namespace NetWeaverServer.Datastructure
{
    public class Room
    {
        private int RoomNumber { get; }
        private string Roomname { get; }
        private List<Client> Clients { get; }
        private string Netmask { get; set; }
        private string Subnetmask { get; set; }

        public Room(int roomnumber, string roomname)
        {
            RoomNumber = roomnumber;
            Roomname = roomname;
        }

        public Room(int roomNumber, string roomname, List<Client> clients, string netmask, string subnetmask)
        {
            RoomNumber = roomNumber;
            Roomname = roomname;
            Clients = clients;
            Netmask = netmask;
            Subnetmask = subnetmask;
        }

        public override string ToString()
        {
            return $"Room: {RoomNumber} Name: {Roomname} Netmask: {Netmask} Subnetmask: {Subnetmask}";
        }
    }
}