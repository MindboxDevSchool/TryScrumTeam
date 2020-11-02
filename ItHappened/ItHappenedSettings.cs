using System.IO;
using Newtonsoft.Json;

namespace ItHappened
{
    public class ItHappenedSettings
    {
        public ItHappenedSettings(
            int eventsWithRatingCountForSearchingBestEvent, 
            int daysSinceLastEventForSearchingBestEvent, 
            int daysSinceBestEventForSearchingBestEvent)
        {
            EventsWithRatingCountForSearchingBestEvent = eventsWithRatingCountForSearchingBestEvent;
            DaysSinceLastEventForSearchingBestEvent = daysSinceLastEventForSearchingBestEvent;
            DaysSinceBestEventForSearchingBestEvent = daysSinceBestEventForSearchingBestEvent;
        }
        
        public static ItHappenedSettings FromJsonFile(string filePath)
        {
            var settingInJson = File.ReadAllText(filePath);

            var settings = JsonConvert.DeserializeObject<ItHappenedSettings>(settingInJson);

            return settings;
        }

        public int EventsWithRatingCountForSearchingBestEvent { get; }
        public int DaysSinceLastEventForSearchingBestEvent { get; }
        public int DaysSinceBestEventForSearchingBestEvent { get; }
    }
}