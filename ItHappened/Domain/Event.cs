using System;
using System.ComponentModel.DataAnnotations;

namespace ItHappened.Domain
{
    public class Event
    {
        public Event()
        {
        }

        public Event(Guid id, DateTime createdAt, Guid trackId, Customizations customization)
        {
            Id = id;
            CreatedAt = createdAt;
            TrackId = trackId;
            Customization = customization ?? throw new ArgumentNullException(nameof(customization));
        }

        [Key]
        public Guid Id { get; }
        public DateTime CreatedAt { get; }
        public Guid TrackId { get; }
        public Customizations Customization { get; }
    }
}