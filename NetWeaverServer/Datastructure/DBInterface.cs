using System;
using System.Collections.Generic;
using System.Threading;

namespace NetWeaverServer.Datastructure
{
    public class DBInterface
    {
        private DbConnect DataBase;
        private List<Client> Clients = new List<Client>();
        private List<Room> Rooms = new List<Room>();

        //TODO: Konzentriere dich lieber auf logische Hintergründe der Datenbank
        /**
         * Denk einfach dis ich vielleicht nur die getClients und addClients methoden benutzen werde
         *     - wenn ich client mit selber MAC adden will --> Updaten
         *     - erstelle Rooms aus den subnets der Clients
         *         (ich werde nie selber so einen Room erstellen; es ist schön die Option zu haben aber arbeite eher selbe mit deinen Tools)
         */

        public DBInterface(DbConnect DB)
        {
            DataBase = DB;
            Thread.Sleep(20);
            getAllRooms();
            getAllClients();
        }

        //TODO: Nenn es lieber nicht getClientList sonder getAllRooms/getAllCLients oder so was (für alles)


        #region listParse

        public void getAllClients()
        {
            DataBase.OpenConnection();
            var clientData = DataBase.GetAllClients();
            DataBase.CloseConnection();
            getAllClients(Parse(clientData));
        }

        public void getAllRooms()
        {
            DataBase.OpenConnection();
            var roomData = DataBase.GetAllRooms();
            DataBase.CloseConnection();
            getAllRooms(Parse(roomData));
        }

        #endregion

        #region DatabaseControl

        public void updateClients(List<Client> clients)
        {
            DataBase.OpenConnection();
            foreach (var client in clients)
            {
                DataBase.updateClient(client);
            }
            
            DataBase.CloseConnection();
            emptyLists();
            getAllClients();
            getAllRooms();
        }

        public void updateSingleClient(Client client)
        {
            DataBase.OpenConnection();

            DataBase.updateClient(client);
            
            DataBase.CloseConnection();
            emptyLists();
            getAllClients();
            getAllRooms();
        }

        public void updateRooms(List<Room> rooms)
        {
            DataBase.OpenConnection();
            foreach (var room in rooms)
            {
                DataBase.updateRoom(room);
            }
            
            
            DataBase.CloseConnection();
            emptyLists();
            getAllClients();
            getAllRooms();
        }

        //AddClient
        public void insertClients(List<Client> clients)
        {
            DataBase.OpenConnection();
            foreach (var client in clients)
            {
                if (isInClientList(client.MAC))
                {
                    updateSingleClient(client);
                }
                else
                {
                    DataBase.InsertClient(client);
                }
            }
            
            DataBase.CloseConnection();
            emptyLists();
            getAllClients();
            getAllRooms();
        }

        public void insertRooms(List<Room> rooms)
        {
            DataBase.OpenConnection();
            foreach (var room in rooms)
            {
                DataBase.InsertRoom(room);
            }
           
            DataBase.CloseConnection();
            emptyLists();
            getAllClients();
            getAllRooms();
        }

        public void deleteClients(List<Client> clients)
        {
            DataBase.OpenConnection();
            foreach (var client in clients)
            {
                DataBase.DeleteClient(client);
            }
            DataBase.CloseConnection();
            emptyLists();
            getAllClients();
            getAllRooms();
        }
        
        public void deleteClientByHostname(string hostname)
        {
            DataBase.OpenConnection();
            foreach (Client client in Clients)
            {
                if (client.HostName.Equals(hostname))
                {
                    DataBase.DeleteClient(client);
                }
            }
            DataBase.CloseConnection();
            emptyLists();
            getAllClients();
            getAllRooms();
        }

        public void deleteRoom(List<Room> rooms)
        {
            DataBase.OpenConnection();
            foreach (var room in rooms)
            {
                DataBase.DeleteRoom(room);
            }
            emptyLists();
            getAllClients();
            getAllRooms();
            DataBase.CloseConnection();
        }

        #endregion


        public static List<String> Parse(List<List<String>> dataList)
        {
            List<String> values = new List<string>();

            foreach (var clients in dataList)
            {
                values.Add(String.Join("~", clients));
            }

            return values;
        }

        public void getAllClients(List<String> dataString)
        {
            foreach (var clientData in dataString)
            {
                Clients.Add(createClient(clientData));
            }
        }

        public void getAllRooms(List<String> dataString)
        {
            foreach (var roomData in dataString)
            {
                Rooms.Add(createRoom(roomData));
            }
        }


        private Client createClient(String clientData)
        {
            string mac = clientData.Split('~')[0];
            string ipAddress = clientData.Split('~')[1];
            string hostName = clientData.Split('~')[2];
            int roomNumber = Int32.Parse(clientData.Split('~')[3]);
            string lastSeen = clientData.Split('~')[4];
            bool isOnline = bool.Parse(clientData.Split('~')[5]);

            if (isInRoomList(roomNumber) == false)
            {
                Rooms.Add(new Room(roomNumber));
            }

            return new Client(mac, roomNumber, hostName, ipAddress, isOnline, lastSeen);
        }

        public List<Client> getClientList()
        {
            return Clients;
        }

        public List<Room> getRoomList()
        {
            return Rooms;
        }

        private Room createRoom(String roomData)
        {
            int RoomNumber = Int32.Parse(roomData.Split('~')[0]);
            string Roomname = roomData.Split('~')[1];
            string Netmask = roomData.Split('~')[2];
            string Subnetmask = roomData.Split('~')[3];

            return new Room(RoomNumber, Roomname, Netmask, Subnetmask);
        }

        private bool isInClientList(string mac)
        {
            foreach (Client client in Clients)
            {
                if (client.RoomNumber.Equals(mac))
                {
                    return true;
                }
            }

            return false;
        }

        private bool isInRoomList(int roomNumber)
        {
            foreach (Room room in Rooms)
            {
                if (room.RoomNumber == roomNumber)
                {
                    return true;
                }
            }

            return false;
        }

        private void emptyLists()
        {
         Rooms.Clear();
         Clients.Clear();
        }
    }
}