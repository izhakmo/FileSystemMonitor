using System.Net;
using FileMonitor.Implementations;

namespace FileMonitorTestss
{
    [TestClass]
    public class FileSystemWatcherMonitorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "maxNumberOfFoldersToMonitor should be greather than 0.")]
        public void FileSystemWatcherMonitor_VerifyMaxNumberOfFoldersToMonitor_Fail_MustBeGreatherThen0()
        {
            new FileSystemWatcherMonitor(0);
        }

        [TestMethod]
        public async Task AddFolder_Fail_FolderAlreadyMonitored_StatusCodeConflict()
        {
            var fileSystemWatcherMonitor = new FileSystemWatcherMonitor(1);
            var expectedStatusCodeOne = HttpStatusCode.OK;
            var expectedStatusCodeTwo = HttpStatusCode.Conflict;

            HttpResponseMessage responseMessageSuccess = fileSystemWatcherMonitor.AddFolder(@"D:\");
            var responseOne = await responseMessageSuccess.Content.ReadAsStringAsync();

            HttpResponseMessage responseMessageFail = fileSystemWatcherMonitor.AddFolder(@"D:\");
            var responseTwo = await responseMessageFail.Content.ReadAsStringAsync();

            Assert.AreEqual(responseMessageSuccess.StatusCode, expectedStatusCodeOne,
                $"recevied status code: {responseMessageSuccess.StatusCode}, message: {responseOne}, " +
                $"but expected status code {expectedStatusCodeOne}");

            Assert.AreEqual(responseMessageFail.StatusCode, expectedStatusCodeTwo,
                $"recevied status code: {responseMessageFail.StatusCode}, message: {responseTwo}, " +
                $"but expected status code {expectedStatusCodeTwo}");
        }

        [TestMethod]
        public async Task AddFolder_Fail_MaxNumberOfFoldersMonitored_StatusCodeForbidden()
        {
            var fileSystemWatcherMonitor = new FileSystemWatcherMonitor(1);
            var expectedStatusCodeOne = HttpStatusCode.OK;
            var expectedStatusCodeTwo = HttpStatusCode.Forbidden;

            HttpResponseMessage responseMessageSuccess = fileSystemWatcherMonitor.AddFolder(@"D:\");
            var responseOne = await responseMessageSuccess.Content.ReadAsStringAsync();

            HttpResponseMessage responseMessageFail = fileSystemWatcherMonitor.AddFolder(@"C:\");
            var responseTwo = await responseMessageFail.Content.ReadAsStringAsync();

            Assert.AreEqual(responseMessageSuccess.StatusCode, expectedStatusCodeOne,
                $"recevied status code: {responseMessageSuccess.StatusCode}, message: {responseOne}, " +
                $"but expected status code {expectedStatusCodeOne}");

            Assert.AreEqual(responseMessageFail.StatusCode, expectedStatusCodeTwo,
                $"recevied status code: {responseMessageFail.StatusCode}, message: {responseTwo}, " +
                $"but expected status code {expectedStatusCodeTwo}");
        }

        [TestMethod]
        public async Task AddFolder_Fail_FolderDoesNotExist_StatusCodeBadRequest()
        {
            var fileSystemWatcherMonitor = new FileSystemWatcherMonitor(1);
            var expectedStatusCode = HttpStatusCode.BadRequest;

            HttpResponseMessage responseMessageFail = fileSystemWatcherMonitor.AddFolder(@"D:\fsdfds");
            var response = await responseMessageFail.Content.ReadAsStringAsync();

            Assert.AreEqual(responseMessageFail.StatusCode, expectedStatusCode,
                $"recevied status code: {responseMessageFail.StatusCode}, message: {response}, " +
                $"but expected status code {expectedStatusCode}");
        }

        [TestMethod]
        public void AddFolder_Success_StatusCodeOK()
        {
            var fileSystemWatcherMonitor = new FileSystemWatcherMonitor(1);
            var expectedStatusCode = HttpStatusCode.OK;

            HttpResponseMessage responseMessage = fileSystemWatcherMonitor.AddFolder(@"D:\");
            Assert.AreEqual(responseMessage.StatusCode, expectedStatusCode,
                $"recevied status code: {responseMessage.StatusCode}, message: {responseMessage.Content}, " +
                $"but expected status code {expectedStatusCode}");
        }
    }
}