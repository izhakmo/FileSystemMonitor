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

        public void Write(string directoryPath, EventLog eventLogMsg)
        {
            _allEventLogs.Push(eventLogMsg);

            if (_logsByPathByChangeTypes.TryGetValue(directoryPath, out Dictionary<string, Stack<EventLog>>? _logsForPathDict))
            {
                _logsForPathDict.TryGetValue(Consts.AllLogsForFolderKey, out Stack<EventLog> allLogsForPath);
                allLogsForPath.Push(eventLogMsg);

                if (_logsForPathDict.TryGetValue(eventLogMsg.GetChangeType(), out Stack<EventLog> logsByChangeType))
                {
                    logsByChangeType.Push(eventLogMsg);
                }

                // first log for changeType for this path
                else
                {
                    logsByChangeType = new Stack<EventLog>();
                    logsByChangeType.Push(eventLogMsg);

                    _logsForPathDict.Add(eventLogMsg.GetChangeType(), logsByChangeType);
                }
            }

            // first log for the path
            else
            {
                var allLogsForPath = new Stack<EventLog>();
                allLogsForPath.Push(eventLogMsg);

                var logsForPathByChangeType = new Stack<EventLog>();
                logsForPathByChangeType.Push(eventLogMsg);

                _logsForPathDict = new Dictionary<string, Stack<EventLog>>
                {
                    { Consts.AllLogsForFolderKey, allLogsForPath },
                    { eventLogMsg.GetChangeType(), logsForPathByChangeType }
                };

                _logsByPathByChangeTypes.Add(directoryPath, _logsForPathDict);
            }
        }

        public void RemoveMonitor(string directoryPath)
        {
            if (_logsByPathByChangeTypes.TryGetValue(directoryPath, out Dictionary<string, Stack<EventLog>>? _logsByEventType))
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
