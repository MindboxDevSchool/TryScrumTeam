using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.StatisticsFacts.General
{
    public class NEventsRecordedFact: IStatisticsFact
    {
        public NEventsRecordedFact(ItHappenedSettings settings)
        {
            _settings = settings;
        }
        
        public double Priority { get; private set; }
        public string Description { get; private set; }
        public bool IsApplicable { get; private set; }
        
        public void Process(IEnumerable<Event> events, string trackName)
        {
            if (CheckApplicable(events))
            {
                int eventsCount = events.Count();
                IsApplicable = true;
                Priority = Math.Log(eventsCount);
                Description = $"У вас произошло уже {eventsCount} событи{GetTheCorrectWordEnding(eventsCount)}!";
            }
            else
            {
                IsApplicable = false;
            }
        }
        
        private bool CheckApplicable(IEnumerable<Event> events)
        {
            return events != null && events.Count() > _settings.NEventsRecorded.MinimalAmountOfEvents;
        }

        private char GetTheCorrectWordEnding(int eventsCount)
        {
            int lastNumber = eventsCount % 10;
            return lastNumber switch
            {
                0 => 'й',
                1 => 'е',
                2 => 'я',
                3 => 'я',
                4 => 'я',
                5 => 'й',
                6 => 'й',
                7 => 'й',
                8 => 'й',
                9 => 'й',
                _ => 'й'
            };
        }
        
        private readonly ItHappenedSettings _settings;
    }
}