using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace NetWeaverServer.Datastructure
{
    public class DbConnect
    {
        //TODO:Vereinfachung, automatisches Backup?, json oder csv
        //TODO:Neiche inserts und deletes schreiben
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
                        Console.WriteLine("Invalid username/password, please try again");
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
        public List<List<string>> GetAllClients()
        {
            return Connection.Select("SELECT * FROM client");
        }

        /// <summary>All rooms in the db</summary>
        public static List<List<string>> GetAllRooms()
        {
            return Connection.Select("select * from room;");
        }

        //--------------------------------------------------
        //UPDATEMETHODS
        //--------------------------------------------------


        public void updateClient(List<Client> clients)
        {
            //TODO: testen
            foreach (var client in clients)
            {
                Connection.Update(
                    $"SET foreign_key_checks = 0; UPDATE client SET hostname = '{client.HostName}',ipaddress = '{client.IPAddress}'" +
                    $",fk_pk_roomnumber = '{client.RoomNumber}',last_seen = STR_TO_DATE('{client.LastSeen}', '%d-%m-%Y'),is_online = {client.IsOnline} WHERE pk_macaddr = '{client.MAC}'; SET foreign_key_checks = 1;");
            }
        }

        //--------------------------------------------------
        //INSERTMETHODS
        //--------------------------------------------------
        //TODO: Testen, was passiert wenn ich doppelt inserte
        public static void InsertClient(string pk_macaddr, string hostname, string ipaddress, int fk_pk_roomnumber,
            bool is_online, string last_seen = "")
        {
            var realDate = (String.IsNullOrEmpty(last_seen) ? DateTime.Today.ToString("dd-MM-yyyy") : last_seen);
            Connection.Insert(
                "INSERT INTO client (pk_macaddr, hostname ,ipaddress, fk_pk_roomnumber, last_seen, is_online)" +
                $"VALUES( '{pk_macaddr}', '{hostname}', '{ipaddress}', '{fk_pk_roomnumber}'," +
                $"STR_TO_DATE('{realDate}', '%d-%m-%Y')," + $"{is_online});");
        }

        public static void InsertRoom(int pk_roomNumber, string roomdescription, string netmask, string subnetmask)
        {
            Connection.Insert(
                $"INSERT INTO room (pk_roomNumber, roomdescription ,netmask, subnetmask) VALUES('{pk_roomNumber}', '{roomdescription}', '{netmask}', '{subnetmask}' )");
        }


        //--------------------------------------------------
        //DELETEMETHODS
        //--------------------------------------------------
        //TODO: Für Client Objects neu machen
        /// <summary>Deletes the given client</summary>
        /// <param name='mac'>The mac from the client</param>
        public void DeleteClientByMac(Client client)
        {
            Connection.Delete("DELETE FROM client WHERE pk_macaddr = '" + client.MAC + "';");
        }

        /// <summary>Deletes the given room</summary>
        /// <param name='roomNumber'>The roomnumber from the room</param>
        public void DeleteRoom(Room room)
        {
            Connection.Delete("DELETE FROM room WHERE pk_roomNumber = '" + room.RoomNumber + "';");
        }
    }
}