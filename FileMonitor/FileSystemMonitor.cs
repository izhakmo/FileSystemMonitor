using log4net.Config;
using log4net;

namespace FileMonitor
{
    public class FileSystemMonitor
    {
        private readonly ILog log = LogManager.GetLogger(typeof(FileSystemMonitor));
        private FileSystemWatcher _watcher;

        private readonly string _directoryPath;
        private readonly string _generalLogFileName;

        public FileSystemMonitor(string directoryPath)
        {
            BasicConfigurator.Configure();

            // TODO add fileName as input
            _generalLogFileName = Consts.DefualtGeneralLogFileName;
            _directoryPath = directoryPath;
            _watcher = new FileSystemWatcher(_directoryPath);
        }

        public void StartMonitoring()
        {
            // Configure log4net
            //XmlConfigurator.Configure();

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

            log.Info($"start monitoring directory: {_directoryPath}");
        }

        public void StopMonitoring()
        {
            _watcher.Created -= OnFileChange;
            _watcher.Deleted -= OnFileChange;
            _watcher.Changed -= OnFileChange;

            _watcher.Renamed -= OnFileRename;

            _watcher.EnableRaisingEvents = false;

            log.Info($"stop monitoring directory: {_directoryPath}");
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
            //var time = DateTime.Now;
            //var changeTypeAsString = ChangeType.GetType().Name;
            //string folderPath = Path.GetDirectoryName(filePath);
            //string fileName = Path.GetFileName(filePath);

            //var updatedLogMsg = $"[{time}] eventType: {changeTypeAsString}, folderPath: {folderPath}, fileName: {fileName}. {logMsg}";

            EventLogMsg eventLogMsg = new EventLogMsg(ChangeType,filePath,logMsg);
            log.Info($"{eventLogMsg}");

            try
            {
                using (StreamWriter writer = new StreamWriter(_generalLogFileName, true))
                {
                    writer.WriteLine($"{eventLogMsg}");
                }
            }
            catch (Exception ex)
            {
                log.Error($"failed to write log to file: {_generalLogFileName}. " +
                    $"expected log to write: {eventLogMsg}. error: {ex.Message}.");
            }
        }
    }
}
