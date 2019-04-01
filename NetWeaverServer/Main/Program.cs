using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Jobs;

namespace NetWeaverServer.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            POCServer();
        }

        public static void POCLogging()
        {
            LoggingOperation eventlog = new LoggingOperation("MCONServer");
            Console.WriteLine(eventlog.GetType());

            eventlog.Info(eventlog, "Program has been started");
            Console.ReadKey();
            EventLog.Delete("MCON");
        }

        public static void POCServer()
        {
            GUIServerInterface guiServerInterface = new GUIServerInterface();
            GUI gui = new GUI(guiServerInterface);
            Server server = new Server(guiServerInterface);
            Console.WriteLine("Start of Server");
        }
    }
}
