using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace NetWeaverServer.Jobs
{
    public class LoggingOperation
    {
        public const string LOG = "NetWeaver";
        private string layout = "{0:dd-MM-yy HH:mm:ss} {1,-11} {2:S}";

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
            string fulname = caller.GetType().FullName;
            string source = fulname.Substring(fulname.LastIndexOf('.'), fulname.Length);
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, LOG);
                Console.WriteLine("Created new EventView Source");
            }
            using (EventLog eventLog = new EventLog(LOG))
            {
                eventLog.Source = source;
                await Task.Run(() => eventLog.WriteEntry(message, type));
            }
        }

    }
}