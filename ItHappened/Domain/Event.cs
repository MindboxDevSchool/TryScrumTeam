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
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid TrackId { get; set; }
        public Customizations Customization { get; set; }
    }
}