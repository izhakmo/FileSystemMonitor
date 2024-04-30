using FileMonitor.Interfaces;
using log4net;
using log4net.Config;
using System.Net;

namespace FileMonitor.Implementations
{
    public class FileSystemWatcherMonitor : IFileSystemWatcherMonitor
    {
        private readonly ILog _log;

        private int _numberOfFolderCurrentlyMonitored = 0;
        private int _maxNumberOfFoldersToMonitor;
        private Dictionary<string, FileSystemMonitor> _pathToMonitor;

        // TODO create FileSystemMonitorFactory and inject here
        public FileSystemWatcherMonitor(int maxNumberOfFoldersToMonitor, ILog log)
        {
            _log = log;
            if (maxNumberOfFoldersToMonitor < 1)
            {
                throw new ArgumentException($"{nameof(maxNumberOfFoldersToMonitor)} should be greather than 0.");
            }

            _maxNumberOfFoldersToMonitor = maxNumberOfFoldersToMonitor;
            _pathToMonitor = new Dictionary<string, FileSystemMonitor>();

            BasicConfigurator.Configure();
        }

        public HttpResponseMessage AddFolder(string folderPath)
        {
            _log.Info($"[{nameof(AddFolder)}] folderPath: {folderPath}.");
            string folderPathLowerCase = folderPath.ToLower();
            if (_pathToMonitor.ContainsKey(folderPathLowerCase))
            {
                string folderAlreadyMonitoredMsg = $"folderPath: `{folderPathLowerCase}` is already monitored.";
                _log.Warn($"[{nameof(AddFolder)}] {folderAlreadyMonitoredMsg}.");
                return new HttpResponseMessage(HttpStatusCode.Conflict) { Content = new StringContent(folderAlreadyMonitoredMsg) };
            }

            if (_numberOfFolderCurrentlyMonitored >= _maxNumberOfFoldersToMonitor)
            {
                string maxNumberOfFoldersMonitoredMsg = ReturnMaxNumberOfFoldersMonitoredResponse(folderPathLowerCase);
                _log.Warn($"[{nameof(AddFolder)}] {maxNumberOfFoldersMonitoredMsg}.");
                return new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = new StringContent(maxNumberOfFoldersMonitoredMsg) };
            }

            try
            {
                MonitorNewFolder(folderPathLowerCase);
                _log.Info($"[{nameof(AddFolder)}] folderPath: `{folderPathLowerCase}` added to Dictionary successfully.");
            }
            catch (ArgumentException ex)
            {
                var folderDoesNotExistMsg = $"failed to monitor folderPath: `{folderPathLowerCase}`. " +
                $"folder probably does not exist. error: {ex.Message}.";
                _log.Warn($"[{nameof(AddFolder)}] {folderDoesNotExistMsg}. {GetMonitoredFolders()}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(folderDoesNotExistMsg) };
            }
            catch (Exception ex)
            {
                var errorMsg = $"failed to monitor folderPath: `{folderPathLowerCase}`. error: {ex.Message}.";
                _log.Error($"[{nameof(AddFolder)}] {errorMsg}.");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(errorMsg) };
            }

            string successfulMsg = $"start monitoring folderPath: `{folderPathLowerCase}`.";
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(successfulMsg) };
        }

        public HttpResponseMessage RemoveFolder(string folderPath)
        {
            _log.Info($"[{nameof(RemoveFolder)}] folderPath: {folderPath}.");
            string folderPathLowerCase = folderPath.ToLower();
            if (!_pathToMonitor.ContainsKey(folderPathLowerCase))
            {
                string folderIsNotMonitoredMsg = $"folderPath: `{folderPathLowerCase}` is not monitored. {GetMonitoredFolders()}";
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(folderIsNotMonitoredMsg) };
            }
            try
            {
                UnmonitorFolder(folderPathLowerCase);
                _log.Info($"[{nameof(RemoveFolder)}] folderPath: `{folderPathLowerCase}` removed from Dictionary successfully.");
            }
            catch (Exception ex)
            {
                var errorMsg = $"failed to unmonitor folderPath: `{folderPathLowerCase}`. error: {ex.Message}.";
                _log.Error($"[{nameof(RemoveFolder)}] {errorMsg}.");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(errorMsg) };
            }

            string successfulMsg = $"unmonitored folderPath: `{folderPathLowerCase}`.";
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(successfulMsg) };
        }

        private void MonitorNewFolder(string folderPathLowerCase)
        {
            FileSystemMonitor fileSystemMonitor = new FileSystemMonitor(folderPathLowerCase,_log);
            _numberOfFolderCurrentlyMonitored++;

            _pathToMonitor.Add(folderPathLowerCase, fileSystemMonitor);
        }

        private void UnmonitorFolder(string folderPathLowerCase)
        {
            bool isFolderMonitored = _pathToMonitor.TryGetValue(folderPathLowerCase, out FileSystemMonitor? fileSystemMonitor);
            if (isFolderMonitored)
            {
                fileSystemMonitor.StopMonitoring();
                _pathToMonitor.Remove(folderPathLowerCase);
                _numberOfFolderCurrentlyMonitored--;
            }

        }

        private string ReturnMaxNumberOfFoldersMonitoredResponse(string folderPathLowerCase)
        {
            var folderpaths = GetMonitoredFolders();

            return $"failed to monitor folderPath: {folderPathLowerCase}.{Environment.NewLine}" +
                $"already monitoring max number of folders allowed " +
                $"({_maxNumberOfFoldersToMonitor} folders).{Environment.NewLine}" +
                $"{folderpaths}.{Environment.NewLine}" +
                $"Please unmonitor one of the folders in order to monitor a new folder.";
        }

        private string GetMonitoredFolders()
        {
            return "MonitoredPaths: " + string.Join(",", _pathToMonitor.Keys) + ".";
        }
    }
}
