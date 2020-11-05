using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.StatisticsFacts
{
    public class WorstEventFact : IStatisticsFact
    {
        public WorstEventFact(ItHappenedSettings settings)
        {
            _settings = settings;
        }
        
        public double Priority { get; private set; }
        public string Description { get; private set; }
        public bool IsApplicable { get; private set; }

        public void Process(IEnumerable<Event> events, string trackName)
        {
            var eventsWithRating = events
                .Where(@event => @event.Customization.Rating != null).ToList();

            if (CheckApplicable(eventsWithRating))
            {
                var worstEvent = eventsWithRating
                    .OrderBy(@event => @event.CreatedAt)
                    .First(@event =>
                    @event.Customization.Rating.Value ==
                    eventsWithRating.Min(@event => @event.Customization.Rating.Value));

                IsApplicable = true;
                Priority = 10 - worstEvent.Customization.Rating.Value;
                Description = $"Событие {trackName} с самым низким рейтингом {worstEvent.Customization.Rating.Value}" +
                              $" произошло {worstEvent.CreatedAt.ToString()}";
                if (worstEvent.Customization.Comment != null)
                    Description += $" с комментарием \"{worstEvent.Customization.Comment.Value}\"";
            }
            else
            {
                IsApplicable = false;
            }
        }

        private bool CheckApplicable(IEnumerable<Event> eventsWithRating)
        {
            return eventsWithRating.Count() >= _settings.WorstEvent.MinimalAmountOfEventsWithRating &&
                   DateTime.Compare(
                       eventsWithRating
                           .OrderBy(@event => @event.CreatedAt)
                           .First().CreatedAt,
                       DateTime.Now - TimeSpan.FromDays(_settings.WorstEvent.DaysSinceEarliestEvent)) < 0 &&
                   DateTime.Compare(
                       eventsWithRating
                           .OrderBy(@event => @event.CreatedAt)
                           .First(@event =>
                               @event.Customization.Rating.Value ==
                               eventsWithRating.Min(@event => @event.Customization.Rating.Value)).CreatedAt,
                       DateTime.Now - TimeSpan.FromDays(_settings.WorstEvent.DaysSinceSoughtEvent)) < 0;
        }

        private readonly ItHappenedSettings _settings;
    }
}