namespace FileMonitor
{
    public interface IPrintEventLogs
    {
        // return eventType (string), FolderName (string), fileName (string), eventDate (date)
        void PrintLastEvents(int NumberOfLastEventsToPrint);


        // return eventType (string), FolderName (string), fileName (string), eventDate (date)
        void PrintFolderLastEvents(string folderPath, int NumberOfLastEventsToPrint);


        // return fileName (string), eventDate (date)
        void PrintFolderLastEvents(string folderPath, string eventType, int NumberOfLastEventsToPrint);

        // TODO swagger annotations
        // TODO webApis
        // TODO UTs
        // TODO DI - bootstrap or autofac
    }
}
