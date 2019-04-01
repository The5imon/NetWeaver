using System.Collections.Generic;

namespace NetWeaverServer.Datastructure
{
    public class Room
    {
        
        private int roomNumber { get; }
        private string Roomname { get; }
        
        private List<Client> Clients { get; }
        
        private string subnetmask 

        
        
        
        
        
        
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