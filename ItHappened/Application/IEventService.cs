using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface IEventService
    {
        Result<IEnumerable<EventDto>> GetEvents(UserDto userDto, Guid trackId);
        Result<EventDto> CreateEvent(UserDto userDto, Guid trackId, DateTime createdAt, Customizations customizations);
        Result<EventDto> EditEvent(UserDto userDto, EventDto eventDto);
        Result<bool> DeleteEvent(UserDto userDto, Guid eventId);
    }
}