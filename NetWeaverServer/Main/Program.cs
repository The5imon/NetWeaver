﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Tasks.Operations;

using static NetWeaverServer.Datastructure.DbConnect;
using static NetWeaverServer.Datastructure.DBDump;

namespace NetWeaverServer.Main
{
    public class Program
    {
        //TODO: Gustl fragen ob ich lieber diese einpaar Objekte statisch mache
        public static LoggingOperation Logger = new LoggingOperation();
        private static GUIServerInterface guiServerInterface = new GUIServerInterface();
        public static ClientOperation ClientRegister = new ClientOperation(guiServerInterface);
        private static GUI gui;
        private static Server server;

        public static void Main(string[] args)
        {
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
            var allClients = GetAllClients();
            List<Client> test = getClientList(allClients);
            foreach (var client in test)
            {
                Console.WriteLine(client.ToString());
            }
                CloseDb();
        }
    }
}