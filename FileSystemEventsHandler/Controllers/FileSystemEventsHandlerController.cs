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
        private ILogsManager _logsManager;


        public FileSystemEventsHandlerController(
            ILogger<FileSystemEventsHandlerController> logger,
            IPrintEventLogs printEventLogs,
            ILogsManager logsManager)
        {
            _logger = logger;
            _printEventLogs = printEventLogs;
            _logsManager = logsManager;
        }

        [HttpGet("PrintLastEvents")]
        public IEnumerable<EventLog> PrintLastEvents(int NumberOfLastEventsToPrint)
        {
            validateNumberOfLastEventsToPrint(NumberOfLastEventsToPrint);
            return _printEventLogs.PrintLastEvents(NumberOfLastEventsToPrint);
        }


        [HttpGet("PrintFolderLastEvents")]
        public IEnumerable<EventLog> PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint)
        {
            validateNumberOfLastEventsToPrint(NumberOfLastEventsToPrint);
            return _printEventLogs.PrintFolderLastEvents(folderPath, NumberOfLastEventsToPrint);
        }

        [HttpGet("PrintFolderLastEventsOfType")]
        public IEnumerable<EventLog> PrintFolderLastEventsOfType(string folderPath, string eventType, int NumberOfLastEventsToPrint)
        {
            validateNumberOfLastEventsToPrint(NumberOfLastEventsToPrint);
            validateEventType(eventType);
            var lastItems =  _printEventLogs.PrintFolderLastEventsOfType(folderPath, eventType, NumberOfLastEventsToPrint);
            foreach (var item in lastItems)
            {
                _logger.LogInformation($"{item}");
            }

            return lastItems;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("PostEventLog")]
        public IActionResult PostEventLog([FromBody] EventLog eventLog)
        {
            _logger.LogInformation($"[{nameof(PostEventLog)}] Received event log message: {eventLog}");

            _logsManager.AddEventLogToCache(eventLog);
            return Ok($"eventLog: `{eventLog}` received successfully.");
        }


        // TODO move validators to a different class
        private void validateNumberOfLastEventsToPrint(int NumberOfLastEventsToPrint)
        {
            if (NumberOfLastEventsToPrint < 1)
            {
                throw new ArgumentException($"NumberOfLastEventsToPrint is must be greather than 0. " +
                    $"(received value: {NumberOfLastEventsToPrint}).");
            };
        }

        private void validateEventType(string eventType)
        {
            var knownEventTypes = new string[] { "created", "deleted", "changed", "renamed" };
            if (!(knownEventTypes.Contains(eventType.ToLower())))
            {
                throw new ArgumentException($"Invalid eventType. received eventType: {eventType}.");
            };
        }
    }
}