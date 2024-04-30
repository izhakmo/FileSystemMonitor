using FileMonitor.Interfaces;

namespace FileMonitor.Implementations
{
    public class PrintEventLogs : IPrintEventLogs
    {
        private ILogsCacheManager _logsManager;

        public PrintEventLogs(ILogsCacheManager logsManager)
        {
            _logsManager = logsManager;
        }

        public IEnumerable<EventLog> PrintLastEvents(int NumberOfLastEventsToPrint)
        {
            return _logsManager.PrintLastEvents(NumberOfLastEventsToPrint);
        }

        public IEnumerable<EventLog> PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint)
        {
            return _logsManager.PrintFolderLastEvents(folderPath, NumberOfLastEventsToPrint);
        }

        public IEnumerable<EventLog> PrintFolderLastEventsOfType(string folderPath, string eventTypeToLower, int NumberOfLastEventsToPrint)
        {
            return _logsManager.PrintFolderLastEventsOfType(folderPath, eventTypeToLower, NumberOfLastEventsToPrint);
        }
    }
}
