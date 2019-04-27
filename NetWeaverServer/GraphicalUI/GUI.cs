using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using static NetWeaverServer.Tasks.Operations.LoggingOperation;

namespace NetWeaverServer.GraphicalUI
{
    public class GUI
    {
        public List<Client> clients = new List<Client>();
        private EventInterface EventInt { get; }

        public GUI(EventInterface eventInt)
        {
            EventInt = eventInt;
            new Thread(Run).Start();
            clients.Add(new Client("abcd", "SimonPC", "127.0.0.1"));
        }

        public void Run()
        {
            string input = "";

            while(true)
            {
                input = Console.ReadLine();
                string[] args = input.Split(':');
                Progress<TaskProgress> progress = new Progress<TaskProgress>();
                progress.ProgressChanged += ReportProgress;
                TaskDetails taskDetails = new TaskDetails(clients, progress);
                switch(args[0])
                {
                    case "copy":
                        EventInt.getExecuteScriptEvent().Invoke(this, taskDetails);
                        break;
                    case "list":
                        PrintClients();
                        break;
                    case "q":
                        //Logger.Delete();
                        return;
                }
            }
        }

        private void PrintClients()
        {
            foreach (Client client in clients)
            {
                Console.WriteLine(client);
            }
        }

        private void ReportProgress(object sender, TaskProgress e)
        {
            Console.WriteLine("Reported Progress: ");
            foreach (JobProgress progress in e.JobProgress)
            {
                Console.WriteLine(progress);
            }
        }
    }
}
