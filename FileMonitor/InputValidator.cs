namespace FileMonitor
{
    public class InputValidator
    {
        public (bool isValidate, string errorMsg) validateNumberOfLastEventsToPrint(int NumberOfLastEventsToPrint)
        {
            if (NumberOfLastEventsToPrint < 1)
            {
                return (false,
                    $"NumberOfLastEventsToPrint is must be greather than 0. " +
                    $"(received value: {NumberOfLastEventsToPrint}).");
            };
            return (true, string.Empty);
        }

        public (bool isValidate, string errorMsg) validateEventType(string eventTypeToLower)
        {
            var knownEventTypes = new string[] { "created", "deleted", "changed", "renamed" };
            if (!(knownEventTypes.Contains(eventTypeToLower)))
            {
                var validEventTypes = string.Join(", ", knownEventTypes);
                return (false, $"Invalid eventType. received eventType: {eventTypeToLower}. validEventTypes: {validEventTypes}.");
            };
            return (true, string.Empty);
        }

    }
}
