using System;
using System.Collections.Generic;

namespace ItHappened.Domain.Repositories
{
    interface IEventRepository
    {
        Result<Event> TryCreate(Event @event);
        Result<List<Event>> TryGetEventsByTrack(Guid trackId);
        Result<Event> TryUpdate(Event @event);
        bool TryDelete(Guid eventId);
    }
}