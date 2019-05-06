using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
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
        public event EventHandler<TaskDetails> ExecuteScriptEvent;
        public event EventHandler<TaskDetails> DeploymentEvent;
        public event EventHandler<TaskDetails> CopyEvent;

        //Server Events
        public event EventHandler UpdatedContentEvent;

        public EventHandler<TaskDetails> GetExecuteScriptEvent() {
            return ExecuteScriptEvent;
        }

        public EventHandler<TaskDetails> GetCopyEvent()
        {
            return CopyEvent;
        }
        public EventHandler<TaskDetails> GetDeploymentEvent()
        {
            return DeploymentEvent;
        }

        public EventHandler GetUpdatedContentEvent()
        {
            return UpdatedContentEvent;
        }
    }
}