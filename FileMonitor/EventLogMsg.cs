namespace FileMonitor
{
    public class EventLogMsg
    {
        private DateTime _time;
        private string _changeTypeAsString;
        private string _folderPath;
        private string _fileName;

        private string _logMsg;

        public EventLogMsg(WatcherChangeTypes ChangeType, string filePath, string logMsg)
        {
            _time = DateTime.Now;
            _changeTypeAsString = ChangeType.GetType().Name;
            _folderPath = Path.GetDirectoryName(filePath);
            _fileName = Path.GetFileName(filePath);
            _logMsg = logMsg;
        }

        public override string ToString()
        {
            return $"[{_time}] eventType: {_changeTypeAsString}, folderPath: {_folderPath}, fileName: {_fileName}. {_logMsg}";
        }

        public string GetChangeType() => _changeTypeAsString;
    }
}
