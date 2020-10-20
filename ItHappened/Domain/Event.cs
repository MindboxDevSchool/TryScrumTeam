using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public class Event
    {
        public Event(Guid id, DateTime createdAt, Guid trackId, Customizations Customization)
        {
            Id = id;
            CreatedAt = createdAt;
            TrackId = trackId;
            Customizationization = Customization ?? throw new ArgumentNullException(nameof(Customization));
        }

        public Guid Id { get; }
        public DateTime CreatedAt { get; }
        public Guid TrackId { get; }
        public Customizations Customizationization { get; }
    }
}