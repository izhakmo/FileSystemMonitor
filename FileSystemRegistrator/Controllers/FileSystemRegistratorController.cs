using FileMonitor;
using Microsoft.AspNetCore.Mvc;

namespace FileSystemRegistrator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileSystemRegistratorController : ControllerBase
    {
        private readonly ILogger<FileSystemRegistratorController> _logger;
        private IFileSystemWatcherMonitor _fileSystemWatcherMonitor;


        // TODO add DI for the LogsManager
        public FileSystemRegistratorController(
            ILogger<FileSystemRegistratorController> logger, 
            IFileSystemWatcherMonitor fileSystemWatcherMonitor)
        {
            _logger = logger;

            LogsManager logsManager = new LogsManager();
            _fileSystemWatcherMonitor = fileSystemWatcherMonitor;
        }

        [HttpPost("AddFolder")]
        public HttpResponseMessage AddFolder(string folderPath)
        {
            return _fileSystemWatcherMonitor.AddFolder(folderPath);
        }

        [HttpPost("RemoveFolder")]
        public HttpResponseMessage RemoveFolder(string folderPath)
        {
            return _fileSystemWatcherMonitor.RemoveFolder(folderPath);
        }
    }
}