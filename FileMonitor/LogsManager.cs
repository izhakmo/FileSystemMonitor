namespace FileMonitor
{

    // TODO need to test class
    public class LogsManager : ILogsManager
    {
        private Stack<EventLogMsg> _allEventLogs;
        private Dictionary<string, Dictionary<string, Stack<EventLogMsg>>> _logsByPathByChangeTypes;

        public LogsManager()
        {
            _allEventLogs = new Stack<EventLogMsg>();
            _logsByPathByChangeTypes = new Dictionary<string, Dictionary<string, Stack<EventLogMsg>>>();
        }

        public void Write(string directoryPath, EventLogMsg eventLogMsg)
        {
            _allEventLogs.Push(eventLogMsg);

            if (_logsByPathByChangeTypes.TryGetValue(directoryPath, out Dictionary<string, Stack<EventLogMsg>>? _logsForPathDict))
            {
                _logsForPathDict.TryGetValue(Consts.AllLogsForFolderKey, out Stack<EventLogMsg> allLogsForPath);
                allLogsForPath.Push(eventLogMsg);

                if (_logsForPathDict.TryGetValue(eventLogMsg.GetChangeType(), out Stack<EventLogMsg> logsByChangeType))
                {
                    logsByChangeType.Push(eventLogMsg);
                }

                // first log for changeType for this path
                else
                {
                    logsByChangeType = new Stack<EventLogMsg>();
                    logsByChangeType.Push(eventLogMsg);

                    _logsForPathDict.Add(eventLogMsg.GetChangeType(), logsByChangeType);
                }
            }

            // first log for the path
            else
            {
                var allLogsForPath = new Stack<EventLogMsg>();
                allLogsForPath.Push(eventLogMsg);

                var logsForPathByChangeType = new Stack<EventLogMsg>();
                logsForPathByChangeType.Push(eventLogMsg);

                _logsForPathDict = new Dictionary<string, Stack<EventLogMsg>>
                {
                    { Consts.AllLogsForFolderKey, allLogsForPath },
                    { eventLogMsg.GetChangeType(), logsForPathByChangeType }
                };

                _logsByPathByChangeTypes.Add(directoryPath, _logsForPathDict);
            }
        }

        public void RemoveMonitor(string directoryPath)
        {
            if (_logsByPathByChangeTypes.TryGetValue(directoryPath, out Dictionary<string, Stack<EventLogMsg>>? _logsByEventType))
            {
                foreach (var a in _logsByEventType)
                {
                    _logsByEventType.Remove(a.Key);
                    a.Value.Clear();
                }
            }
        }
    }
}
