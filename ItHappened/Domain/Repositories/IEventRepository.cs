using System;
using System.Collections.Generic;

namespace ItHappened.Domain.Repositories
{
    public interface IEventRepository
    {
        Event TryCreate(Event @event);
        IEnumerable<Event> TryGetEventsByTrack(Guid trackId);

        Event TryGetById(Guid id);
        Event TryUpdate(Event @event);
        Guid TryDelete(Guid eventId);
        Guid TryDeleteByTrack(Guid trackId);
    }
}