using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace NetWeaverServer.Datastructure
{
    class DbConnect
    {
        //The connection
        public static DbConnect Connection;

        //The MySQL connection
        private MySqlConnection _connection;

        //The ipaddress for the server
        private string _server;

        //The database name
        private string _database;

        //The username for the database login
        private string _uid;

        //The password for the user
        private string _password;

        /// <summary>The Constructor calls the initialize method</summary>
        public DbConnect()
        {
            Initialize();
        }

        /// <summary>Initialises the Connection</summary>
        public static void InitializeDb()
        {
            Connection = new DbConnect();
        }

        /// <summary>Initialises the values for the connection to the database</summary>
        private void Initialize()
        {
            _server = "192.168.230.131";
            _database = "mcondb";
            _uid = "remote";
            _password = "htl3r";
            string connectionString;
            connectionString = "SERVER=" + _server + ";" + "DATABASE=" + _database + ";" + "UID=" + _uid + ";" +
                               "PASSWORD=" + _password + ";";

            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }


        /// <summary>Opens a connection to the given database</summary>
        private bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid Username/password, please try again");
                        break;
                }

                return false;
            }
        }

        /// <summary>Closes the database connection and nullifies it</summary>
        public static void CloseDb()
        {
            Connection.CloseConnection();
            Connection = null;
        }

        /// <summary>Closes the databease connection</summary>
        private void CloseConnection()
        {
            try
            {
                _connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>Executes a insert statement</summary>
        /// <param name='qu'>The query</param>
        private void Insert(string qu)
        {
            string query = qu;
            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(query, _connection);

            //Execute command
            cmd.ExecuteNonQuery();
        }

        /// <summary>Executes a update statement</summary>
        /// <param name='qu'>The query</param>
        private void Update(String qu)
        {
            string query = qu;

            //create mysql command
            MySqlCommand cmd = new MySqlCommand();
            //Assign the query using CommandText
            cmd.CommandText = query;
            //Assign the connection using Connection
            cmd.Connection = _connection;

            //Execute query
            cmd.ExecuteNonQuery();
        }

        /// <summary>Executes a delete statement</summary>
        /// <param name='qu'>The query</param>
        private void Delete(string qu)
        {
            string query = qu;

            MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
        }

        /// <summary>Executes a select statement</summary>
        /// <param name='qu'>The query</param>
        private List<List<string>> Select(string qu)
        {
            string query = qu;

            //Create a list to store the result
            List<List<string>> list = new List<List<string>>();


            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, _connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                List<string> tmp = new List<string>();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    tmp.Add(dataReader.GetString(i));
                }

                list.Add(tmp);
            }

            //close Data Reader
            dataReader.Close();

            //return list to be displayed
            return list;
        }

        //--------------------------------------------------
        //SELECTMETHODS
        //--------------------------------------------------

        /// <summary>Every column from every client in the database</summary>
        public static List<List<string>> GetAllClients()
        {
            return Connection.Select("SELECT * FROM client");
        }


        /// <summary>Client name, mac ip, roomnumber and roomname</summary>
        public static List<List<string>> GetAllClientsWithRoom()
        {
            return Connection.Select("SELECT  hostname, pk_macaddr ,ipaddress,roomdescription,pk_roomNumber " +
                                     "FROM client,room WHERE fk_pk_roomnumber = pk_roomNumber");
        }

        /// <summary>Hostname, mac , ip  and roominformation</summary>
        /// <param name='roomNumber'>The number of the room</param>
        public static List<List<string>> GetAllClientsByRoomNumber(int roomNumber)
        {
            return Connection.Select("select hostname, pk_macaddr, ipaddress, roomdescription, fk_pk_roomnumber " +
                                     " from client INNER JOIN room  ON fk_pk_roomnumber =  pk_roomnumber and pk_roomnumber  = '131'");
        }

        /// <summary>All clients in the DB with mac, hostname and ip</summary>
        public static List<List<string>> GetAllClientsWithIp()
        {
            return Connection.Select("select pk_macaddr , hostname , ipaddress from client;");
        }

        /// <summary>All clients in the DB with mac and hostname</summary>
        public static List<List<string>> GetAllClientsWithMac()
        {
            return Connection.Select("select pk_macaddr , hostname from client;");
        }

        /// <summary>All ip addresses in the DB</summary>
        public static List<List<string>> GetAllIP()
        {
            return Connection.Select("select ipaddress from client;");
        }

        /// <summary>All mac addresses in the DB</summary>
        public static List<List<string>> GetAllMac()
        {
            return Connection.Select("select pk_macaddr from client;");
        }

        /// <summary>All online clients with hostame and ip</summary>
        public List<List<string>> GetAllOnlineWithNameAndIp()
        {
            return Connection.Select("select hostname , ipaddress from client WHERE is_online = true;");
        }

        /// <summary>Every client and his last senn date</summary>
        public static List<List<string>> GetAllLastSeenWithName()
        {
            return Connection.Select("select hostname , last_seen from client;");
        }

        /// <summary>Every offline client with hostname and roomname</summary>
        public static List<List<string>> GetAllOfflineWithNameAndRoom()
        {
            return Connection.Select(
                "select hostname , last_seen ,roomdescription from client,room where " +
                "fk_pk_roomnumber = pk_roomNumber AND is_online = false;");
        }

        /// <summary>All rooms in the db</summary>
        public static List<List<string>> GetAllRooms()
        {
            return Connection.Select("select pk_roomNumber,roomdescription from room;");
        }

        /// <summary>Subnet information for all rooms</summary>
        public static List<List<string>> GetAllSubnetsWithRooms()
        {
            return Connection.Select(
                "select netmask AS 'Netmask', subnetmask As 'Subnetmask' , roomdescription from room;");
        }

        /// <summary>Get client hostname, mac, ip by its ip</summary>
        /// /// <param name='ip'>The ip from the computer</param>
        public static List<List<string>> GetPcByIp(string ip)
        {
            return Connection.Select("select hostname , pk_macaddr , ipaddress from client where ipaddress = '" + ip +
                                     "';");
        }

        /// <summary>Get client hostname, mac, ip by its mac</summary>
        /// <param name='mac'>The mac from the computer</param>
        public static List<List<string>> GetPcByMac(string mac)
        {
            return Connection.Select("select hostname,pk_macaddr , ipaddress from client where pk_macaddr = '" + mac +
                                     "';");
        }

        /// <summary>Get the room information by an ip</summary>
        /// /// <param name='ip'>The ip address for the client or room</param>
        public static List<List<string>> GetRoomByIp(string ip)
        {
            string netip = ip.Split('.')[0] + "." + ip.Split('.')[1] + "." +
                           ip.Split('.')[2] + "." + "0";
            return Connection.Select(
                "select roomdescription , pk_roomNumber from room where netmask = '" + netip + "';");
        }

        /// <summary>Get ever pice of information for this room</summary>
        /// <param name='roomNumber'>The number of the room</param>
        public static List<List<string>> GetRoomInformationByRoomNumber(int roomNumber)
        {
            return Connection.Select("select * from room where pk_roomNumber = '" + roomNumber + "'");
        }

        //--------------------------------------------------
        //UPDATEMETHODS
        //--------------------------------------------------

        /// <summary>Changes the ip from the given client</summary>
        /// <param name='hostname'>The hostname from the client</param>
        /// <param name='newIp'>The new ip for the client</param>
        public static void UpdateIpFromClientHostname(string hostname, string newIp)
        {
            Connection.Update("UPDATE client SET ipaddress = '" + newIp + "' WHERE hostname = '" + hostname + "';");
        }

        /// <summary>Changes the ip from the given client</summary>
        /// <param name='mac'>The mac from the client</param>
        /// <param name='newIP'>The new ip for the client</param>
        public static void UpdateIpFromClientMac(string mac, string newIP)
        {
            Connection.Update("UPDATE client SET ipaddress = '" + newIP + "' WHERE pk_macaddr = '" + mac + "';");
        }

        /// <summary>Changes the room from the given client</summary>
        /// <param name='mac'>The mac from the client</param>
        /// <param name='newRoomNumber'>The new room for the client</param>
        public static void UpdateRoomFromMac(string mac, int newRoomNumber)
        {
            Connection.Update("UPDATE client SET fk_pk_roomnumber = '" + newRoomNumber + "' WHERE pk_macaddr = '" +
                              mac + "';");
        }

        /// <summary>Changes the room from the given client</summary>
        /// <param name='ip'>The ip from the client</param>
        /// <param name='newRoomNumber'>The new room for the client</param>
        public static void UpdateRoomFromIP(string ip, int newRoomNumber)
        {
            Connection.Update("UPDATE client SET fk_pk_roomnumber = '" + newRoomNumber + "' WHERE ipaddress = '" + ip +
                              "';");
        }

        /// <summary>Changes the room from the given client</summary>
        /// <param name='hostname'>The hostname from the client</param>
        /// <param name='newRoomNumber'>The new room for the client</param>
        public static void UpdateRoomFromHostname(string hostname, int newRoomNumber)
        {
            Connection.Update("UPDATE client SET fk_pk_roomnumber = '" + newRoomNumber + "' WHERE hostname = '" +
                              hostname + "';");
        }

        /// <summary>Changes the hostname from the given client</summary>
        /// <param name='ip'>The ip from the client</param>
        /// <param name='newHostname'>The new hostname for the client</param>
        public static void UpdateHostnameByIp(string ip, string newHostname)
        {
            Connection.Update("UPDATE client SET hostname = '" + newHostname + "' WHERE ipaddress = '" + ip + "';");
        }

        /// <summary>Changes the hostname from the given client</summary>
        /// <param name='mac'>The mac from the client</param>
        /// <param name='newHostname'>The new hostname for the client</param>
        public static void UpdateHostnameByMac(string mac, string newHostname)
        {
            Connection.Update("UPDATE client SET hostname = '" + newHostname + "' WHERE pk_macaddr = '" + mac + "';");
        }

        /// <summary>Changes the last_seen date from the given client</summary>
        /// <param name='mac'>The mac from the client</param>
        /// <param name='last_seen'>The new last seen date for the client</param>
        public static void UpdateLastSeenByMac(string mac, string last_seen)
        {
            Connection.Update("UPDATE client SET last_seen = STR_TO_DATE('" + last_seen +
                              "', '%d-%m-%Y')  WHERE pk_macaddr = '" + mac + "';");
        }

        /// <summary>Changes the last_seen date from the given client</summary>
        /// <param name='ip'>The ip from the client</param>
        /// <param name='last_seen'>The new last seen date for the client</param>
        public static void UpdateLastSeenByIp(string ip, string last_seen)
        {
            Connection.Update("UPDATE client SET last_seen = STR_TO_DATE('" + last_seen +
                              "', '%d-%m-%Y')  WHERE ipaddress = '" + ip + "';");
        }

        /// <summary>Changes the last_seen date from the given client</summary>
        /// <param name='hostname'>The hostname from the client</param>
        /// <param name='last_seen'>The new last seen date for the client</param>
        public static void UpdateLastSeenByHostname(string hostname, string last_seen)
        {
            Connection.Update("UPDATE client SET last_seen = STR_TO_DATE('" + last_seen +
                              "', '%d-%m-%Y')  WHERE hostname = '" + hostname + "';");
        }

        /// <summary>Changes the is_online date from the given client</summary>
        /// <param name='mac'>The mac from the client</param>
        /// <param name='isOnline'>The new is_online state for the client</param>
        public static void UpdateIsOnlineByMac(bool isOnline, string mac)
        {
            Connection.Update("UPDATE client SET is_online = " + isOnline + " WHERE pk_macaddr = '" + mac + "';");
        }

        /// <summary>Changes the is_online date from the given client</summary>
        /// <param name='hostname'>The hostname from the client</param>
        /// <param name='isOnline'>The new is_online state for the client</param>
        public static void UpdateIsOnlineByHostname(bool isOnline, string hostname)
        {
            Connection.Update("UPDATE client SET is_online = " + isOnline + " WHERE hostname = '" + hostname + "';");
        }

        /// <summary>Changes the is_online date from the given client</summary>
        /// <param name='ip'>The ip from the client</param>
        /// <param name='isOnline'>The new is_online state for the client</param>
        public static void UpdateIsOnlineByIp(bool isOnline, string ip)
        {
            Connection.Update("UPDATE client SET is_online = " + isOnline + " WHERE ipaddress = '" + ip + "';");
        }

        /// <summary>Changes the roomdescription from the given room</summary>
        /// <param name='roomNumber'>The RoomNumber from the room</param>
        /// <param name='roomdescr'>The new roomdescription for the room</param>
        public static void ChangeRoomDescr(string roomdescr, int roomNumber)
        {
            Connection.Update("UPDATE room SET roomdescription = '" + roomdescr + "' WHERE pk_roomNumber = '" +
                              roomNumber + "';");
        }

        /// <summary>Changes the netmask from the given room</summary>
        /// <param name='roomNumber'>The RoomNumber from the room</param>
        /// <param name='netmask'>The new netmask for the room</param>
        public static void ChangeNetmask(string netmask, int roomNumber)
        {
            Connection.Update("UPDATE room SET netmask = '" + netmask + "' WHERE pk_roomNumber = '" + roomNumber +
                              "';");
        }

        /// <summary>Changes the subnetmask from the given room</summary>
        /// <param name='roomNumber'>The RoomNumber from the room</param>
        /// <param name='subnetmask'>The new subnetmask for the room</param>
        public static void ChangeSubnetmask(string subnetmask, int roomNumber)
        {
            Connection.Update("UPDATE room SET subnetmask = '" + subnetmask + "' WHERE pk_roomNumber = '" + roomNumber +
                              "';");
        }

        //--------------------------------------------------
        //INSERTMETHODS
        //--------------------------------------------------

        /// <summary>Inserts a Client into the database</summary>
        /// <param name='pk_macaddr'>The mac from the client</param>
        /// <param name='hostname'>The hostname from the client</param>
        /// <param name='ipaddress'>The ip from the client</param>
        /// <param name='fk_pk_roomnumber'>The roomnumber from the client</param>
        /// <param name='is_online'>The online status from the client</param>
        /// <param name='last_seen'>The last seen date from the client</param>
        public static void InsertClient(string pk_macaddr, string hostname, string ipaddress, int fk_pk_roomnumber,
            bool is_online, string last_seen = "")
        {
            var realDate = (String.IsNullOrEmpty(last_seen) ? DateTime.Today.ToString("dd-MM-yyyy") : last_seen);
            Connection.Insert(
                $"INSERT INTO client (pk_macaddr, hostname ,ipaddress, fk_pk_roomnumber, last_seen, is_online)" +
                $"VALUES( '{pk_macaddr}', '{hostname}', '{ipaddress}', '{fk_pk_roomnumber}'," +
                $"STR_TO_DATE('{realDate}', '%d-%m-%Y')," + $"{is_online});");
        }

        /// <summary>Inserts a Client into the database</summary>
        /// <param name='pk_roomNumber'>The roomnumber from the room</param>
        /// <param name='roomdescription'>The roomdecription for the room</param>
        /// <param name='netmask'>The netmask for the room</param>
        /// <param name='subnetmask'>The netmask for the room</param>
        public static void InsertRoom(int pk_roomNumber, string roomdescription, string netmask, string subnetmask)
        {
            Connection.Insert(
                $"INSERT INTO room (pk_roomNumber, roomdescription ,netmask, subnetmask)" +
                " VALUES('{pk_roomNumber}', '{roomdescription}', '{netmask}', '{subnetmask}' )");
        }


        //--------------------------------------------------
        //DELETEMETHODS
        //--------------------------------------------------


        /// <summary>Deletes the given client</summary>
        /// <param name='ip'>The ip from the client</param>
        public static void DeleteClientByIP(string ip)
        {
            Connection.Delete("DELETE FROM client WHERE ipaddress = '" + ip + "';");
        }

        /// <summary>Deletes the given client</summary>
        /// <param name='mac'>The mac from the client</param>
        public static void DeleteClientByMac(string mac)
        {
            Connection.Delete("DELETE FROM client WHERE pk_macaddr = '" + mac + "';");
        }

        /// <summary>Deletes the given client</summary>
        /// <param name='hostname'>The hostname from the client</param>
        public static void DeleteClientByHostname(string hostname)
        {
            Connection.Delete("DELETE FROM client WHERE hostname = '" + hostname + "';");
        }

        /// <summary>Deletes the given room</summary>
        /// <param name='roomNumber'>The roomnumber from the room</param>
        public static void DeleteRoom(int roomNumber)
        {
            Connection.Delete("DELETE FROM room WHERE pk_roomNumber = '" + roomNumber + "';");
        }
    }
}