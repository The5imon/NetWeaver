using System;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;

namespace NetWeaverServer.Tasks.Operations
{
    public class ClientOperation
    {
        private GUIServerInterface Channel { get; }
        public ClientOperation(GUIServerInterface communication)
        {
            Channel = communication;
            Channel.NewClientEvent += OnClientConnect;
        }

        private void OnClientConnect(object sender, ClientDetails e)
        {
            Console.WriteLine("New Client - " + e.Client);
        }
    }
}