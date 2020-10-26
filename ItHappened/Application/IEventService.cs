using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface IEventService
    {
        IEnumerable<EventDto> GetEvents(Guid userId, Guid trackId);
        EventDto CreateEvent(Guid userId, Guid trackId, DateTime createdAt, CustomizationsDto customizationsDto);
        EventDto EditEvent(Guid userId, EventToEditDto eventDto);
        Guid DeleteEvent(Guid userId, Guid eventId);
    }
}