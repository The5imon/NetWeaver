using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWeaverServer.Datastructure
{
    public class MessageDetails : EventArgs
    {
        public IProgress<ProgressDetails> Progress { get; }

        public List<Client> Clients { get; }

        public MessageDetails(List<Client> clients, IProgress<ProgressDetails> progress)
        {
            Clients = clients;
            Progress = progress;
        }
    }

    public class ProgressDetails
    {
        public int Percentage { get; set; }

        public List<Client> Clients = new List<Client>();
    }
}
