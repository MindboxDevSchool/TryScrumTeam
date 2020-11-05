using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.StatisticsFacts
{
    public class BestEventFact : IStatisticsFact
    {
        public BestEventFact(ItHappenedSettings settings)
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
                var bestEvent = eventsWithRating
                    .OrderBy(@event => @event.CreatedAt)
                    .First(@event =>
                    @event.Customization.Rating.Value ==
                    eventsWithRating.Max(@event => @event.Customization.Rating.Value));

                IsApplicable = true;
                Priority = bestEvent.Customization.Rating.Value;
                Description = $"Событие {trackName} с самым высоким рейтингом {bestEvent.Customization.Rating.Value}" +
                              $" произошло {bestEvent.CreatedAt.ToString()}";
                if (bestEvent.Customization.Comment != null)
                    Description += $" с комментарием \"{bestEvent.Customization.Comment.Value}\"";
            }
            else
            {
                IsApplicable = false;
            }
        }

        private bool CheckApplicable(IEnumerable<Event> eventsWithRating)
        {
            return eventsWithRating.Count() >= _settings.BestEvent.MinimalAmountOfEventsWithRating &&
                   DateTime.Compare(
                       eventsWithRating
                           .OrderBy(@event => @event.CreatedAt)
                           .First().CreatedAt,
                       DateTime.Now - TimeSpan.FromDays(_settings.BestEvent.DaysSinceLastEvent)) < 0 &&
                   DateTime.Compare(
                       eventsWithRating
                           .OrderBy(@event => @event.CreatedAt)
                           .First(@event =>
                               @event.Customization.Rating.Value ==
                               eventsWithRating.Max(@event => @event.Customization.Rating.Value)).CreatedAt,
                       DateTime.Now - TimeSpan.FromDays(_settings.BestEvent.DaysSinceBestEvent)) < 0;
        }

        private readonly ItHappenedSettings _settings;
    }
}