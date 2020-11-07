namespace ItHappened
{
    public class NEventsRecordedFactSettings
    {
        public NEventsRecordedFactSettings(int minimalAmountOfEvents)
        {
            MinimalAmountOfEvents = minimalAmountOfEvents;
        }

        public int MinimalAmountOfEvents { get; }
    }
}