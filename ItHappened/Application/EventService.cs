using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;
using ItHappened.Domain.Exceptions;

namespace ItHappened.Application
{
    public class EventService:IEventService
    {
        public EventService(IEventRepository eventRepository,ITrackRepository trackRepository)
        {
            _eventRepository = eventRepository;
            _trackRepository = trackRepository;
        }

        private IEventRepository _eventRepository;
        private ITrackRepository _trackRepository;
        
        public IEnumerable<EventDto> GetEvents(AuthData authData, Guid trackId)
        {
            var track = _trackRepository.TryGetTrackById(trackId);

            if (track.CreatorId != authData.Id)
            {
               throw new DomainException(DomainExceptionType.TrackAccessDenied, authData.Id, trackId);
            }
            
            var events = _eventRepository.TryGetEventsByTrack(trackId);
            return new List<EventDto>(events.Select(elem => new EventDto(elem)));
        }

        public EventDto CreateEvent(AuthData authData, Guid trackId, DateTime createdAt, Customizations customizations)
        {
            var track = _trackRepository.TryGetTrackById(trackId);

            if (track.CreatorId != authData.Id)
            {
                throw new DomainException(DomainExceptionType.TrackAccessDenied, authData.Id, trackId);
            }
            
            var newEvent = new Event(Guid.NewGuid(), createdAt, trackId, customizations);
            var createdEvent = _eventRepository.TryCreate(newEvent);
            return new EventDto(createdEvent);
        }

        public EventDto EditEvent(AuthData authData, EventDto eventDto)
        {
            var track = _trackRepository.TryGetTrackById(eventDto.TrackId);

            if (track.CreatorId != authData.Id)
            {
                throw new DomainException(DomainExceptionType.EventAccessDenied, authData.Id, eventDto.Id);
            }

            var oldEvent = _eventRepository.TryGetById(eventDto.Id);
            var editedEvent = new Event(eventDto.Id, oldEvent.CreatedAt, eventDto.TrackId, eventDto.Customization);
            var editResult = _eventRepository.TryUpdate(editedEvent);
            return new EventDto(editResult);
        }

        public Guid DeleteEvent(AuthData authData, Guid eventId)
        {
            var @event = _eventRepository.TryGetById(eventId);

            var track = _trackRepository.TryGetTrackById(@event.TrackId);

            if (track.CreatorId != authData.Id)
            {
                throw new DomainException(DomainExceptionType.EventAccessDenied, authData.Id, @event.Id);
            }
            
            var deletedEventId = _eventRepository.TryDelete(eventId);
            return deletedEventId;
        }
    }
}