using System.IO;
using Newtonsoft.Json;

namespace ItHappened
{
    public class ItHappenedSettings
    {
        public ItHappenedSettings(BestEventSettings bestEvent)
        {
            BestEvent = bestEvent;
        }
        
        public static ItHappenedSettings FromJsonFile(string filePath)
        {
            var settingInJson = File.ReadAllText(filePath);

            var settings = JsonConvert.DeserializeObject<ItHappenedSettings>(settingInJson);

            return settings;
        }

        public BestEventSettings BestEvent { get; }
    }
}