using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface IEventService
    {
        Result<IEnumerable<EventDto>> GetEvents(AuthData authData, Guid trackId);
        Result<EventDto> CreateEvent(AuthData authData, Guid trackId, DateTime createdAt, Customizations customizations);
        Result<EventDto> EditEvent(AuthData authData, EventDto eventDto);
        Result<bool> DeleteEvent(AuthData authData, Guid eventId);
    }
}