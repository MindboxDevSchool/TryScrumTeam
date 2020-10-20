using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public class EventDto
    {
        public EventDto(Event @event)
        {
            Id = @event.Id;
            CreatedAt = @event.CreatedAt;
            TrackId = @event.TrackId;
            Customization = @event.Customization;
        }
        
        public EventDto(Guid id, DateTime createdAt, Guid trackId, Customs customisation)
        {
            Id = id;
            CreatedAt = createdAt;
            TrackId = trackId;
            Customization = customisation;
        }
        public readonly Guid Id;
        public readonly DateTime CreatedAt;
        public readonly Guid TrackId;
        public readonly Customs Customization;

    }
}