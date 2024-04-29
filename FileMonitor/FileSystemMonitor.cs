using log4net.Config;
using log4net;

namespace FileMonitor
{
    public class FileSystemMonitor
    {
        private readonly ILog log = LogManager.GetLogger(typeof(FileSystemMonitor));
        private FileSystemWatcher _watcher;

        private ILogsManager _logsManager;
        //private Dictionary<string, Stack<EventLogMsg>> _logsByChangeTypes;

        private readonly string _directoryPath;
        //private readonly string _generalLogFileName;

        public FileSystemMonitor(string directoryPath, ILogsManager logsManager)
        {
            BasicConfigurator.Configure();

            // TODO add fileName as input
            //_generalLogFileName = Consts.DefualtGeneralLogFileName;
            _directoryPath = directoryPath;
            _watcher = new FileSystemWatcher(_directoryPath);
            _logsManager = logsManager;

            //_logsByChangeTypes = new Dictionary<string, Stack<EventLogMsg>>
            //{
            //    { Consts.AllLogsForFolderKey, new Stack<EventLogMsg>() }
            //};
        }

        public void StartMonitoring()
        {
            // Configure log4net
            //XmlConfigurator.Configure();
            log.Info($"start monitoring directory: {_directoryPath}");

            _watcher = new FileSystemWatcher(_directoryPath);
            // TODO do i want recursive monitoring?
            //watcher.IncludeSubdirectories = true;

            // Set the event handlers
            _watcher.Created += OnFileChange;
            _watcher.Deleted += OnFileChange;
            _watcher.Changed += OnFileChange;

            _watcher.Renamed += OnFileRename;


            // Enable the watcher
            // TODO what is this?
            _watcher.EnableRaisingEvents = true;
        }

        public void StopMonitoring()
        {
            log.Info($"stop monitoring directory: {_directoryPath}");

            _watcher.EnableRaisingEvents = false;

            _watcher.Created -= OnFileChange;
            _watcher.Deleted -= OnFileChange;
            _watcher.Changed -= OnFileChange;

            _watcher.Renamed -= OnFileRename;

            _logsManager.RemoveMonitor(_directoryPath);

            //foreach (var a in _logsByChangeTypes)
            //{
            //    // TODO do i need to clear the queues and dictionary?
            //    _logsByChangeTypes.Remove(a.Key);
            //    a.Value.Clear();
            //}

            // TODO do i need to dispose????
            _watcher.Dispose();
        }

        private void OnFileChange(object sender, FileSystemEventArgs e)
        {
            LogChangesToFile(e.ChangeType, e.FullPath, string.Empty);
        }

        private void OnFileRename(object sender, RenamedEventArgs e)
        {
            string logMsg = $"old fileName: {e.OldName}, oldFullPath: {e.OldFullPath}.";
            LogChangesToFile(e.ChangeType, e.FullPath, logMsg);
        }

        private void LogChangesToFile(WatcherChangeTypes ChangeType, string filePath, string logMsg)
        {

            EventLogMsg eventLogMsg = new EventLogMsg(ChangeType, filePath, logMsg);
            log.Info($"{eventLogMsg}");

            _logsManager.Write(_directoryPath, eventLogMsg);

            //_logsByChangeTypes.TryGetValue(Consts.AllLogsForFolderKey, out Stack<EventLogMsg> allLogsForFolder);
            //allLogsForFolder.Push(eventLogMsg);

            //if(_logsByChangeTypes.TryGetValue(ChangeType.ToString(), out Stack<EventLogMsg>? logsByTypeForFolder))
            //{
            //    // TODO add log here
            //    logsByTypeForFolder.Push(eventLogMsg);
            //}
            //else
            //{
            //    // TODO add log here
            //    logsByTypeForFolder = new Stack<EventLogMsg>();
            //    logsByTypeForFolder.Push(eventLogMsg);
            //    _logsByChangeTypes.Add(ChangeType.ToString(), logsByTypeForFolder);
            //}
        }
    }
}
