using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface IEventService
    {
        Result<IEnumerable<EventDto>> GetEvents(Guid userId, Guid trackId);
        Result<EventDto> CreateEvent(Guid userId, Guid trackId, DateTime createdAt, Customizations customizations);
        Result<EventDto> EditEvent(Guid userId, EventDto eventDto);
        Result<bool> DeleteEvent(Guid userId, Guid eventId);
    }
}