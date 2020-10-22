using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface IEventService
    {
        IEnumerable<EventDto> GetEvents(AuthData authData, Guid trackId);
        EventDto CreateEvent(AuthData authData, Guid trackId, DateTime createdAt, Customizations customizations);
        EventDto EditEvent(AuthData authData, EventDto eventDto);
        Guid DeleteEvent(AuthData authData, Guid eventId);
    }
}