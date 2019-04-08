using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static LoggingOperation Logger = new LoggingOperation();
        private static GUIServerInterface guiServerInterface = new GUIServerInterface();
        public static ClientOperation ClientRegister = new ClientOperation(guiServerInterface);
        private static GUI gui;
        private static Server server;

        public static void Main(string[] args)
        {
            //MqttBroker broker = new MqttBroker(6666);
            //broker.StartAsync();

            //MqttMaster master = new MqttMaster("192.168.0.171", 6666);
            //master.StartAsync();

            ProoveOfWurzer();
            //POCServer();
        }

        public static void POCLogging()
        {
            LoggingOperation eventlog = new LoggingOperation();
            Console.WriteLine(eventlog.GetType());

            eventlog.Info(eventlog, "Program has been started");
            Console.ReadKey();
            EventLog.Delete("NetWeaver");
        }

        public static void POCServer()
        {
            gui = new GUI(guiServerInterface);
            server = new Server(guiServerInterface);
            Console.WriteLine("Start of Server");
        }

        public static void ProoveOfWurzer()
        {
            InitializeDb();
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