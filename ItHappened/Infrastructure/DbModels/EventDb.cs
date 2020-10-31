using System;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.DbModels
{
    public class EventDb
    {
        public EventDb(
            Guid id, DateTime createdAt, Guid trackId,
            double? scale, int? rating, string photoUrl, double? geotagLatitude, double? geotagLongitude, string comment)
        {
            Id = id;
            CreatedAt = createdAt;
            TrackId = trackId;
            Comment = comment;
            GeotagLatitude = geotagLatitude;
            GeotagLongitude = geotagLongitude;
            PhotoUrl = photoUrl;
            Rating = rating;
            Scale = scale;
        }
        
        public Guid Id { get; }
        public DateTime CreatedAt { get; }
        public Guid TrackId { get; }

        public string Comment { get; }
        public double? GeotagLatitude { get; }
        public double? GeotagLongitude { get; }
        public string PhotoUrl { get; }
        public int? Rating { get; }
        public double? Scale { get; }
    }
}