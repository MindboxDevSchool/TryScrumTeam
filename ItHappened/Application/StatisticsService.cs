using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public class StatisticsService
    {
        public StatisticsService(IEventRepository eventRepository, 
            ItHappenedSettings settings)
        {
            _eventRepository = eventRepository;
            _settings = settings;
        }

        private readonly IEventRepository _eventRepository;
        private readonly ItHappenedSettings _settings;

        public EventDto GetBestEvent(Guid trackId)
        {
            Event bestEvent = null;
            var trackEvents = _eventRepository.TryGetEventsByTrack(trackId);
            var trackEventsWithRating = trackEvents
                .Where(@event => @event.Customization.Rating != null).ToList();

            if (trackEventsWithRating.Count() >= _settings.EventsWithRatingCountForSearchingBestEvent
                && DateTime.Compare(
                    trackEventsWithRating
                        .OrderByDescending(@event => @event.CreatedAt)
                        .Skip(_settings.EventsWithRatingCountForSearchingBestEvent - 1)
                        .First().CreatedAt,
                    DateTime.Now - TimeSpan.FromDays(_settings.DaysSinceLastEventForSearchingBestEvent)) > 0
                && DateTime.Compare(
                    trackEventsWithRating.First(@event =>
                            @event.Customization.Rating.Value ==
                            trackEventsWithRating.Max(@event => @event.Customization.Rating.Value))
                        .CreatedAt,
                    DateTime.Now - TimeSpan.FromDays(_settings.DaysSinceBestEventForSearchingBestEvent)) < 0)
            {
                bestEvent = trackEventsWithRating.FirstOrDefault(@event =>
                    @event.Customization.Rating.Value ==
                    trackEventsWithRating.Max(@event => @event.Customization.Rating.Value));
            }
            
            if (bestEvent == null)
                throw new DomainException(DomainExceptionType.BestEventNotFound);

            return new EventDto(bestEvent);
        }
    }
}