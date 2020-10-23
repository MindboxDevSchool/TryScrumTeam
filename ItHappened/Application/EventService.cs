using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;
using ItHappened.Domain.Exceptions;

namespace ItHappened.Application
{
    public class EventService : IEventService
    {
        public EventService(IEventRepository eventRepository, ITrackRepository trackRepository)
        {
            _eventRepository = eventRepository;
            _trackRepository = trackRepository;
        }

        private IEventRepository _eventRepository;
        private ITrackRepository _trackRepository;

        public IEnumerable<EventDto> GetEvents(Guid userId, Guid trackId)
        {
            var track = TryGetAccessToTrack(userId, trackId);

            var events = _eventRepository.TryGetEventsByTrack(trackId);
            return new List<EventDto>(events.Select(elem => new EventDto(elem)));
        }

        public EventDto CreateEvent(Guid userId, Guid trackId, DateTime createdAt, CustomizationsDto customizationsDto)
        {
            var track = TryGetAccessToTrack(userId, trackId);

            var customizations = new Customizations(customizationsDto, track.AllowedCustomizations);
            var newEvent = new Event(Guid.NewGuid(), createdAt, trackId, customizations);
            var createdEvent = _eventRepository.TryCreate(newEvent);
            return new EventDto(createdEvent);
        }

        public EventDto EditEvent(Guid userId, EventDto eventDto)
        {
            var track = TryGetAccessToTrack(userId, eventDto.TrackId);

            var customizations = new Customizations(eventDto.CustomizationDto, track.AllowedCustomizations);
            var oldEvent = _eventRepository.TryGetById(eventDto.Id);
            var editedEvent = new Event(eventDto.Id, oldEvent.CreatedAt, eventDto.TrackId, customizations);
            var editResult = _eventRepository.TryUpdate(editedEvent);
            return new EventDto(editResult);
        }

        public Guid DeleteEvent(Guid userId, Guid eventId)
        {
            var @event = _eventRepository.TryGetById(eventId);

            var track = TryGetAccessToTrack(userId, @event.TrackId);

            var deletedEventId = _eventRepository.TryDelete(eventId);
            return deletedEventId;
        }

        private Track TryGetAccessToTrack(Guid userId, Guid trackId)
        {
            var track = _trackRepository.TryGetTrackById(trackId);

            if (track.CreatorId != userId)
            {
                throw new DomainException(DomainExceptionType.TrackAccessDenied, userId, trackId);
            }

            return track;
        }
    }
}