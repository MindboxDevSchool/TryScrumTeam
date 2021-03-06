﻿using System.IO;
using Newtonsoft.Json;

namespace ItHappened
{
    public class ItHappenedSettings
    {
        public ItHappenedSettings(TrackSettingsForRatingFacts bestEvent, TrackSettingsForRatingFacts worstEvent, NEventsRecordedFactSettings nEventsRecorded)
        {
            BestEvent = bestEvent;
            WorstEvent = worstEvent;
            NEventsRecorded = nEventsRecorded;
        }
        
        public static ItHappenedSettings FromJsonFile(string filePath)
        {
            var settingInJson = File.ReadAllText(filePath);

            var settings = JsonConvert.DeserializeObject<ItHappenedSettings>(settingInJson);

            return settings;
        }

        public TrackSettingsForRatingFacts BestEvent { get; }
        public TrackSettingsForRatingFacts WorstEvent { get; }
        public NEventsRecordedFactSettings NEventsRecorded { get; }
    }
}