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
            Customizationization = @event.Customizationization;
        }
        
        public EventDto(Guid id, DateTime createdAt, Guid trackId, Customizations Customization)
        {
            Id = id;
            CreatedAt = createdAt;
            TrackId = trackId;
            Customizationization = Customization;
        }
        public readonly Guid Id;
        public readonly DateTime CreatedAt;
        public readonly Guid TrackId;
        public readonly Customizations Customizationization;

    }
}