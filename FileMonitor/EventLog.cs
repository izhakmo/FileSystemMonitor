namespace FileMonitor
{
    public class EventLog
    {
        public DateTime Time { get; set; }
        public string ChangeType { get; set; }
        public string FolderPath { get; set; }
        public string FileName { get; set; }

        public string LogMsg { get; set; }

        public EventLog() { } // constructor for serialization / deserialization 

        public EventLog(WatcherChangeTypes changeType, string filePath, string logMsg)
        {
            Time = DateTime.Now;
            ChangeType = changeType.ToString();
            FolderPath = Path.GetDirectoryName(filePath);
            FileName = Path.GetFileName(filePath);
            LogMsg = logMsg;
        }

        public override string ToString()
        {
            return $"[{Time}] eventType: {ChangeType}, folderPath: {FolderPath}, fileName: {FileName}. {LogMsg}";
        }

        public string GetChangeType() => ChangeType;
    }
}
