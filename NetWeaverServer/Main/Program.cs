using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Tasks.Operations;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Main
{
    class Program
    {
        /**
         *                  - - - - Legende - - - - -
         * Task    = User Interaction; handles one Job on many Clients [= JobManager]
         * Job     = (Queue of Commands) Does one Job on ONE Client
         * Command = Executes one small bit of Work (Client or Server)
         */

        //TODO: Gustl fragen wie man am besten unmanaged resourcen handeln kann (aka. DB, MQTT, EventView)
        //Resources
        private static EventInterface eventInterface = new EventInterface();
        private static DbConnect dbconnection;
        private static DBInterface dbInterface;
        private static MqttBroker mqttbroker;
        private static MqttMaster mqttmaster;

        //Operations
        public static LoggingOperation Logger;
        public static ClientOperation Registration;

        //Main Components
        private static GUI GUI;
        private static Server Server;

        public static void Main(string[] args)
        {
            /*
            MqttBroker broker = new MqttBroker(6666);
            MqttMaster master = new MqttMaster("127.0.0.1", 6666);

            Task.Run(() => broker.StartAsync());
            Task.Run(() => master.StartAsync());*/

            ProoveOfWurzer();
            //POCServer();
           // StartServer();
        }

        public static void StartServer()
        {
            //Setup MQTT Server/Broker; Acts like a hub, reposting every publish
            mqttbroker = new MqttBroker(6666);
            Task.Run(() => mqttbroker.StartAsync());

            //Setup MQTT Master; Special MQTT Client that listen to every publish and can respond
            mqttmaster = new MqttMaster("127.0.0.1", 6666);
            Task.Run(() => mqttmaster.StartAsync());
            
            //Setup Database Connection; Interface for specific queries
            //dbconnection = new DbConnect();
            //dbInterface = new DBInterface(dbconnection);

            //Setup Main Components; GUI and Server
            GUI = new GUI(eventInterface); // + Database connection
            Server = new Server(eventInterface, mqttmaster); // + Database access

            //Setup Passive Operations
            Registration = new ClientOperation(mqttmaster, GUI, dbInterface, eventInterface);
            Logger = new LoggingOperation(mqttmaster);

            Console.WriteLine("- - - - - - - - - -");
            Console.WriteLine("Start of Server");
            Console.WriteLine("- - - - - - - - - -");
        }

        public static void ProoveOfWurzer()
        {
            DbConnect con = new DbConnect("localhost", "mcondb", "root","htl3r", "3333");
            DBInterface dbi = new DBInterface(con);
            var clients = dbi.getClientList();
            var rooms = dbi.getRoomList();

            dbi.deleteClient(clients);

            Console.WriteLine(clients.Count);
            
        }
    }
}