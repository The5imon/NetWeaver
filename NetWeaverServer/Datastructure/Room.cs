using System.Collections.Generic;

namespace NetWeaverServer.Datastructure
{
    public class Room
    {
        public string Roomname { get; set; }
        
        public List<Client> Clients { get; }

        public Room(string roomname)
        {
            Roomname = roomname;
        }

        public Room(string roomname, List<Client> clients)
        {
            Roomname = roomname;
            Clients = clients;
        }
    }
}