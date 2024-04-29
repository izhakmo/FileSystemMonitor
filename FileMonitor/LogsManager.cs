namespace FileMonitor
{
    // TODO need to test class
    public class LogsManager : ILogsManager
    {
        private Stack<EventLog> _allEventLogs;
        private Dictionary<string, Dictionary<string, Stack<EventLog>>> _logsByPathByChangeTypes;

        public LogsManager()
        {
            _allEventLogs = new Stack<EventLog>();
            _logsByPathByChangeTypes = new Dictionary<string, Dictionary<string, Stack<EventLog>>>();
        }

        public void AddEventLogToCache(EventLog eventLog)
        {
            _allEventLogs.Push(eventLog);

            if (_logsByPathByChangeTypes.TryGetValue(eventLog.FolderPath, out Dictionary<string, Stack<EventLog>>? _logsForPathDict))
            {
                _logsForPathDict.TryGetValue(Consts.AllLogsForFolderKey, out Stack<EventLog> allLogsForPath);
                allLogsForPath.Push(eventLog);

                if (_logsForPathDict.TryGetValue(eventLog.GetChangeType(), out Stack<EventLog> logsByChangeType))
                {
                    logsByChangeType.Push(eventLog);
                }

                // first log for changeType for this path
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
    }
}
