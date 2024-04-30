using log4net.Config;
using log4net;
using System.Text;
using Newtonsoft.Json;

namespace FileMonitor.Implementations
{
    public class FileSystemMonitor
    {
        //private readonly ILog log = LogManager.GetLogger(typeof(FileSystemMonitor));
        private readonly ILog _log;
        private FileSystemWatcher _watcher;

        private readonly string _directoryPath;

        // TODO inject the URL, inject log filePath
        private readonly string _postEventUrl = "https://localhost:7174/FileSystemEventsHandler/PostEventLog";


        public FileSystemMonitor(string directoryPath, ILog log)
        {
            _log = log;

            // Configure log4net
            //XmlConfigurator.Configure();

            // TODO add fileName as input
            //_generalLogFileName = Consts.DefualtGeneralLogFileName;

            _directoryPath = directoryPath;
            _watcher = new FileSystemWatcher(_directoryPath);
            StartMonitoring();
        }

        private void StartMonitoring()
        {
            _log.Info($"[{nameof(StartMonitoring)}] start monitoring directory: {_directoryPath}");

            _watcher.Created += (sender, e) => { _ = OnFileChangeAsync(sender, e); };
            _watcher.Deleted += (sender, e) => { _ = OnFileChangeAsync(sender, e); };
            _watcher.Changed += (sender, e) => { _ = OnFileChangeAsync(sender, e); };

            _watcher.Renamed += (sender, e) => { _ = OnFileRenameAsync(sender, e); };

            _watcher.EnableRaisingEvents = true;
        }

        public void StopMonitoring()
        {
            _log.Info($"[{nameof(StopMonitoring)}] stop monitoring directory: {_directoryPath}");

            _watcher.EnableRaisingEvents = false;

            _watcher.Created -= (sender, e) => { _ = OnFileChangeAsync(sender, e); };
            _watcher.Deleted -= (sender, e) => { _ = OnFileChangeAsync(sender, e); };
            _watcher.Changed -= (sender, e) => { _ = OnFileChangeAsync(sender, e); };

            _watcher.Renamed -= (sender, e) => { _ = OnFileRenameAsync(sender, e); };

            // TODO do i need to dispose????
            _watcher.Dispose();
        }

        private async Task OnFileChangeAsync(object sender, FileSystemEventArgs e)
        {
            await PostFileEventLog(e.ChangeType, e.FullPath, string.Empty);
        }

        private async Task OnFileRenameAsync(object sender, RenamedEventArgs e)
        {
            string logMsg = $"old fileName: {e.OldName}, oldFullPath: {e.OldFullPath}.";
            await PostFileEventLog(e.ChangeType, e.FullPath, logMsg);
        }

        private async Task PostFileEventLog(WatcherChangeTypes ChangeType, string filePath, string logMsg)
        {

            EventLog eventLog = new EventLog(ChangeType, filePath, logMsg);
            _log.Info($"[{nameof(PostFileEventLog)}] eventLog: {eventLog}.");

            await SendPostRequest(eventLog);
        }

        private async Task SendPostRequest(EventLog eventLog)
        {
            string eventLogAsjson = JsonConvert.SerializeObject(eventLog);

            using (var client = new HttpClient())
            {
                HttpContent messagePostContent = new StringContent(eventLogAsjson, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage responseMsg = await client.PostAsync(_postEventUrl, messagePostContent);
                    if (responseMsg.IsSuccessStatusCode)
                    {
                        _log.Info($"[{nameof(SendPostRequest)}] eventLog published successfully. " +
                            $"eventLog: {eventLog}. statusCode: {responseMsg.StatusCode}.");
                    }
                    else
                    {
                        _log.Error($"[{nameof(SendPostRequest)}] failed to published eventLog. " +
                            $"eventLog: {eventLog}. statusCode: {responseMsg.StatusCode}.");
                    }
                }
                catch (Exception ex)
                {
                    _log.Error($"[{nameof(SendPostRequest)}] failed to published eventLog. error: {ex}.");
                }
            }
        }
    }
}
