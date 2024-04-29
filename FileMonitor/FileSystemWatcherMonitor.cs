using System.Net;

namespace FileMonitor
{
    public class FileSystemWatcherMonitor : IFileSystemWatcherMonitor
    {
        private int _numberOfFolderCurrentlyMonitored = 0;
        private int _maxNumberOfFoldersToMonitor;
        private Dictionary<string, FileSystemMonitor> _pathToMonitor;

        public FileSystemWatcherMonitor(int maxNumberOfFoldersToMonitor)
        {
            if (maxNumberOfFoldersToMonitor < 1)
            {
                throw new ArgumentException($"{nameof(maxNumberOfFoldersToMonitor)} should be greather than 0.");
            }

            _maxNumberOfFoldersToMonitor = maxNumberOfFoldersToMonitor;
            _pathToMonitor = new Dictionary<string, FileSystemMonitor>();
        }

        public HttpResponseMessage AddFolder(string folderPath)
        {
            string folderPathLowerCase = folderPath.ToLower();
            if (_pathToMonitor.ContainsKey(folderPathLowerCase))
            {
                string folderAlreadyMonitoredMsg = $"folderPath: `{folderPathLowerCase}` is already monitored.";
                return new HttpResponseMessage(HttpStatusCode.Conflict) { Content = new StringContent(folderAlreadyMonitoredMsg) };
            }

            if (_numberOfFolderCurrentlyMonitored >= _maxNumberOfFoldersToMonitor)
            {
                return ReturnMaxNumberOfFoldersMonitoredResponse();
            }

            try
            {
                // TODO add logic for onboard a monitor
                MonitorNewFolder(folderPathLowerCase);
            }
            catch (ArgumentException ex)
            {
                var folderDoesNotExistMsg = $"failed to monitor folderPath: `{folderPathLowerCase}`. " +
                $"folder probably does not exist. error: {ex.Message}.";
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(folderDoesNotExistMsg) };
            }
            catch (Exception ex)
            {
                var errorMsg = $"failed to monitor folderPath: `{folderPathLowerCase}`. error: {ex.Message}.";
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(errorMsg) };
            }

            string successfulMsg = $"start monitoring folderPath: `{folderPathLowerCase}`.";
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(successfulMsg) };
        }

        public HttpResponseMessage RemoveFolder(string folderPath)
        {
            string folderPathLowerCase = folderPath.ToLower();
            if (!(_pathToMonitor.ContainsKey(folderPathLowerCase)))
            {
                string folderIsNotMonitoredMsg = $"folderPath: `{folderPathLowerCase}` is not monitored.";
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(folderIsNotMonitoredMsg) };
            }
            try
            {
                UnmonitorFolder(folderPathLowerCase);
            }
            catch (Exception ex)
            {
                var errorMsg = $"failed to unmonitor folderPath: `{folderPathLowerCase}`. error: {ex.Message}.";
                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(errorMsg) };
            }

            string successfulMsg = $"unmonitored folderPath: `{folderPathLowerCase}`.";
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(successfulMsg) };
        }

        private void UnmonitorFolder(string folderPathLowerCase)
        {
            throw new NotImplementedException();
        }

        private void MonitorNewFolder(string folderPathLowerCase)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage ReturnMaxNumberOfFoldersMonitoredResponse()
        {
            var folderpaths = GetMonitoredFolders();

            string contentMsg = $"already monitoring max number of folders allowed " +
                $"({_maxNumberOfFoldersToMonitor} folders).{Environment.NewLine}" +
                $"MonitoredPaths: {folderpaths}.{Environment.NewLine}" +
                $"Please unmonitor one of the folders in order to monitor a new folder.";

            // TODO what status code do i want?
            HttpResponseMessage maxNumberOfFoldersMonitoredMsg = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent(contentMsg)
            };

            return maxNumberOfFoldersMonitoredMsg;
        }

        private string GetMonitoredFolders()
        {
            return string.Join(Environment.NewLine, _pathToMonitor.Keys);


            //string folderPaths = string.Empty;
            //foreach (var Folderpath in _pathToMonitor.Keys)
            //{
            //    folderPaths += Folderpath + Environment.NewLine;
            //}
            //return folderPaths;
        }
    }
}
