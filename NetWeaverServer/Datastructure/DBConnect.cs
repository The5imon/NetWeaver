using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace NetWeaverServer.Datastructure
{
    public class DbConnect
    {
        //TODO:Vereinfachung, automatisches Backup?, json oder csv

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

        //The port for the connection
        private string _port;

        /// <summary>The Constructor calls the initialize method</summary>
        public DbConnect(string server, string database, string uid, string password, string port)
        {
            Initialize(server, database, uid, password, port);
        }

        /// <summary>Initialises the values for the connection to the database</summary>
        private void Initialize(string server, string database, string uid, string password, string port)
        {
            _server = server;
            _database = database;
            _uid = uid;
            _password = password;
            _port = port;
            string connectionString;
            connectionString = $"SERVER={_server};Port={_port};Database={_database};Uid={_uid};Pwd={_password};";
            _connection = new MySqlConnection(connectionString); //TODO: Testen
        }


        public bool OpenConnection()
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
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }

                return false;
            }
        }

        /// <summary>Closes the database connection</summary>
        public void CloseConnection()
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
        public List<List<string>> GetAllClients()
        {
            var data = Select("SELECT * FROM client");
            return data;
        }

        /// <summary>All rooms in the db</summary>
        public List<List<string>> GetAllRooms()
        {
            return Select("select * from room;");
        }

        //--------------------------------------------------
        //UPDATEMETHODS
        //--------------------------------------------------


        public void updateClient(Client client)
        {
            //TODO: testen

            Update(
                $"SET foreign_key_checks = 0; UPDATE client SET hostname = '{client.HostName}',ipaddress = '{client.IPAddress}'" +
                $",fk_pk_roomnumber = '{client.RoomNumber}',last_seen = STR_TO_DATE('{client.LastSeen}', '%d-%m-%Y'),is_online = {client.IsOnline} WHERE pk_macaddr = '{client.MAC}'; SET foreign_key_checks = 1;");
        }

        public void updateRoom(Room room)
        {
            //TODO: testen

            Update(
                $"SET foreign_key_checks = 0; UPDATE room SET roomdescription = '{room.Roomname}',netmask = '{room.Netmask}'" +
                $",subnetmask = '{room.Subnetmask}' WHERE pk_roomNumber = '{room.RoomNumber}'; SET foreign_key_checks = 1;");
        }

        //--------------------------------------------------
        //INSERTMETHODS
        //--------------------------------------------------
        //TODO: Testen, was passiert wenn ich doppelt inserte
        public void InsertClient(Client client)
        {
            Insert(
                "INSERT INTO client (pk_macaddr, hostname ,ipaddress, fk_pk_roomnumber, last_seen, is_online)" +
                $"VALUES( '{client.MAC}', '{client.HostName}', '{client.IPAddress}', '{client.RoomNumber}'," +
                $"STR_TO_DATE('{client.LastSeen}', '%d-%m-%Y')," + $"{client.IsOnline});");
        }

        public void InsertRoom(Room room)
        {
            Insert(
                $"INSERT INTO room (pk_roomNumber, roomdescription ,netmask, subnetmask) " + "" +
                $"VALUES('{room.RoomNumber}', '{room.Roomname}', '{room.Netmask}', '{room.Subnetmask}' )");
        }


        //--------------------------------------------------
        //DELETEMETHODS
        //--------------------------------------------------
        //TODO: Für Client Objects neu machen
        /// <summary>Deletes the given client</summary>
        /// <param name='mac'>The mac from the client</param>
        public void DeleteClient(Client client)
        {
            Delete($"DELETE FROM client WHERE pk_macaddr = '{client.MAC}';");
        }

        /// <summary>Deletes the given room</summary>
        /// <param name='roomNumber'>The roomnumber from the room</param>
        public void DeleteRoom(Room room)
        {
            Delete($"DELETE FROM room WHERE pk_roomNumber = '{room.RoomNumber}';");
        }
    }
}