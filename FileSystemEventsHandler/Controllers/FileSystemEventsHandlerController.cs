using FileMonitor;
using FileMonitor.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileSystemEventsHandler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileSystemEventsHandlerController : ControllerBase
    {
        private readonly ILogger<FileSystemEventsHandlerController> _logger;
        private IPrintEventLogs _printEventLogs;


        public FileSystemEventsHandlerController(ILogger<FileSystemEventsHandlerController> logger, IPrintEventLogs printEventLogs)
        {
            _logger = logger;
            _printEventLogs = printEventLogs;
        }

        [HttpGet("PrintLastEvents")]
        public IEnumerable<string> PrintLastEvents(int NumberOfLastEventsToPrint)
        {
            return _printEventLogs.PrintLastEvents(NumberOfLastEventsToPrint);
        }


        [HttpGet("PrintFolderLastEvents")]
        public IEnumerable<string> PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint)
        {
            return _printEventLogs.PrintFolderLastEvents(folderPath, NumberOfLastEventsToPrint);
        }

        [HttpGet("PrintFolderLastEvents")]
        public IEnumerable<string> PrintFolderLastEvents(string folderPath, string eventType, int NumberOfLastEventsToPrint)
        {
            return _printEventLogs.PrintFolderLastEvents(folderPath, eventType, NumberOfLastEventsToPrint);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("PostEventLog")]
        public IActionResult PostEventLog([FromBody] EventLog eventLogMsg)
        {
            _logger.LogInformation($"Received event log message: {eventLogMsg}");
            // TODO PostEventLog
            return Ok($"eventLogMsg: `{eventLogMsg}` received successfully.");
        }
    }
}