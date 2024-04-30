using FileMonitor;

namespace FileMonitorTests
{
    [TestClass]
    public class InputValidatorTests
    {
        private readonly InputValidator validator = new InputValidator();
        
        [TestMethod]
        public void ValidateNumberOfLastEventsToPrint_ReturnTrue()
        {
            int NumberOfLastEventsToPrint = 3;
            var expectedResult = true;
            var result = validator.ValidateNumberOfLastEventsToPrint(NumberOfLastEventsToPrint);
            Assert.AreEqual(expectedResult, result.isValidate,
                "valid NumberOfLastEventsToPrint should be any integer greather then 0");
        }

        [TestMethod]
        public void ValidateNumberOfLastEventsToPrint_ReturnFalse()
        {
            int NumberOfLastEventsToPrint = 0;
            var expectedResult = false;
            var result = validator.ValidateNumberOfLastEventsToPrint(NumberOfLastEventsToPrint);
            Assert.AreEqual(expectedResult, result.isValidate,
                "valid NumberOfLastEventsToPrint should be any integer greather then 0. " +
                "test should return false for input < 1.");
        }

        [TestMethod]
        [DataRow("created")]
        [DataRow("deleted")]
        [DataRow("changed")]
        [DataRow("renamed")]
        public void ValidateEventType_ReturnTrue(string knownEventType)
        {
            var expectedResult = true;
            var result = validator.ValidateEventType(knownEventType);
            Assert.AreEqual(expectedResult, result.isValidate, result.errorMsg);
        }

        [TestMethod]
        public void ValidateEventType_ReturnFalse()
        {
            string unknownEventType = "moshe";
            var expectedResult = false;
            var result = validator.ValidateEventType(unknownEventType);
            Assert.AreEqual(expectedResult, result.isValidate, result.errorMsg);
        }
    }
}
