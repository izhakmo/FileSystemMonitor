using log4net.Config;
using log4net;

namespace FileMonitor
{
    public class FileSystemMonitor
    {
        private readonly ILog log = LogManager.GetLogger(typeof(FileSystemMonitor));
        private FileSystemWatcher _watcher;

        //private ILogsManager _logsManager;
        private readonly string _directoryPath;


        public FileSystemMonitor(string directoryPath
            //, ILogsManager logsManager
            )
        {
            BasicConfigurator.Configure();

            // TODO add fileName as input
            //_generalLogFileName = Consts.DefualtGeneralLogFileName;

            _directoryPath = directoryPath;
            _watcher = new FileSystemWatcher(_directoryPath);
            //_logsManager = logsManager;

            StartMonitoring();
        }

        private void StartMonitoring()
        {
            // Configure log4net
            //XmlConfigurator.Configure();
            log.Info($"[{nameof(StartMonitoring)}] start monitoring directory: {_directoryPath}");

            _watcher = new FileSystemWatcher(_directoryPath);

            // Set the event handlers
            _watcher.Created += OnFileChange;
            _watcher.Deleted += OnFileChange;
            _watcher.Changed += OnFileChange;

            _watcher.Renamed += OnFileRename;

            _watcher.EnableRaisingEvents = true;
        }

        public void StopMonitoring()
        {
            log.Info($"[{nameof(StopMonitoring)}] stop monitoring directory: {_directoryPath}");

            _watcher.EnableRaisingEvents = false;

            _watcher.Created -= OnFileChange;
            _watcher.Deleted -= OnFileChange;
            _watcher.Changed -= OnFileChange;

            _watcher.Renamed -= OnFileRename;

            // TODO post remove monitor 
            //_logsManager.RemoveMonitor(_directoryPath);

            // TODO do i need to dispose????
            _watcher.Dispose();
        }

        private void OnFileChange(object sender, FileSystemEventArgs e)
        {
            PostFileEventLog(e.ChangeType, e.FullPath, string.Empty);
        }

        private void OnFileRename(object sender, RenamedEventArgs e)
        {
            string logMsg = $"old fileName: {e.OldName}, oldFullPath: {e.OldFullPath}.";
            PostFileEventLog(e.ChangeType, e.FullPath, logMsg);
        }

        private void PostFileEventLog(WatcherChangeTypes ChangeType, string filePath, string logMsg)
        {

            EventLogMsg eventLogMsg = new EventLogMsg(ChangeType, filePath, logMsg);
            log.Info($"[{nameof(PostFileEventLog)}] eventLogMsg: {eventLogMsg}.");

            //_logsManager.Write(_directoryPath, eventLogMsg);
            // TODO postEvent
        }
    }
}
