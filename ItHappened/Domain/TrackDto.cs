using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public class TrackDto
    {
        public TrackDto(Track track)
        {
            Id = track.Id;
            Name = track.Name ?? throw new ArgumentNullException(nameof(track.Name));
            CreatedAt = track.CreatedAt;
            CreatorId = track.CreatorId;
            AllowedCustomizations = track.AllowedCustomizations ?? throw new ArgumentNullException(nameof(track.AllowedCustomizations));
        }
        public TrackDto(Guid id, string name, DateTime createdAt, Guid creatorId, IEnumerable<CustomizationType> allowedCustomizations)
        {
            Id = id;
            Name = name;
            CreatedAt = createdAt;
            CreatorId = creatorId;
            AllowedCustomizations = allowedCustomizations;
        }

        public readonly Guid Id;
        public readonly string Name;
        public readonly DateTime CreatedAt;
        public readonly Guid CreatorId;
        public readonly IEnumerable<CustomizationType> AllowedCustomizations;
    }
}