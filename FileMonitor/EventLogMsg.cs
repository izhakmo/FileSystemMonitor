namespace FileMonitor
{
    public class EventLogMsg
    {
        public DateTime Time { get; set; }
        public string ChangeType { get; set; }
        public string FolderPath { get; set; }
        public string FileName { get; set; }

        public string LogMsg { get; set; }

        public EventLogMsg() { } // Parameterless constructor for serialization / deserialization 

        public EventLogMsg(WatcherChangeTypes ChangeType, string filePath, string logMsg)
        {
            Time = DateTime.Now;
            this.ChangeType = ChangeType.GetType().Name;
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
