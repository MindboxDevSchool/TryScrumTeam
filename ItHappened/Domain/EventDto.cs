using System;

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
        
        public EventDto(Guid id, DateTime createdAt, Guid trackId, Customizations customization)
        {
            Id = id;
            CreatedAt = createdAt;
            TrackId = trackId;
            Customization = customization;
        }
        public readonly Guid Id;
        public readonly DateTime CreatedAt;
        public readonly Guid TrackId;
        public readonly Customizations Customization;

    }
}