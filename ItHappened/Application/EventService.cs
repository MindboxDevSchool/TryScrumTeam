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

        public IEnumerable<EventDto> GetEvents(Guid userId, Guid trackId, int? take = null, int? skip = null)
        {
            var track = TryGetAccessToTrack(userId, trackId);

            var events = _eventRepository.TryGetEventsByTrack(trackId, take, skip);
            return new List<EventDto>(events.Select(elem => new EventDto(elem)));
        }

        public EventDto GetEvent(Guid userId, Guid trackId, Guid eventId)
        {
            var track = TryGetAccessToTrack(userId, trackId);

            var @event = _eventRepository.TryGetById(eventId);
            return new EventDto(@event);
        }

        public EventDto CreateEvent(Guid userId, Guid trackId, DateTime createdAt, CustomizationsDto customizationsDto)
        {
            var track = TryGetAccessToTrack(userId, trackId);

            CheckNotAllowedCustomizationsInDto(customizationsDto, track.AllowedCustomizations, trackId);
            
            var customizations = new Customizations(customizationsDto, track.AllowedCustomizations);
            var newEvent = new Event(Guid.NewGuid(), createdAt, trackId, customizations);
            var createdEvent = _eventRepository.TryCreate(newEvent);
            return new EventDto(createdEvent);
        }

        public EventDto EditEvent(Guid userId, EventToEditDto eventDto)
        {
            var oldEvent = _eventRepository.TryGetById(eventDto.Id);
            var track = TryGetAccessToTrack(userId, oldEvent.TrackId);

            var oldCustomizations = Customizations.GetCustomizationTypes(oldEvent.Customization.GetDto());
            var allowedCustomizations = track.AllowedCustomizations.Concat(oldCustomizations);
            CheckNotAllowedCustomizationsInDto(eventDto.CustomizationDto, allowedCustomizations, track.Id);
            
            var customizations = new Customizations(eventDto.CustomizationDto, allowedCustomizations);
            var editedEvent = new Event(eventDto.Id, oldEvent.CreatedAt, track.Id, customizations);
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

        private void CheckNotAllowedCustomizationsInDto(
            CustomizationsDto customizationsDto,
            IEnumerable<CustomizationType> allowedCustomizations, 
            Guid trackId)
        {
            var givenCustomizations = Customizations.GetCustomizationTypes(customizationsDto);
            
            var notAllowedCustomizations =
                givenCustomizations.Where(c => !allowedCustomizations.Contains(c)).ToList();
            
            if (notAllowedCustomizations.Any())
                throw new DomainException(DomainExceptionType.NotAllowedCustomizations, trackId, notAllowedCustomizations);
        }
    }
}