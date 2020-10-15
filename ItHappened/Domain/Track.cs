using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public class Track
    {
        public Track(Guid id, string name, DateTime createdAt, Guid creatorId, List<CustomType> allowedCustoms)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CreatedAt = createdAt;
            CreatorId = creatorId;
            AllowedCustoms = allowedCustoms ?? throw new ArgumentNullException(nameof(allowedCustoms));
        }

        public Guid Id { get; }
        public string Name { get; }
        public DateTime CreatedAt { get; }
        public Guid CreatorId { get; }
        public List<CustomType> AllowedCustoms { get; }
    }
}