using log4net.Config;
using log4net;
using log4net.Util;

namespace FileMonitor
{
    public class FileSystemMonitor
    {
        private readonly ILog log = LogManager.GetLogger(typeof(FileSystemMonitor));
        private readonly string directoryPath;

        public FileSystemMonitor(string directoryPath)
        {
            BasicConfigurator.Configure();
            this.directoryPath = directoryPath;
        }

        public void StartMonitoring()
        {
            // Configure log4net
            //XmlConfigurator.Configure();

            // Create a new FileSystemWatcher
            FileSystemWatcher watcher = new FileSystemWatcher(directoryPath);
            
            // TODO do i want recursive monitoring?
            //watcher.IncludeSubdirectories = true;

            // Set the event handlers
            watcher.Created += OnFileChange;
            watcher.Deleted += OnFileChange;
            watcher.Changed += OnFileChange;

            watcher.Renamed += OnFileRename;
            watcher.Error += OnFileError;


            // Enable the watcher
            // TODO what is this?
            watcher.EnableRaisingEvents = true;

            log.Info($"Monitoring directory: {directoryPath}");
        }

        private void OnFileChange(object sender, FileSystemEventArgs e)
        {
            string logMsg = $"File {e.ChangeType}: {e.FullPath}";

            LogChangesToFile(logMsg);
        }

        private void OnFileError(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnFileRename(object sender, RenamedEventArgs e)
        {
            log.Info($"File changed: {e.FullPath} , {e.ChangeType}");
            LogChangesToFile($"File changed: {e.FullPath}");
        }

        private void LogChangesToFile(string logMsg)
        {
            log.Info(logMsg);

            string logFilePath = "changes.log";

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now} - {logMsg}");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
