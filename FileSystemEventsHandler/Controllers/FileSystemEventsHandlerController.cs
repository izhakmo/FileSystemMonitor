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
        private readonly InputValidator _inputValidator;
        private IPrintEventLogs _printEventLogs;
        private ILogsCacheManager _logsManager;


        public FileSystemEventsHandlerController(
            ILogger<FileSystemEventsHandlerController> logger,
            InputValidator inputValidator,
            IPrintEventLogs printEventLogs,
            ILogsCacheManager logsManager)
        {
            _logger = logger;
            _inputValidator = inputValidator;
            _printEventLogs = printEventLogs;
            _logsManager = logsManager;
        }

        [HttpGet("PrintLastEvents")]
        public IActionResult PrintLastEvents(int NumberOfLastEventsToPrint)
        {
            var numberOfEventsValidator = _inputValidator.ValidateNumberOfLastEventsToPrint(NumberOfLastEventsToPrint);
            if (!numberOfEventsValidator.isValidate)
            {
                return BadRequest(numberOfEventsValidator.errorMsg);
            }
            return Ok(_printEventLogs.PrintLastEvents(NumberOfLastEventsToPrint));
        }


        [HttpGet("PrintFolderLastEvents")]
        public IActionResult PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint)
        {
            var numberOfEventsValidator = _inputValidator.ValidateNumberOfLastEventsToPrint(NumberOfLastEventsToPrint);
            if (!numberOfEventsValidator.isValidate)
            {
                return BadRequest(numberOfEventsValidator.errorMsg);
            }
            return Ok(_printEventLogs.PrintFolderLastEvents(folderPath, NumberOfLastEventsToPrint));
        }

        [HttpGet("PrintFolderLastEventsOfType")]
        public IActionResult PrintFolderLastEventsOfType(string folderPath, string eventType, int NumberOfLastEventsToPrint)
        {
            var eventTypeToLower = eventType.ToLower();

            var numberOfEventsValidator = _inputValidator.ValidateNumberOfLastEventsToPrint(NumberOfLastEventsToPrint);
            if (!numberOfEventsValidator.isValidate)
            {
                return BadRequest(numberOfEventsValidator.errorMsg);
            }
            var eventTypeValidator = _inputValidator.ValidateEventType(eventTypeToLower);
            if (!eventTypeValidator.isValidate)
            {
                return BadRequest(eventTypeValidator.errorMsg);
            }
            var lastItems = _printEventLogs.PrintFolderLastEventsOfType(folderPath, eventTypeToLower, NumberOfLastEventsToPrint);
            foreach (var item in lastItems)
            {
                _logger.LogInformation($"{item}");
            }

            return Ok(lastItems);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("PostEventLog")]
        public IActionResult PostEventLog([FromBody] EventLog eventLog)
        {
            _logger.LogInformation($"[{nameof(PostEventLog)}] Received event log message: {eventLog}");

            _logsManager.AddEventLogToCache(eventLog);
            return Ok($"eventLog: `{eventLog}` received successfully.");
        }
    }
}