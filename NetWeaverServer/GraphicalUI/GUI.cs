using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;

using static NetWeaverServer.Tasks.Operations.LoggingOperation;

namespace NetWeaverServer.GraphicalUI
{
    public class GUI
    {
        private List<Client> clients = new List<Client>();
        private GUIServerInterface EventInt { get; }

        public GUI(GUIServerInterface eventInt)
        {
            EventInt = eventInt;
            new Thread(Run).Start();
            clients.Add(new Client("abcd", 1, "SimonsPC", "10.0.0.1", true, ""));
            clients.Add(new Client("qwer", 1, "MaxPC", "10.0.0.2", true, ""));
            clients.Add(new Client("yxcv", 1, "GregorsPC", "10.0.0.3", true, ""));
            clients.Add(new Client("fghj", 1, "WurzersPC", "10.0.0.4", true, ""));
        }

        public void Run()
        {
            string input = "";

            while(true)
            {
                input = Console.ReadLine();
                string[] args = input.Split(":");
                Progress<ProgressDetails> progress = new Progress<ProgressDetails>();
                progress.ProgressChanged += ReportProgress;
                MessageDetails md = new MessageDetails(clients, progress);
                switch(args[0])
                {
                    case "copy":
                        /**
                         * Event Trigger kann als direkter Zugriff durch den Server ersetzt werden
                         *     - weniger Abspaltung (Event Trigger/ Async Handler ==> Komplette Abspaltung)
                         *     - bzw. maybe unschön
                         */
                        EventInt.triggerCopyFileEvent(md);
                        break;
                    case "reply":
                        EventInt.triggerClientReplyEvent();
                        break;
                    case "client":
                        EventInt.newClientEvent(new ClientDetails{Client = args[1]});
                        break;
                    case "q":
                        //Logger.Delete();
                        return;
                }
            }
        }

        private void ReportProgress(object sender, ProgressDetails e)
        {
            Console.WriteLine("Reporting List of finished clients");
            foreach (Client client in e.Clients)
            {
                Console.WriteLine($"\r\t PC - [{client}] is finished");
            }
        }
    }
}
