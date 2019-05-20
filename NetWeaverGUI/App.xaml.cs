using System;
using System.Threading.Tasks;
using System.Windows;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Main;
using NetWeaverServer.MQTT;
using NetWeaverServer.Tasks.Operations;

namespace NetWeaverGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /**
         *                  - - - - Legende - - - - -
         * Task     = User Interaction; handles one Job on many Clients [= JobManager]
         * Job      = (Queue of Commands) Does one Job on ONE Client
         * Job(Cmd) = Executes one small bit of Work (Client or Server); Works on the lowest level
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

        public void StartServer(object sender, StartupEventArgs e)
        {
            //Setup MQTT Server/Broker; Acts like a hub, reposting every publish
            mqttbroker = new MqttBroker(6666);
            Task.Run(() => mqttbroker.StartAsync());

            //Setup MQTT Master; Special MQTT Client that listen to every publish and can respond
            mqttmaster = new MqttMaster("127.0.0.1", 6666);
            Task.Run(() => mqttmaster.StartAsync());
            
            /*//Setup Database Connection; Interface for specific queries
            dbconnection = new DbConnect("192.168.88.254", "mcondb", "root","htl3r", "3333");
            dbInterface = new DBInterface(dbconnection);

            //Setup Main Components; GUI and Server
            GUI = new GUI(eventInterface, dbInterface); // + Database connection*/
            Server = new Server(eventInterface, mqttmaster);
            MainWindow mw = new MainWindow(eventInterface, dbInterface);
            mw.Show();

            //Setup Passive Operations
            //Registration = new ClientOperation(mqttmaster, dbInterface, eventInterface);
            Logger = new LoggingOperation(mqttmaster);

            MessageBox.Show("Start of Server");
        }

        public static void ProoveOfWurzer()
        {
            DbConnect con = new DbConnect("localhost", "mcondb", "root", "htl3r", "3333");
            DBInterface dbi = new DBInterface(con);
            var clients = dbi.getClientList();
            var rooms = dbi.getRoomList();

            // dbi.setOffline("CLIENT_WURZER");

        }
    }
}