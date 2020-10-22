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
        
        public Result<IEnumerable<EventDto>> GetEvents(UserDto userDto, Guid trackId)
        {
            var track = _trackRepository.TryGetTrackById(trackId);
            
            if (!track.IsSuccessful())
            {
                return new Result<IEnumerable<EventDto>>(track.Exception);
            }
            
            if (track.Value.CreatorId != userDto.Id)
            {
                return new Result<IEnumerable<EventDto>>(new TrackAccessDeniedException(userDto.Id, trackId));
            }
            
            var events = _eventRepository.TryGetEventsByTrack(trackId);
            if (!events.IsSuccessful())
            {
                return new Result<IEnumerable<EventDto>>(events.Exception);
            }
            return new Result<IEnumerable<EventDto>>(events.Value.Select(elem => new EventDto(elem)));
        }

        public Result<EventDto> CreateEvent(UserDto userDto, Guid trackId, DateTime createdAt, Customizations customizations)
        {
            var track = _trackRepository.TryGetTrackById(trackId);
            
            if (!track.IsSuccessful())
            {
                return new Result<EventDto>(track.Exception);
            }
            
            if (track.Value.CreatorId != userDto.Id)
            {
                return new Result<EventDto>(new TrackAccessDeniedException(userDto.Id, trackId));
            }
            
            var newEvent = new Event(Guid.NewGuid(), createdAt, trackId, customizations);
            var result = _eventRepository.TryCreate(newEvent);
            if (!result.IsSuccessful())
            {
                return new Result<EventDto>(result.Exception);
            }
            var eventDto = new EventDto(newEvent);
            return new Result<EventDto>(eventDto);
        }

        public Result<EventDto> EditEvent(UserDto userDto, EventDto eventDto)
        {
            var track = _trackRepository.TryGetTrackById(eventDto.TrackId);
            
            if (!track.IsSuccessful())
            {
             return new Result<EventDto>(track.Exception);
            }
            
            if (track.Value.CreatorId != userDto.Id)
            {
                return new Result<EventDto>(new EventAccessDeniedException(userDto.Id, eventDto.Id));
            }

            var oldEvent = _eventRepository.TryGetById(eventDto.Id);
            if (oldEvent.Value.CreatedAt != eventDto.CreatedAt)
            {
                return new Result<EventDto>(new EditingImmutableDataException(nameof(eventDto.CreatedAt)));
            }
            if (oldEvent.Value.TrackId != eventDto.TrackId)
            {
                return new Result<EventDto>(new EditingImmutableDataException(nameof(eventDto.TrackId)));
            }

            var editedEvent = new Event(eventDto.Id, eventDto.CreatedAt, eventDto.TrackId, eventDto.Customization);
            var editResult = _eventRepository.TryUpdate(editedEvent);
            if (!editResult.IsSuccessful())
            {
                return new Result<EventDto>(editResult.Exception);
            }
            return new Result<EventDto>(new EventDto(editResult.Value));
        }

        public Result<bool> DeleteEvent(UserDto userDto, Guid eventId)
        {
            var @event = _eventRepository.TryGetById(eventId);
            if (!@event.IsSuccessful())
            {
                return new Result<bool>(@event.Exception);
            }
            
            var track = _trackRepository.TryGetTrackById(@event.Value.TrackId);
            
            if (!track.IsSuccessful())
            {
                return new Result<bool>(track.Exception);
            }
            
            if (track.Value.CreatorId != userDto.Id)
            {
                return new Result<bool>(new EventAccessDeniedException(userDto.Id, @event.Value.Id));
            }
            
            var deleteResult = _eventRepository.TryDelete(eventId);
            if (!deleteResult.IsSuccessful())
            {
                return new Result<bool>(deleteResult.Exception);
            }
            return new Result<bool>(deleteResult.Value);
        }
    }
}