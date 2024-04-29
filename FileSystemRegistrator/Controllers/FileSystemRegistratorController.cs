using FileMonitor;
using Microsoft.AspNetCore.Mvc;

namespace FileSystemRegistrator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileSystemRegistratorController : ControllerBase
    {
        private readonly ILogger<FileSystemRegistratorController> _logger;



        public FileSystemRegistratorController(ILogger<FileSystemRegistratorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Get")]
        public IEnumerable<string> Get()
        {
            return Enumerable.Range(1, 2).Select(index => new EventLogMsg(WatcherChangeTypes.Renamed, "ds", "ds").ToString())
            .ToArray();
        }

        [HttpGet("GetAll")]
        public IEnumerable<string> GetAll()
        {
            return new List<string> { "Item 1", "Item 2", "Item 3" };
        }


        // TODO remove the comment of the hidden Api
        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("PostEventLog")]
        public IActionResult PostEventLog([FromBody] EventLogMsg eventLogMsg)
        {
            _logger.LogInformation($"Received event log message: {eventLogMsg}");
            return Ok($"eventLogMsg: `{eventLogMsg}` received successfully.");
        }
    }
}