using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace NetWeaverServer.Jobs
{
    public class LoggingOperation
    {
        public const string LOG = "NetWeaver";
        public string Source { get; }
        private string layout = "{0:dd-MM-yy HH:mm:ss} {1,-11} {2:S}";

        public LoggingOperation(string source)
        {
            Source = source;
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, LOG);
                Console.WriteLine("Created new EventView Source");
            }
        }

        public async void Error(object caller, string message)
        {
            await WriteLog(caller, EventLogEntryType.Error, message);
        }

        public async void Warning(object caller, string message)
        {
            await WriteLog(caller, EventLogEntryType.Warning, message);
        }

        public async void Info(object caller, string message)
        {
            await WriteLog(caller, EventLogEntryType.Information, message);
        }

        public async void Debug(string message)
        {
            await Task.Run(() => Console.WriteLine(string.Format(layout, DateTime.Now, "[DEBUG]", message)));
        }

        private async Task WriteLog(object caller, EventLogEntryType type, string message)
        {
            using (EventLog eventLog = new EventLog(LOG))
            {
                eventLog.Source = Source;
                await Task.Run(() => eventLog.WriteEntry(message, type));
            }
        }

    }
}