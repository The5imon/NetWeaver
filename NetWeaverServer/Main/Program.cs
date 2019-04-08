using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Tasks.Operations;
using NetWeaverServer.MQTT;
using static NetWeaverServer.Datastructure.DbConnect;

namespace NetWeaverServer.Main
{
    class Program
    {
        //TODO: Gustl fragen ob ich lieber diese einpaar Objekte statisch mache
        //Resources
        private static GUIServerInterface guiServerInterface = new GUIServerInterface();
        private static DbConnect dbconnection;
        private static DBDump dbinterface;
        private static MqttBroker mqttbroker;
        private static MqttMaster mqttmaster;
        
        //Operations
        public static LoggingOperation Logger = new LoggingOperation();
        public static ClientOperation ClientRegister = new ClientOperation(guiServerInterface);
        
        //Main Components
        private static GUI gui;
        private static Server server;

        public static void Main(string[] args)
        {
            //ProoveOfWurzer();
            StartServer();
        }

        public static void POCMqtt()
        {
            MqttBroker broker = new MqttBroker(6666);
            Task.Run(() => broker.StartAsync());

            MqttMaster master = new MqttMaster("127.0.0.1", 6666);
            Task.Run(() =>master.StartAsync());
        }

        public static void POCLogging()
        {
            LoggingOperation eventlog = new LoggingOperation();
            Console.WriteLine(eventlog.GetType());

            eventlog.Info(eventlog, "Program has been started");
            Console.ReadKey();
            EventLog.Delete("NetWeaver");
        }

        public static void StartServer()
        {
            mqttbroker = new MqttBroker(6666);
            Task.Run(()=>mqttbroker.StartAsync());
            mqttmaster = new MqttMaster("127.0.0.1", 6666);
            Task.Run(() => mqttmaster.StartAsync());
            
            gui = new GUI(guiServerInterface); // + Database connection
            server = new Server(guiServerInterface, mqttmaster); // + Database access
            Console.WriteLine("Start of Server");
        }

        public static void ProoveOfWurzer()
        {
            InitializeDb();
            DBInterface DBI = new DBInterface(new DbConnect());
            
            
            //var allClients = GetAllClients();
            //List<Client> test = getClientList(allClients);
            //var rooms = GetAllRooms();
            //List<Room> test = getRoomList(rooms);
            
           // foreach (var room in test)
          //  {
               // Console.WriteLine(room.ToString());
           // }

            CloseDb();
        }
    }
}