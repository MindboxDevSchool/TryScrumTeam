using System;

namespace ItHappened.Infrastructure.DbModels
{
    public class TrackDb
    {
        public TrackDb(Guid id, string name, DateTime createdAt, Guid creatorId, string allowedCustomizations)
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
        public string AllowedCustomizations { get; }
    }
}