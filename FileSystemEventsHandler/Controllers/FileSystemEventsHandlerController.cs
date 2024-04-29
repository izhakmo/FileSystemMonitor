using FileMonitor;
using Microsoft.AspNetCore.Mvc;

namespace FileSystemEventsHandler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileSystemEventsHandlerController : ControllerBase
    {
        private readonly ILogger<FileSystemEventsHandlerController> _logger;



        public FileSystemEventsHandlerController(ILogger<FileSystemEventsHandlerController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<string> Get()
        {
            return Enumerable.Range(1, 2).Select(index => new EventLogMsg(WatcherChangeTypes.Renamed, "ds","ds").ToString())
            .ToArray();
        }
    }
}