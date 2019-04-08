using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet;
using NetWeaverServer.Datastructure;

namespace NetWeaverServer.GraphicalUI
{
    public class GUIServerInterface
    {
        /**
         *guiServerInterface.getClientReplyEvent().Invoke(guiServerInterface,
         * new MessageDetails(new List<Client>(), new Progress<ProgressDetails>()));
         */
        
        //TODO: Create Interaction Concept; GUI -> ServerInterface, Server; Server -> GUIinterface, GUI
        //GUI Events
        public event EventHandler<MessageDetails> CopyFileEvent;
        public event EventHandler<MqttApplicationMessageReceivedEventArgs> ClientReplyEvent;
        public event EventHandler<ClientDetails> NewClientEvent;
        
        //Server Events
        public event EventHandler<MessageDetails> UpdatedContentEvent;

        public void triggerCopyFileEvent(MessageDetails details)
        {
            CopyFileEvent?.Invoke(this, details);
        }

        public void triggerClientReplyEvent()
        {
            MqttApplicationMessage am = new MqttApplicationMessageBuilder()
                .WithTopic("/reply/SimonsPC").WithPayload("ACK").Build();
            MqttApplicationMessageReceivedEventArgs md = new MqttApplicationMessageReceivedEventArgs("SimonsPC", am);
            ClientReplyEvent?.Invoke(this, md);
        }

        public void newClientEvent(ClientDetails details)
        {
            NewClientEvent?.Invoke(this, details);
        }

        public void triggerUpdatedContentEvent()
        {
            UpdatedContentEvent?.Invoke(this ,new MessageDetails(new List<Client>(), new Progress<ProgressDetails>()));
        }
        
        /**
         * This changes Everything
         */
        public EventHandler<MqttApplicationMessageReceivedEventArgs> getClientReplyEvent()
        {
            return ClientReplyEvent;
        }
    }
}