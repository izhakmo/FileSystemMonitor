using FileMonitor.Interfaces;

namespace FileMonitor.Implementations
{
    public class PrintEventLogs : IPrintEventLogs
    {
        public IEnumerable<string> PrintLastEvents(int NumberOfLastEventsToPrint)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> PrintFolderLastEventsOfType(string folderPath, string eventType, int NumberOfLastEventsToPrint)
        {
            throw new NotImplementedException();
        }
    }
}
