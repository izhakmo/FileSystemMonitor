namespace FileMonitor
{
    public interface ILogsManager
    {
        void AddEventLogToCache(EventLog eventLog);
        IEnumerable<EventLog> PrintLastEvents(int NumberOfLastEventsToPrint);

        // TODO implement the 2 other functions
    }
}
