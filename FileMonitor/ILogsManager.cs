namespace FileMonitor
{
    public interface ILogsManager
    {
        void Write(string directoryPath, EventLogMsg eventLogMsg);
        void RemoveMonitor(string directoryPath);
    }
}
