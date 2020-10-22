using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure
{
    public class EventRepositoryInMemory : IEventRepository
    {
        private Dictionary<Guid, Event> _events = new Dictionary<Guid, Event>();
        
        public Event TryCreate(Event @event)
        {
            _events[@event.Id] = @event;
            return @event;
        }

        public IEnumerable<Event> TryGetEventsByTrack(Guid trackId)
        {
            var result = _events
                .Where(elem => elem.Value.TrackId == trackId)
                .Select(elem => elem.Value);
            return result;
        }

        public Event TryGetById(Guid eventId)
        {
            if (!_events.ContainsKey(eventId))
            {
                throw new RepositoryException(RepositoryExceptionType.EventNotFound, eventId);
            }
            return _events[eventId];
        }

        public Event TryUpdate(Event @event)
        {
            if (!_events.ContainsKey(@event.Id))
            {
                throw new RepositoryException(RepositoryExceptionType.EventNotFound, @event.Id);
            }
            _events[@event.Id] = @event;
            return @event;
        }

        public Guid TryDelete(Guid eventId)
        {
            _events.Remove(eventId);
            return eventId;
        }

        public Guid TryDeleteByTrack(Guid trackId)
        {
            var eventsToDelete = _events.Where(elem => elem.Value.TrackId == trackId);
            foreach (var element in eventsToDelete)
            {
                _events.Remove(element.Key);
            }
            return trackId;
        }
    }
}