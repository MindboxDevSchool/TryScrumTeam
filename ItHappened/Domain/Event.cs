using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public class Event
    {
        public Event(Guid id, DateTime createdAt, Guid trackId, Customs customisation)
        {
            Id = id;
            CreatedAt = createdAt;
            TrackId = trackId;
            Customization = customisation ?? throw new ArgumentNullException(nameof(customisation));
        }

        public Guid Id { get; }
        public DateTime CreatedAt { get; }
        public Guid TrackId { get; }
        public Customs Customization { get; }
    }
}