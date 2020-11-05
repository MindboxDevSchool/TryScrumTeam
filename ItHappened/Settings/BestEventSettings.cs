namespace ItHappened
{
    public class BestEventSettings
    {
        public BestEventSettings(int eventsWithRatingCount, int daysSinceLastEvent, int daysSinceBestEvent)
        {
            MinimalAmountOfEventsWithRating = eventsWithRatingCount;
            DaysSinceLastEvent = daysSinceLastEvent;
            DaysSinceBestEvent = daysSinceBestEvent;
        }
        
        public int MinimalAmountOfEventsWithRating { get; }
        public int DaysSinceLastEvent { get; }
        public int DaysSinceBestEvent { get; }
    }
}