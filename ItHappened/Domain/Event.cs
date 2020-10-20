using System;

namespace ItHappened.Domain
{
    public class Event
    {
        public Event(Guid id, DateTime createdAt, Guid trackId, Customizations customization)
        {
            Id = id;
            CreatedAt = createdAt;
            TrackId = trackId;
            customization = customization ?? throw new ArgumentNullException(nameof(customization));
        }

        public Guid Id { get; }
        public DateTime CreatedAt { get; }
        public Guid TrackId { get; }
        public Customizations Customization { get; }
    }
}