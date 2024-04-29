namespace FileMonitor
{
    public interface IFileSystemWatcherMonitor
    {
        HttpResponseMessage AddFolder(string folderPath);
        HttpResponseMessage RemoveFolder(string folderPath);
    }
}
