using System;
using System.Collections.Generic;

namespace NetWeaverServer.Datastructure
{
    public class DBInterface
    {
        private DbConnect DataBase;
        public List<Client> Clients = new List<Client>();
        public List<Room> Rooms = new List<Room>();
        

        public DBInterface(DbConnect DB)
        {
            DataBase = DB;
        }

        public  List<Client> getClientList()
        {  
            return parseClientList(Parse(DataBase.GetAllClients()));
        }

        public static List<String> Parse(List<List<String>> dataList)
        {
            List<String> values = new List<string>();

            foreach (var clientss in dataList)
            {
                values.Add(String.Join("~", clientss));
            }

            return values;
        }

        public static List<Client> parseClientList(List<String> dataString)
        {
            List<Client> clients = new List<Client>();

            foreach (var clientData in dataString)
            {
                clients.Add(createClient(clientData));
            }

            return clients;
        }

        public static List<Room> parseRoomList(List<String> dataString)
        {
            List<Room> rooms = new List<Room>();

            foreach (var clientData in dataString)
            {
                rooms.Add(createRoom(clientData));
            }

            return rooms;
        }

        public Client createClient(String clientData)
        {
            string mac = clientData.Split('~')[0];
            string ipAddress = clientData.Split('~')[1];
            string hostName = clientData.Split('~')[2];
            int roomNumber = Int32.Parse(clientData.Split('~')[3]);
            string lastSeen = clientData.Split('~')[4];
            bool isOnline = bool.Parse(clientData.Split('~')[5]);

            foreach (var room in this.Rooms)
            {
                if (expr)
                {
                    
                }
            }
            
                
            

            return new Client(mac, hostName, ipAddress, isOnline, lastSeen);
        }

        public static Room createRoom(String roomData)
        {
            int RoomNumber = Int32.Parse(roomData.Split('~')[0]);
            string Roomname = roomData.Split('~')[1];
            string Netmask = roomData.Split('~')[2];
            string Subnetmask = roomData.Split('~')[3];

            return new Room(RoomNumber, Roomname, Netmask, Subnetmask);
        }
    }
}