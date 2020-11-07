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
    public class TrackRepositoryDapper : ITrackRepository
    {
        private readonly IDbConnection _connection;

        public TrackRepositoryDapper(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public Track TryCreate(Track track)
        {
            var trackDb = FromTrackToTrackDb(track);
            _connection
                .Query<TrackDb>(@"insert into ItHappend.Tracks
                (Id, Name, CreatedAt,CreatorId,AllowedCustomizations) 
                VALUES (@Id, @Name, @CreatedAt,@CreatorId,@AllowedCustomizations)",
                    trackDb);

            return track;
        }

        public IEnumerable<Track> TryGetTracksByUser(Guid userId, int? take = null, int? skip = null)
        {
            var takeString = take is null ? "" : $" FETCH NEXT {take} ROWS ONLY";
            var result = _connection
                .Query<TrackDb>(@"select * from ItHappend.Tracks
                                    where CreatorId = @CreatorId
                                    ORDER BY CreatedAt DESC
                                    OFFSET  @Skip ROWS"
                                + takeString,
                    new {CreatorId = userId, Skip = skip ?? 0}).ToList();

            var resultConverted = result.Select(FromTrackDbToTrack);

            return resultConverted;
        }

        public Track TryGetTrackById(Guid trackId)
        {
            var result = _connection
                .Query<TrackDb>(@"select * from ItHappend.Tracks
                                    where Id = @TrackId",
                    new {TrackId = trackId}).ToList();

            if (!result.Any())
                throw new RepositoryException(RepositoryExceptionType.TrackNotFound, trackId);
            var resultConverted = result.Select(FromTrackDbToTrack);

            return resultConverted.Single();
        }

        public Track TryUpdate(Track track)
        {
            var trackDb = FromTrackToTrackDb(track);
            _connection
                .Query(@"UPDATE ItHappend.Tracks 
                SET
                Name = @Name,
                 CreatedAt = @CreatedAt,
                 CreatorId = @CreatorId,
                 AllowedCustomizations = @AllowedCustomizations
                 where Id = @Id",
                    trackDb);
            return track;
        }

        public Guid TryDelete(Guid trackId)
        {
            _connection.Query(
                @"delete from ItHappend.Tracks
                      where Id = @Id",
                new {Id = trackId}
            );
            return trackId;
        }

        private Track FromTrackDbToTrack(TrackDb trackDb)
        {
            var enumStrings = 
                trackDb.AllowedCustomizations == ""
                ? new List<string>()
                : trackDb.AllowedCustomizations.Split().ToList();
            var enums = enumStrings.Select(Enum.Parse<CustomizationType>);
            var newTrack = new Track(trackDb.Id, trackDb.Name, trackDb.CreatedAt, trackDb.CreatorId, enums);
            return newTrack;
        }

        private TrackDb FromTrackToTrackDb(Track track)
        {
            var stringOfEnums = track.AllowedCustomizations.CreateString();
            var newTrackDb = new TrackDb(track.Id, track.Name, track.CreatedAt, track.CreatorId, stringOfEnums);
            return newTrackDb;
        }
    }
}