using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public class TrackDto
    {
        public TrackDto(Guid id, string name, DateTime createdAt, Guid creatorId, List<CustomType> allowedCustoms)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CreatedAt = createdAt;
            CreatorId = creatorId;
            AllowedCustoms = allowedCustoms ?? throw new ArgumentNullException(nameof(allowedCustoms));
        }

        public readonly Guid Id;
        public readonly string Name;
        public readonly DateTime CreatedAt;
        public readonly Guid CreatorId;
        public readonly List<CustomType> AllowedCustoms;
    }
}