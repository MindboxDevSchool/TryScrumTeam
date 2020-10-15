﻿using System;
using System.Collections.Generic;
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

        public Result<Event> TryUpdate(Event @event)
        {
            _events[@event.Id] = @event;
            return new Result<Event>(@event);
        }

        public Result<bool> TryDelete(Guid eventId)
        {
            _events.Remove(eventId);
            return new Result<bool>(true);
        }
    }
}