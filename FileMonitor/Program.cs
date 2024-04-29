using FileMonitor.Implementations;
using log4net.Config;

namespace DirectoryWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"C:\Users\izhak\Downloads\dasdasdasd";
            FileSystemMonitor monitor = new FileSystemMonitor(directoryPath);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
