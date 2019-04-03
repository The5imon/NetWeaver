using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;

namespace NetWeaverServer.GraphicalUI
{
    public class GUI
    {

        private List<Client> clients = new List<Client>();

        private GUIServerInterface EventInt { get; }

        public GUI(GUIServerInterface eventInt)
        {
            EventInt = eventInt;
            new Thread(this.Run).Start();
            clients.Add(new Client("abcd", "EDV", "SimonsPC", "10.0.0.1"));
            clients.Add(new Client("qwer", "EDV", "MaxPC", "10.0.0.2"));
            clients.Add(new Client("yxcv", "EDV", "GregorsPC", "10.0.0.3"));
            clients.Add(new Client("fghj", "EDV", "WurzersPC", "10.0.0.4"));
        }

        public void Run()
        {
            string input = "";

            while(true)
            {
                input = Console.ReadLine();
                Progress<ProgressDetails> progress = new Progress<ProgressDetails>();
                progress.ProgressChanged += ReportProgress;
                MessageDetails md = new MessageDetails(clients, progress);
                switch(input)
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
                        EventInt.triggerClientReplyEvent(); break;
                    case "q": return;
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
