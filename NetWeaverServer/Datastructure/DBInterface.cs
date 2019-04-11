using System;
using System.Collections.Generic;

namespace NetWeaverServer.Datastructure
{
    public class DBInterface
    {
        private DbConnect DataBase;
        public List<Client> Clients = new List<Client>();
        public List<Room> Rooms = new List<Room>();
        private List<int> roomNumbers = new List<int>();


        public DBInterface(DbConnect DB)
        {
            DataBase = DB;
        }

        public List<Client> getClientList()
        {
            DataBase.OpenConnection();
            var clientData = DataBase.GetAllClients();
            DataBase.CloseConnection();
            return parseClientList(Parse(clientData));
        }

        public List<Room> getRoomList()
        {
            DataBase.OpenConnection();
            var roomData = DataBase.GetAllRooms();
            DataBase.CloseConnection();
            return parseRoomList(Parse(roomData));
        }

        public void updateClient(List<Client> clients)
        {
            DataBase.OpenConnection();
            foreach (var client in clients)
            {
                DataBase.updateClient(client);
            }

            DataBase.CloseConnection();
        }

        public void updateRoom(List<Room> rooms)
        {
            DataBase.OpenConnection();
            foreach (var room in rooms)
            {
                DataBase.updateRoom(room);
            }

            DataBase.CloseConnection();
        }

        public void insertClient(List<Client> clients)
        {
            DataBase.OpenConnection();
            foreach (var client in clients)
            {
                DataBase.InsertClient(client);
            }

            DataBase.CloseConnection();
        }

        public void insertRoom(List<Room> rooms)
        {
            DataBase.OpenConnection();
            foreach (var room in rooms)
            {
                DataBase.InsertRoom(room);
            }

            DataBase.CloseConnection();
        }

        public void deleteClient(List<Client> clients)
        {
            DataBase.OpenConnection();
            foreach (var client in clients)
            {
                DataBase.DeleteClient(client);
            }

            DataBase.CloseConnection();
        }

        public void deleteRoom(List<Room> rooms)
        {
            DataBase.OpenConnection();
            foreach (var room in rooms)
            {
                DataBase.DeleteRoom(room);
            }

            DataBase.CloseConnection();
        }

        public static List<String> Parse(List<List<String>> dataList)
        {
            List<String> values = new List<string>();

            foreach (var clients in dataList)
            {
                values.Add(String.Join("~", clients));
            }

            return values;
        }

        public List<Client> parseClientList(List<String> dataString)
        {
            List<Client> clients = new List<Client>();

            foreach (var clientData in dataString)
            {
                clients.Add(createClient(clientData));
            }

            return clients;
        }

        public List<Room> parseRoomList(List<String> dataString)
        {
            List<Room> rooms = new List<Room>();

            foreach (var roomData in dataString)
            {
                rooms.Add(createRoom(roomData));
            }

            return rooms;
        }


        private Client createClient(String clientData)
        {
            string mac = clientData.Split('~')[0];
            string ipAddress = clientData.Split('~')[1];
            string hostName = clientData.Split('~')[2];
            int roomNumber = Int32.Parse(clientData.Split('~')[3]);
            string lastSeen = clientData.Split('~')[4];
            bool isOnline = bool.Parse(clientData.Split('~')[5]);

            if (!roomNumbers.Contains(roomNumber))
            {
                Rooms.Add(new Room(roomNumber));
            }

            return new Client(mac, hostName, ipAddress, isOnline, lastSeen);
        }

        private Room createRoom(String roomData)
        {
            int RoomNumber = Int32.Parse(roomData.Split('~')[0]);
            string Roomname = roomData.Split('~')[1];
            string Netmask = roomData.Split('~')[2];
            string Subnetmask = roomData.Split('~')[3];

            return new Room(RoomNumber, Roomname, Netmask, Subnetmask);
        }
    }
}