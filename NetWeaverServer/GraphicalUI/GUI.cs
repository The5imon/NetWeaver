using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetWeaverServer.GraphicalUI
{
    public class GUI
    {

        private List<string> clients = new List<string>();

        private GUIServerInterface EventInt { get; }

        public GUI(GUIServerInterface eventInt)
        {
            EventInt = eventInt;
            new Thread(this.Run).Start();
            clients.Add("simon");
            clients.Add("gregor");
            clients.Add("max");
            clients.Add("wurzer");
        }

        public void Run()
        {
            string input = "";

            while(true)
            {
                input = Console.ReadLine();
                switch(input)
                {
                    case "copy":
                        /**
                         * Event Trigger kann als direkter Zugriff durch den Server ersetzt werden
                         *     - weniger Abspaltung (Event Trigger/ Async Handler ==> Komplette Abspaltung)
                         *     - bzw. maybe unschön
                         */
                        EventInt.triggerCopyFileEvent(clients);
                        break;
                    case "reply":
                        EventInt.triggerClientReplyEvent(); break;
                    case "q": return;
                }
            }
        }
    }
}
