using System;
using System.Threading;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;

namespace NetWeaverServer.Tasks.Commands
{
    //TODO: Maybe implement in the Future
    public class Command
    {
        private GUIServerInterface CommunicationInterface { get; }

        private AutoResetEvent Reply = new AutoResetEvent(false);

        public Command(EventHandler<ProgressDetails> message)
        {

        }
    }
}