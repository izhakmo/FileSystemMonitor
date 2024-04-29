namespace FileMonitor.Interfaces
{
    public interface IPrintEventLogs
    {
        // return eventType (string), FolderName (string), fileName (string), eventDate (date)
        IEnumerable<string> PrintLastEvents(int NumberOfLastEventsToPrint);


        // return eventType (string), FolderName (string), fileName (string), eventDate (date)
        IEnumerable<string> PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint);


        // return fileName (string), eventDate (date)
        IEnumerable<string> PrintFolderLastEvents(string folderPath, string eventType, int NumberOfLastEventsToPrint);


        // TODO UTs
        // TODO DI - bootstrap or autofac
    }
}
