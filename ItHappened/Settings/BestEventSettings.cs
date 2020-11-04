namespace ItHappened
{
    public class BestEventSettings
    {
        public BestEventSettings(int eventsWithRatingCount, int daysSinceLastEvent, int daysSinceBestEvent)
        {
            EventsWithRatingCount = eventsWithRatingCount;
            DaysSinceLastEvent = daysSinceLastEvent;
            DaysSinceBestEvent = daysSinceBestEvent;
        }
        
        public int EventsWithRatingCount { get; }
        public int DaysSinceLastEvent { get; }
        public int DaysSinceBestEvent { get; }
    }
}