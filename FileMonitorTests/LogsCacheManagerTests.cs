using FileMonitor.Implementations;
using log4net;
using Moq;
using FileMonitor;

namespace FileMonitorTests
{
    [TestClass]
    public class LogsCacheManagerTests
    {
        private readonly ILog _log = new Mock<ILog>().Object;

        [TestMethod]
        public void PrintLastEvents_ReturnsCorrectNumberOfEvents_WhenEventsExist()
        {
            var logsCacheManager = new LogsCacheManager(_log);
            int numberOfEventsToPrint = 5;

            string filePath = @"D:\moshe.exe";

            // Add events to cache
            for (int i = 0; i < 10; i++)
            {
                EventLog eventLog = new EventLog(WatcherChangeTypes.Changed, filePath,string.Empty) ;
                logsCacheManager.AddEventLogToCache(eventLog);
            }

            var result = logsCacheManager.PrintLastEvents(numberOfEventsToPrint);
            var actualNumberOfEvents = result.Count();

            Assert.AreEqual(numberOfEventsToPrint, actualNumberOfEvents, 
                $"expectedNumberOfEvents: {numberOfEventsToPrint}, actualNumberOfEvents: {actualNumberOfEvents}."); 
        }

        [TestMethod]
        public void PrintLastEvents_ReturnsEmptyList_WhenNoEventsExist()
        {
            var logsCacheManager = new LogsCacheManager(_log);
            int numberOfEventsToPrint = 5; 

            var result = logsCacheManager.PrintLastEvents(numberOfEventsToPrint);
            var actualNumberOfEvents = result.Count();

            Assert.IsNotNull(result); 
            Assert.AreEqual(0, result.Count(),
                $"expectedNumberOfEvents: {numberOfEventsToPrint}, actualNumberOfEvents: {actualNumberOfEvents}.");
        }

        [TestMethod]
        public void PrintFolderLastEvents_ReturnsListOfEventsFromTheSameFolder()
        {
            var logsCacheManager = new LogsCacheManager(_log);
            int numberOfEventsToPrint = 5;

            string filePathOne = @"D:\DJ.exe";
            string filePathTwo = @"D:\moshe\cohen.exe";

            string expectedFolder = @"D:\";

            // Add events to cache
            for (int i = 0; i < 5; i++)
            {
                EventLog eventLog = new EventLog(WatcherChangeTypes.Changed, filePathOne, string.Empty);
                logsCacheManager.AddEventLogToCache(eventLog);

                EventLog eventLogTwo = new EventLog(WatcherChangeTypes.Changed, filePathTwo, string.Empty);
                logsCacheManager.AddEventLogToCache(eventLogTwo);
            }

            var results = logsCacheManager.PrintFolderLastEvents(expectedFolder, numberOfEventsToPrint);
            var actualNumberOfEvents = results.Count();

            Assert.AreEqual(numberOfEventsToPrint, actualNumberOfEvents,
                $"expectedNumberOfEvents: {numberOfEventsToPrint}, actualNumberOfEvents: {actualNumberOfEvents}.");
            
            foreach(var result in results)
            {
                Assert.AreEqual(expectedFolder, result.FolderPath,
                $"folder path does not match to expected folder path.");
            }
        }

        [TestMethod]
        public void PrintFolderLastEventsOfType_ReturnsListOfEventsWithTheExpectedType()
        {
            var logsCacheManager = new LogsCacheManager(_log);
            int numberOfEventsToPrint = 5;
            var expectedEventType = WatcherChangeTypes.Created;

            string filePath = @"D:\moshe.exe";

            string expectedFolder = @"D:\";

            // Add events to cache
            for (int i = 0; i < 5; i++)
            {
                EventLog eventLog = new EventLog(WatcherChangeTypes.Changed, filePath, string.Empty);
                logsCacheManager.AddEventLogToCache(eventLog);

                EventLog eventLogTwo = new EventLog(WatcherChangeTypes.Created, filePath, string.Empty);
                logsCacheManager.AddEventLogToCache(eventLogTwo);
            }

            var results = logsCacheManager.
                PrintFolderLastEventsOfType(expectedFolder, expectedEventType.ToString().ToLower(), numberOfEventsToPrint);
            var actualNumberOfEvents = results.Count();

            Assert.AreEqual(numberOfEventsToPrint, actualNumberOfEvents,
                $"expectedNumberOfEvents: {numberOfEventsToPrint}, actualNumberOfEvents: {actualNumberOfEvents}.");

            foreach (var result in results)
            {
                Assert.AreEqual(result.GetChangeType(), expectedEventType.ToString().ToLower(),
                $"ChangeType does not match to expected ChangeType. expected: {expectedEventType.ToString().ToLower()}, " +
                $"actual: {result.GetChangeType()}");
            }
        }
    }
}
