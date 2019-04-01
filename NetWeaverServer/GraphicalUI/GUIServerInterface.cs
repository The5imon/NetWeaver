using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;

namespace NetWeaverServer.GraphicalUI
{
    public class GUIServerInterface
    {
        //TODO: Create Interaction Concept; GUI -> ServerInterface, Server; Server -> GUIinterface, GUI
        public event EventHandler<MessageDetails> CopyFileEvent;

        public event EventHandler<MessageDetails> ClientReplyEvent;
        
        public void print()
        {
            Console.WriteLine("asd");
        }
        public void triggerCopyFileEvent(List<string> clients)
        {
            Progress<ProgressDetails> progress = new Progress<ProgressDetails>();
            progress.ProgressChanged += ReportProgress;
            MessageDetails md = new MessageDetails(clients, progress);
            CopyFileEvent?.Invoke(this, md);
            /*
            Parallel.ForEach(md.clients, (mdClient) =>
                Console.Write(mdClient));
                */
        }

        public void triggerClientReplyEvent()
        {
            ClientReplyEvent?.Invoke(this, new MessageDetails(new List<string>(), new Progress<ProgressDetails>()));
        }

        private void ReportProgress(object sender, ProgressDetails e)
        {
            Console.WriteLine("Reporting List of finished clients");
            foreach (string client in e.Clients)
            {
                Console.WriteLine($"\r\t PC - {client} is finished");
            }
        }
    }
}