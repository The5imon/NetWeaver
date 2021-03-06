using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using MQTTnet;
using NetWeaverServer.Datastructure;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks.Operations
{
    public class LoggingOperation
    {
        //TODO: Definitely not done (30%), needs improvement
        private const string LOG = "NetWeaver";
        private static string layout = "{0:dd-MM-yy HH:mm:ss} {1,-11} {2:S}";
        private const string Topic = "/log/";

        private MqttMaster Channel { get; }

        public LoggingOperation(MqttMaster channel)
        {
            Channel = channel;
            channel.MessageReceivedEvent += OnClientLog;
        }

        private async void OnClientLog(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            if (e.ApplicationMessage.ConvertPayloadToString().StartsWith(Topic))
            {
                await WriteLog(this, EventLogEntryType.Error, "naga");
            }
        }

        public static async void Error(object caller, string message)
        {
            await WriteLog(caller, EventLogEntryType.Error, message);
        }

        public static async void Warning(object caller, string message)
        {
            await WriteLog(caller, EventLogEntryType.Warning, message);
        }

        public static async void Info(object caller, string message)
        {
            await WriteLog(caller, EventLogEntryType.Information, message);
        }

        public static async void Debug(string message)
        {
            await Task.Run(() => Console.WriteLine(string.Format(layout, DateTime.Now, "[DEBUG]", message)));
        }

        private static async Task WriteLog(object caller, EventLogEntryType type, string message)
        /**
         * Can Log Server System and Client Information (dependent on caller)
         */
        {
            string fullname = caller.GetType().FullName;
            string source = caller.GetType() == typeof(Client) ?
                ((Client) caller).HostName : fullname.Substring(fullname.LastIndexOf('.'), fullname.Length);

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, LOG);
            }
            using (EventLog eventLog = new EventLog(LOG))
            {
                eventLog.Source = source;
                await Task.Run(() => eventLog.WriteEntry(message, type));
            }
        }

        public void Delete()
        {
            EventLog.Delete(LOG);
        }
    }
}