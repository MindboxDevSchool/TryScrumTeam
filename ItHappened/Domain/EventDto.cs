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
        public readonly Guid Id;
        public readonly DateTime CreatedAt;
        public readonly Guid TrackId;
        public readonly List<Customs> Customization;

    }
}