using System;
using System.Collections.Generic;
using System.Threading;
using NetWeaverServer.Datastructure;
using NetWeaverServer.GraphicalUI;
using NetWeaverServer.Jobs;

namespace NetWeaverServer.Main
{
    public class Server
    {
        private GUIServerInterface EventInt { get; }

        public Server(GUIServerInterface eventInt)
        {
            EventInt = eventInt;
            new Thread(this.Run).Start();
            eventInt.print();
        }

        public void Run()
        {
            WireUpGUIHandlers();
        }

        private void WireUpGUIHandlers()
        {
            Console.WriteLine(EventInt.GetType());
            EventInt.CopyFileEvent += Gui_CopyFileEvent;
        }

        private async void Gui_CopyFileEvent(object sender, MessageDetails md)
        //EventInterface zwischen GUI und Server
        //    - mit async kann der Invoker (Caller) weitermachen, was kein Problem weil, weil ich zurrückreporten kann
        //    - ohne async wartet der Invoker (Caller) bis das event fertig ist, gibt dabei aber nicht controll zurrück an die CMD
        //Wäre das GUI direkt durchgepätscht könnte man den copy file Task await -en
        {
            CopyFileJob cfj = new CopyFileJob(md, (GUIServerInterface) sender);
            await cfj.Work();
        }
    }
}
