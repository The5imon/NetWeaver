using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;

using static NetWeaverServer.Tasks.Operations.LoggingOperation;

namespace NetWeaverServer.Tasks.Jobs
{
    public class CopyFileJob : Job
    {
        public CopyFileJob(Client client, GUIServerInterface comminication, IProgress<ProgressDetails> progress)
            : base(client, comminication, progress) { }

        public override void Work()
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

        protected override void AwaitReply(object sender, MessageDetails args)
        {
            Reply.Set();
        }
    }
}