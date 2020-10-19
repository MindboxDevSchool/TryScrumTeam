using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure
{
    public class EventRepositoryInMemory : IEventRepository
    {
        private Dictionary<Guid, Event> _events = new Dictionary<Guid, Event>();
        
        public Result<Event> TryCreate(Event @event)
        {
            
            _events[@event.Id] = @event;
            return new Result<Event>(@event);
        }

        public Result<IEnumerable<Event>> TryGetEventsByTrack(Guid trackId)
        {
            var result = _events
                .Where(elem => elem.Value.TrackId == trackId)
                .Select(elem => elem.Value);
            return new Result<IEnumerable<Event>>(result);
        }

        public Result<Event> TryGetById(Guid id)
        {
            if (!_events.ContainsKey(id))
            {
                return new Result<Event>(new DataException());
            }
            return new Result<Event>(_events[id]);
        }

        public Result<Event> TryUpdate(Event @event)
        {
            if (!_events.ContainsKey(@event.Id))
            {
                return new Result<Event>(new DataException());
            }
            _events[@event.Id] = @event;
            return new Result<Event>(@event);
        }

        public Result<bool> TryDelete(Guid eventId)
        {
            _events.Remove(eventId);
            return new Result<bool>(true);
        }

        public Result<bool> TryDeleteByTrack(Guid trackId)
        {
            var eventsToDelete = _events.Where(elem => elem.Value.TrackId == trackId);
            foreach (var element in eventsToDelete)
            {
                _events.Remove(element.Key);
            }
            return new Result<bool>(true);
        }
    }
}