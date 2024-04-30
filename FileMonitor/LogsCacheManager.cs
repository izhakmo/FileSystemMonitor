using log4net;

namespace FileMonitor
{
    // TODO need to test class
    public class LogsCacheManager : ILogsCacheManager
    {
        // TODO - add logs for the class !!!!!!
        //private readonly ILog _log = LogManager.GetLogger(typeof(LogsCacheManager));
        private readonly ILog _log;

        private Stack<EventLog> _allEventLogs;
        private Dictionary<string, Dictionary<string, Stack<EventLog>>> _logsByPathByChangeTypes;

        public LogsCacheManager(ILog log)
        {
            _allEventLogs = new Stack<EventLog>();
            _logsByPathByChangeTypes = new Dictionary<string, Dictionary<string, Stack<EventLog>>>();
            _log = log;
        }

        public void AddEventLogToCache(EventLog eventLog)
        {
            _allEventLogs.Push(eventLog);

            // not first log for the path
            if (_logsByPathByChangeTypes.TryGetValue(eventLog.FolderPath, out Dictionary<string, Stack<EventLog>>? _logsForPathDict))
            {
                _logsForPathDict.TryGetValue(Consts.AllLogsForFolderKey, out Stack<EventLog> allLogsForPath);
                allLogsForPath.Push(eventLog);

                // not first log for changeType for the path
                if (_logsForPathDict.TryGetValue(eventLog.GetChangeType(), out Stack<EventLog> logsByChangeType))
                {
                    logsByChangeType.Push(eventLog);
                }

                // first log for changeType for the path
                else
                {
                    logsByChangeType = new Stack<EventLog>();
                    logsByChangeType.Push(eventLog);

                    _logsForPathDict.Add(eventLog.GetChangeType(), logsByChangeType);
                }
            }

            // first log for the path
            else
            {
                var allLogsForPath = new Stack<EventLog>();
                allLogsForPath.Push(eventLog);

                var logsForPathByChangeType = new Stack<EventLog>();
                logsForPathByChangeType.Push(eventLog);

                _logsForPathDict = new Dictionary<string, Stack<EventLog>>
                {
                    { Consts.AllLogsForFolderKey, allLogsForPath },
                    { eventLog.GetChangeType(), logsForPathByChangeType }
                };

                _logsByPathByChangeTypes.Add(eventLog.FolderPath, _logsForPathDict);
            }
        }


        public IEnumerable<EventLog> PrintLastEvents(int NumberOfLastEventsToPrint)
        {
            return _allEventLogs.Take(NumberOfLastEventsToPrint);
        }

        public IEnumerable<EventLog> PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint)
        {
            return PrintFolderLastEventsOfType(folderPath, Consts.AllLogsForFolderKey, NumberOfLastEventsToPrint);
        }

        public IEnumerable<EventLog> PrintFolderLastEventsOfType(string folderPath, string eventType, int NumberOfLastEventsToPrint)
        {
            var eventTypeToLower = eventType.ToLower();
            bool isPathLogsExist = _logsByPathByChangeTypes.
                TryGetValue(folderPath, out Dictionary<string, Stack<EventLog>> dictForReceivedPath);
            if (!isPathLogsExist)
            {
                _log.Info($"[{PrintFolderLastEventsOfType}] no logs for path: {folderPath}.");
                return Enumerable.Empty<EventLog>();
            }
            Stack<EventLog> allLogsForReceivedPath = dictForReceivedPath.GetValueOrDefault(eventTypeToLower);
            var logsToReturn = allLogsForReceivedPath?.Take(NumberOfLastEventsToPrint) ?? Enumerable.Empty<EventLog>();
            _log.Info($"[{PrintFolderLastEventsOfType}] folderPath: {folderPath}, eventType: {eventTypeToLower}, " +
                $"numberOfLogsRequested:{NumberOfLastEventsToPrint}, returning {logsToReturn.Count()} logs.");
            return logsToReturn;
        }
    }
}
