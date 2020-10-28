using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public class Track
    {
        public Track()
        {
        }
        public Track(Guid id, string name, DateTime createdAt, Guid creatorId, IEnumerable<CustomizationType> allowedCustomizations)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CreatedAt = createdAt;
            CreatorId = creatorId;
            AllowedCustomizations = allowedCustomizations ?? throw new ArgumentNullException(nameof(allowedCustomizations));
        }

        public Guid Id { get; }
        public string Name { get; }
        public DateTime CreatedAt { get; }
        public Guid CreatorId { get; }
        public IEnumerable<CustomizationType> AllowedCustomizations { get; }
    }
}