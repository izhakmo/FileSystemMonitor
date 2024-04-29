using log4net.Config;
using log4net;
using System.Text;
using Newtonsoft.Json;

namespace FileMonitor
{
    public class FileSystemMonitor
    {
        private readonly ILog log = LogManager.GetLogger(typeof(FileSystemMonitor));
        private FileSystemWatcher _watcher;

        private readonly string _directoryPath;
        // TODO configure the URL
        private readonly string _postEventUrl = "https://localhost:7174/FileSystemEventsHandler/PostEventLog";


        public FileSystemMonitor(string directoryPath)
        {
            BasicConfigurator.Configure();

            // TODO add fileName as input
            //_generalLogFileName = Consts.DefualtGeneralLogFileName;

            _directoryPath = directoryPath;
            _watcher = new FileSystemWatcher(_directoryPath);
            StartMonitoring();
        }

        private void StartMonitoring()
        {
            // Configure log4net
            //XmlConfigurator.Configure();
            log.Info($"[{nameof(StartMonitoring)}] start monitoring directory: {_directoryPath}");

            _watcher = new FileSystemWatcher(_directoryPath);

            // Set the event handlers
            _watcher.Created += (sender, e) => { _ = OnFileChangeAsync(sender, e); };
            _watcher.Deleted += (sender, e) => { _ = OnFileChangeAsync(sender, e); };
            _watcher.Changed += (sender, e) => { _ = OnFileChangeAsync(sender, e); };

            _watcher.Renamed += (sender, e) => { _ = OnFileRenameAsync(sender, e); };

            _watcher.EnableRaisingEvents = true;
        }

        public void StopMonitoring()
        {
            log.Info($"[{nameof(StopMonitoring)}] stop monitoring directory: {_directoryPath}");

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
            log.Info($"[{nameof(PostFileEventLog)}] eventLog: {eventLog}.");

            await SendPostRequest(eventLog);
        }

        private async Task SendPostRequest(EventLog eventLog)
        {
            string eventLogAsjson = JsonConvert.SerializeObject(eventLog);
            
            using (var client = new HttpClient())
            {
                HttpContent content = new StringContent(eventLogAsjson, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(_postEventUrl, content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read response content as string
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response: " + responseBody);
                    }
                    else
                    {
                        // Print error status code
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
                catch (HttpRequestException e)
                {
                    // Print any exception that occurred
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }
    }
}
