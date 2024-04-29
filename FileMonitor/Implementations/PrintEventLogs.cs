using FileMonitor.Interfaces;

namespace FileMonitor.Implementations
{
    public class PrintEventLogs : IPrintEventLogs
    {
        private ILogsManager _logsManager;

        public PrintEventLogs(ILogsManager logsManager)
        {
            _logsManager = logsManager;
        }

        public IEnumerable<EventLog> PrintLastEvents(int NumberOfLastEventsToPrint)
        {
            return _logsManager.PrintLastEvents(NumberOfLastEventsToPrint);
        }

        public IEnumerable<EventLog> PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventLog> PrintFolderLastEventsOfType(string folderPath, string eventType, int NumberOfLastEventsToPrint)
        {
            throw new NotImplementedException();
        }
    }
}
