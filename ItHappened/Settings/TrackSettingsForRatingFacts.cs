namespace ItHappened
{
    public class TrackSettingsForRatingFacts
    {
        public TrackSettingsForRatingFacts(int eventsWithRatingCount, int daysSinceEarliestEvent, int daysSinceSoughtEvent)
        {
            MinimalAmountOfEventsWithRating = eventsWithRatingCount;
            DaysSinceEarliestEvent = daysSinceEarliestEvent;
            DaysSinceSoughtEvent = daysSinceSoughtEvent;
        }
        
        public int MinimalAmountOfEventsWithRating { get; }
        public int DaysSinceEarliestEvent { get; }
        public int DaysSinceSoughtEvent { get; }
    }
}