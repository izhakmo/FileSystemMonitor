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
                _logger.LogError($"[{nameof(PrintLastEvents)}] {numberOfEventsValidator.errorMsg}");
                return BadRequest(numberOfEventsValidator.errorMsg);
            }
            var lastEvents = _printEventLogs.PrintLastEvents(NumberOfLastEventsToPrint);
            foreach (var receivedEvent in lastEvents)
            {
                _logger.LogInformation($"[{nameof(PrintLastEvents)}] receivedEvent: {receivedEvent}");
            }
            return Ok(lastEvents);
        }


        [HttpGet("PrintFolderLastEvents")]
        public IActionResult PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint)
        {
            var numberOfEventsValidator = _inputValidator.ValidateNumberOfLastEventsToPrint(NumberOfLastEventsToPrint);
            if (!numberOfEventsValidator.isValidate)
            {
                _logger.LogError($"[{nameof(PrintFolderLastEvents)}] {numberOfEventsValidator.errorMsg}");
                return BadRequest(numberOfEventsValidator.errorMsg);
            }
            var lastEventsForFolder = _printEventLogs.PrintFolderLastEvents(folderPath, NumberOfLastEventsToPrint);
            foreach (var receivedEventForFolder in lastEventsForFolder)
            {
                _logger.LogInformation($"[{nameof(PrintFolderLastEvents)}] receivedEventForFolder: {receivedEventForFolder}");
            }
            return Ok(lastEventsForFolder);
        }

        [HttpGet("PrintFolderLastEventsOfType")]
        public IActionResult PrintFolderLastEventsOfType(string folderPath, string eventType, int NumberOfLastEventsToPrint)
        {
            var eventTypeToLower = eventType.ToLower();

            var numberOfEventsValidator = _inputValidator.ValidateNumberOfLastEventsToPrint(NumberOfLastEventsToPrint);
            if (!numberOfEventsValidator.isValidate)
            {
                _logger.LogError($"[{nameof(PrintFolderLastEventsOfType)}] {numberOfEventsValidator.errorMsg}");
                return BadRequest(numberOfEventsValidator.errorMsg);
            }
            var eventTypeValidator = _inputValidator.ValidateEventType(eventTypeToLower);
            if (!eventTypeValidator.isValidate)
            {
                _logger.LogError($"[{nameof(PrintFolderLastEventsOfType)}] {eventTypeValidator.errorMsg}");
                return BadRequest(eventTypeValidator.errorMsg);
            }
            var lastEventsForFolderOfType = _printEventLogs.PrintFolderLastEventsOfType(folderPath, eventTypeToLower, NumberOfLastEventsToPrint);
            foreach (var eventForFolderOfType in lastEventsForFolderOfType)
            {
                _logger.LogInformation($"[{nameof( PrintFolderLastEventsOfType)}] lastEventsForFolderOfType: {eventForFolderOfType}");
            }

            return Ok(lastEventsForFolderOfType);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("PostEventLog")]
        public IActionResult PostEventLog([FromBody] EventLog eventLog)
        {
            _logger.LogInformation($"[{nameof(PostEventLog)}] Received event log message: {eventLog}");
            try
            {
                _logsManager.AddEventLogToCache(eventLog);
            }
            catch (Exception ex)
            {

            }
            return Ok($"eventLog: `{eventLog}` received successfully.");
        }
    }
}