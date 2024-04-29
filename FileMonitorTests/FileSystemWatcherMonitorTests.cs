using FileMonitor;

namespace FileMonitorTestss
{
    [TestClass]
    public class FileSystemWatcherMonitorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "maxNumberOfFoldersToMonitor should be greather than 0.")]
        public void verifyMaxSize_fail()
        {
            var a = new FileSystemWatcherMonitor(0);
        }

        [TestMethod]
        public void verifyMaxSize_fail22()
        {
            var a = new FileSystemWatcherMonitor(1);
        }
    }
}