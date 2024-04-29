namespace FileMonitor
{
    public interface ILogsManager
    {
        void Write(string directoryPath, EventLog eventLogMsg);
        void RemoveMonitor(string directoryPath);
    }
}
