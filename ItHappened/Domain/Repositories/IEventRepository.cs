using System;
using System.Collections.Generic;

namespace ItHappened.Domain.Repositories
{
    public interface IEventRepository
    {
        Result<Event> TryCreate(Event @event);
        Result<IEnumerable<Event>> TryGetEventsByTrack(Guid trackId);
        Result<Event> TryUpdate(Event @event);
        Result<bool> TryDelete(Guid eventId);
        Result<bool> TryDeleteByTrack(Guid trackId);
    }
}