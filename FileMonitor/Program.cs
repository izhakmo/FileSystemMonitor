using FileMonitor;
using log4net.Config;

namespace DirectoryWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"C:\Users\izhak\Downloads\dasdasdasd";
            //BasicConfigurator.Configure();
            FileSystemMonitor monitor = new FileSystemMonitor(directoryPath);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
