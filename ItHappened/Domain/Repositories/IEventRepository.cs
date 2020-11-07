using System;
using System.Collections.Generic;

namespace ItHappened.Domain.Repositories
{
    public interface IEventRepository
    {
        Event TryCreate(Event @event);
        IEnumerable<Event> TryGetEventsByTrack(Guid trackId, int? take = null, int? skip = null);

        Event TryGetById(Guid id);
        Event TryUpdate(Event @event);
        Guid TryDelete(Guid eventId);
        Guid TryDeleteByTrack(Guid trackId);
        IEnumerable<Event> TryGetEventsByUser(Guid userId);
    }
}