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
        public event EventHandler<ClientDetails> NewClientEvent;

        public void triggerCopyFileEvent(MessageDetails details)
        {
            CopyFileEvent?.Invoke(this, details);
        }

        public void triggerClientReplyEvent()
        {
            ClientReplyEvent?.Invoke(this, new MessageDetails(new List<Client>(), new Progress<ProgressDetails>()));
        }

        public void newClientEvent(ClientDetails details)
        {
            NewClientEvent?.Invoke(this, details);
        }
    }
}