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

        [HttpGet("Get")]
        public IEnumerable<string> Get()
        {
            return Enumerable.Range(1, 2).Select(index => new EventLog(WatcherChangeTypes.Renamed, "ds", "ds").ToString())
            .ToArray();
        }

        // TODO remove the comment of the hidden Api
        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("PostEventLog")]
        public IActionResult PostEventLog([FromBody] EventLog eventLogMsg)
        {
            _logger.LogInformation($"Received event log message: {eventLogMsg}");
            return Ok($"eventLogMsg: `{eventLogMsg}` received successfully.");
        }
    }
}