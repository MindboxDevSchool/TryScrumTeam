using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;
using ItHappened.Infrastructure.DbModels;

namespace ItHappened.Infrastructure.Repositories
{
    public class EventRepositoryDapper : IEventRepository
    {
        private readonly IDbConnection _connection;

        public EventRepositoryDapper(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public Event TryCreate(Event @event)
        {
            var eventDb = FromEventToEventDb(@event);
            _connection
                .Query<EventDb>(@"insert into ItHappend.Events
                (Id, CreatedAt, TrackId, Scale, Rating, PhotoUrl, GeotagLatitude, GeotagLongitude, Comment) 
                VALUES (@Id, @CreatedAt, @TrackId, @Scale, @Rating, @PhotoUrl, @GeotagLatitude, @GeotagLongitude, @Comment)",
                    eventDb);

            return @event;
        }

        public IEnumerable<Event> TryGetEventsByTrack(Guid trackId, int? take = null, int? skip = null)
        {
            var takeString = take is null ? "" : $" FETCH NEXT {take} ROWS ONLY";
            var result = _connection
                .Query<EventDb>(@"select * from ItHappend.Events
                                    where TrackId = @trackId
                                    ORDER BY CreatedAt DESC
                                    OFFSET  @Skip ROWS
                                    " + takeString,
                    new
                    {
                        TrackId = trackId,
                        Skip = skip ?? 0,
                    }).ToList();

            if (!result.Any())
                return new List<Event>();
            var resultConverted = result.Select(FromEventDbToEvent);

            return resultConverted.ToList();
        }

        public Event TryGetById(Guid id)
        {
            var result = _connection
                .Query<EventDb>(@"select * from ItHappend.Events
                                    where Id = @Id",
                    new {Id = id}).ToList();

            if (!result.Any())
                throw new RepositoryException(RepositoryExceptionType.EventNotFound, id);
            var resultConverted = result.Select(FromEventDbToEvent);

            return resultConverted.Single();
        }

        public Event TryUpdate(Event @event)
        {
            var eventDb = FromEventToEventDb(@event);
            _connection
                .Query(@"UPDATE ItHappend.Events 
                SET
                 CreatedAt = @CreatedAt,
                 TrackId = @TrackId,
                 Scale = @Scale,
                 Rating = @Rating,
                 PhotoUrl = @PhotoUrl,
                 GeotagLatitude = @GeotagLatitude,
                 GeotagLongitude = @GeotagLongitude,
                 Comment = @Comment
                 where Id = @Id",
                    eventDb);
            return @event;
        }

        public Guid TryDelete(Guid eventId)
        {
            _connection.Query(
                @"delete from ItHappend.Events
                      where Id = @Id",
                new {Id = eventId}
            );
            return eventId;
        }

        public Guid TryDeleteByTrack(Guid trackId)
        {
            return trackId;
        }

        public IEnumerable<Event> TryGetEventsByUser(Guid userId)
        {
            var result = _connection
                .Query<EventDb>(@"select * from ItHappend.Events
                                    where TrackId in
                                    (select Id from ItHappend.Tracks
                                    where CreatorId = @UserId)",
                    new {UserId = userId}).ToList();

            if (!result.Any())
                return new List<Event>();
            var resultConverted = result.Select(FromEventDbToEvent);

            return resultConverted.ToList();
        }

        private Event FromEventDbToEvent(EventDb eventDb)
        {
            var customizations = new Customizations(
                eventDb.Comment, eventDb.GeotagLatitude, eventDb.GeotagLongitude, eventDb.PhotoUrl, eventDb.Rating,
                eventDb.Scale);
            var @event = new Event(eventDb.Id, eventDb.CreatedAt, eventDb.TrackId, customizations);
            return @event;
        }

        private EventDb FromEventToEventDb(Event @event)
        {
            var newEventDb = new EventDb(
                @event.Id,
                @event.CreatedAt,
                @event.TrackId,
                @event.Customization.Scale?.Value,
                @event.Customization.Rating?.Value,
                @event.Customization.Photo?.Value,
                @event.Customization.Geotag?.Latitude,
                @event.Customization.Geotag?.Longitude,
                @event.Customization.Comment?.Value);
            return newEventDb;
        }
    }
}