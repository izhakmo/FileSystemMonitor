using FileMonitor.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileSystemRegistrator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileSystemRegistratorController : ControllerBase
    {
        private IFileSystemWatcherMonitor _fileSystemWatcherMonitor;

        public FileSystemRegistratorController(IFileSystemWatcherMonitor fileSystemWatcherMonitor)
        {
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