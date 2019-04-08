using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.MQTT;
using NetWeaverServer.Tasks.Jobs;
using static NetWeaverServer.Main.Program;

namespace NetWeaverServer.Main
{
    public class Server
    {
        private GUIServerInterface EventInt { get; }
        private MqttMaster Channel { get; }

        public Server(GUIServerInterface eventInt, MqttMaster channel)
        {
            EventInt = eventInt;
            Channel = channel;
            new Thread(Run).Start();
        }

        public void Run()
        {
            WireUpGUIHandlers();
        }

        private void WireUpGUIHandlers()
        {
            EventInt.CopyFileEvent += Gui_CopyFileEvent;
        }

        //TODO: Rework Event and Handler design
        private async void Gui_CopyFileEvent(object sender, MessageDetails md)
        //EventInterface zwischen GUI und Server
        //    - mit async kann der Invoker (Caller) weitermachen, was kein Problem weil, weil ich zurrückreporten kann
        //    - ohne async wartet der Invoker (Caller) bis das event fertig ist, gibt dabei aber nicht controll zurrück an die CMD
        //Wäre das GUI direkt durchgepätscht könnte man den copy file Task await -en
        {
            await StartJob(typeof(CopyFileJob), md);
        }
        
         //TODO: Nicht konform
        private async Task StartJob(Type job, MessageDetails md)
        {
            JobManager manager = new JobManager(job, md, Channel);
            await manager.RunOnAllClients();
        }
    }
}
