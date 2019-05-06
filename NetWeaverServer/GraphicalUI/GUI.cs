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
        private DBInterface DbInterface { get; }

        public GUI(EventInterface eventInt, DBInterface dbInterface)
        {
            EventInt = eventInt;
            DbInterface = dbInterface;
            eventInt.UpdatedContentEvent += UpdateContent;
            new Thread(Run).Start();
            
            /*clients.Add(new Client("abcd", "SimonPC", "127.0.0.1"));
            clients.Add(new Client("abcd", "GregorPC", "127.0.0.1"));
            clients.Add(new Client("abcd", "WurzerPC", "127.0.0.1"));
            clients.Add(new Client("abcd", "MaxPC", "127.0.0.1"));
            clients.Add(new Client("abcd", "LukasPC", "127.0.0.1"));
            clients.Add(new Client("abcd", "BrunnerPC", "127.0.0.1"));
            clients.Add(new Client("abcd", "KalchiPC", "127.0.0.1"));
            clients.Add(new Client("abcd", "SpiderPC", "127.0.0.1"));
            clients.Add(new Client("abcd", "ManPC", "127.0.0.1"));
            clients.Add(new Client("abcd", "DasPC", "127.0.0.1"));*/

        }

        private void UpdateContent(object sender, EventArgs e)
        {
            clients = DbInterface.getClientList();
            PrintClients();
        }

        public void Run()
        {
            string input = "";

            while(true)
            {
                input = Console.ReadLine();
                string[] args = input.Split(' ');
                Progress<TaskProgress> progress = new Progress<TaskProgress>();
                progress.ProgressChanged += ReportProgress;
                switch(args[0])
                {
                    case "copy":
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Missing a file");
                            continue;
                        }
                        TaskDetails taskDetails = new TaskDetails(clients, progress, args[1]);
                        EventInt.GetCopyEvent().Invoke(this, taskDetails);
                        break;
                    case "exec":
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Missing a file");
                            continue;
                        }
                        TaskDetails ttdd = new TaskDetails(clients, progress, args[1]);
                        EventInt.GetCopyEvent().Invoke(this, ttdd);
                        break;
                    case "deploy":
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Missing a file");
                            continue;
                        }
                        TaskDetails td = new TaskDetails(clients, progress, args[1]);
                        EventInt.GetDeploymentEvent().Invoke(this, td);
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
