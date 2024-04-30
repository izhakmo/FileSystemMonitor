namespace FileMonitor
{
    public interface ILogsCacheManager
    {
        void AddEventLogToCache(EventLog eventLog);
        IEnumerable<EventLog> PrintLastEvents(int NumberOfLastEventsToPrint);
        IEnumerable<EventLog> PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint);
        IEnumerable<EventLog> PrintFolderLastEventsOfType(string folderPath, string eventType, int NumberOfLastEventsToPrint);
    }
}
