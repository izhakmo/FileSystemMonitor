﻿namespace FileMonitor
{
    public class PrintEventLogs : IPrintEventLogs
    {
        public IEnumerable<string> PrintLastEvents(int NumberOfLastEventsToPrint)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> PrintFolderLastEvents(string folderPath, string eventType, int NumberOfLastEventsToPrint)
        {
            throw new NotImplementedException();
        }
    }
}
