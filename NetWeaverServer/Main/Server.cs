using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Tasks.Jobs;
using static NetWeaverServer.Main.Program;

namespace NetWeaverServer.Main
{
    public class Server
    {
        private GUIServerInterface EventInt;

        public Server(GUIServerInterface eventInt)
        {
            EventInt = eventInt;
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

        private async void Gui_CopyFileEvent(object sender, MessageDetails md)
        //EventInterface zwischen GUI und Server
        //    - mit async kann der Invoker (Caller) weitermachen, was kein Problem weil, weil ich zurrückreporten kann
        //    - ohne async wartet der Invoker (Caller) bis das event fertig ist, gibt dabei aber nicht controll zurrück an die CMD
        //Wäre das GUI direkt durchgepätscht könnte man den copy file Task await -en
        {
            await StartJob(typeof(CopyFileJob), md, (GUIServerInterface) sender);
        }

        private async Task StartJob(Type job, MessageDetails md, GUIServerInterface inter)
        {
            JobManager manager = new JobManager(job, md, EventInt);
            await manager.RunOnAllClients();
        }
    }
}
