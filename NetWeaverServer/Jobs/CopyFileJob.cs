using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;

using static NetWeaverServer.Main.Program;

namespace NetWeaverServer.Jobs
{
    public class CopyFileJob : Job
    {

        public CopyFileJob(MessageDetails messageDetails, GUIServerInterface server)
            : base(messageDetails, server) { }

        public override async Task Work()
        {
            List<Task> jobs = new List<Task>();

            foreach (Client client in Clients)
            {
                //ACK Display
                CopyFile cf = new CopyFile(client, Server);
                jobs.Add(Task.Run((() => cf.execute())));
                //Async GUI Experience Display
                /*
                Random r = new Random();
                jobs.Add(Task.Run(() => writeFile(@"data\", client, r.Next(1000) + 1000)));
                */
            }

            await Task.WhenAll(jobs);
        }

        /*
        private void writeFile(string path, string client, int count)
            //TODO: Extract this Task into a Job class with a AutoResetEvent
        {
            for (int i = 0; i < count; i++)
            {
                lock (path) //synchronized area where only 1 process/task can have access
                {
                    File.WriteAllText(path + client + ".txt", client);
                }
            }
            pd.clients.Add(client);
            progress.Report(pd);
        }*/
    }

    class CopyFile
    {
        private Client Client { get; }
        private GUIServerInterface CommunicationInterface { get; }
        private AutoResetEvent Reply { get; }

        public CopyFile(Client client, GUIServerInterface commint)
        {
            Client = client;
            CommunicationInterface = commint;
            CommunicationInterface.ClientReplyEvent += AwaitReply;
            Reply = new AutoResetEvent(false);
        }

        private void AwaitReply(object sender, MessageDetails e)
        {
            Reply.Set();
        }

        public void execute()
        {
            Console.WriteLine("Telling {0} to open NetShare", Client);
            Reply.WaitOne();
            Console.WriteLine("ACK: {0} opened the NetShare", Client);

            Console.WriteLine("Copying File to Netshare");
            Reply.WaitOne();
            Console.WriteLine("ACK: {0} received the File", Client);

            Console.WriteLine("Telling {0} to close the NetShare", Client);
            Reply.WaitOne();
            Console.WriteLine("ACK: {0} closed the NetShare", Client);

            Console.WriteLine("Telling {0} to Execute the File", Client);
            Reply.WaitOne();
            Console.WriteLine("ACK: {0} executed the File", Client);
        }

    }
}