using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Tasks.Jobs;

namespace NetWeaverServer.GraphicalUI
{
    public class EventInterface
    {
        /**
         *guiServerInterface.getClientReplyEvent().Invoke(guiServerInterface,
         * new MessageDetails(new List<Client>(), new Progress<ProgressDetails>()));
         */
        //GUI Events
        public event EventHandler<MessageDetails> ExecuteScriptEvent;
        
        //Server Events
        public event EventHandler<MessageDetails> UpdatedContentEvent;

        public EventHandler<MessageDetails> getExecuteScriptEvent() {
            return ExecuteScriptEvent;
        }

        public EventHandler<MessageDetails> getUpdatedContentEvent()
        {
            return UpdatedContentEvent;
        } 
    }
}