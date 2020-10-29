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
    public class TrackRepositoryDapper: ITrackRepository
    {
        private readonly IDbConnection _connection;
        
        public TrackRepositoryDapper(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
        public Track TryCreate(Track track)
        {
            var allowedCustomizations =
                    string.Join(" ", track.AllowedCustomizations.Select(s => s.ToString()).ToArray());
                var newTrack = _connection
                    .Query<TrackDb>(@"insert into ItHappend.Tracks
                (Id, Name, CreatedAt,CreatorId,AllowedCustomizations) 
                VALUES (@Id, @Name, @CreatedAt,@CreatorId,@AllowedCustomizations)",
                        new
                        {
                            Id = track.Id,
                            Name = track.Name,
                            CreatedAt = track.CreatedAt,
                            CreatorId = track.CreatorId,
                            AllowedCustomizations = allowedCustomizations
                        });
                
            return track;
        }

        public IEnumerable<Track> TryGetTracksByUser(Guid userId)
        {
            var result = _connection
                .Query<TrackDb>(@"select * from ItHappend.Tracks
                                    where CreatorId = @CreatorId",
                    new{CreatorId = userId}).ToList();
            
            if (!result.Any())
                throw new RepositoryException(RepositoryExceptionType.TrackNotFound, userId);

            var resultConverted = result.Select(FromTrackDbToTrack);
            
            return resultConverted;
        }

        public Track TryGetTrackById(Guid trackId)
        {
            var result = _connection
                .Query<TrackDb>(@"select * from ItHappend.Tracks
                                    where Id = @TrackId",
                    new{TrackId = trackId}).ToList();
            
            if (!result.Any())
                throw new RepositoryException(RepositoryExceptionType.TrackNotFound, trackId);
            var resultConverted = result.Select(FromTrackDbToTrack);
            
            return resultConverted.Single();
        }

        public Track TryUpdate(Track track)
        {
            var allowedCustomizations =
                string.Join(" ", track.AllowedCustomizations.Select(s => s.ToString()).ToArray());
            var newTrack = _connection
                .Query<TrackDb>(@"UPDATE ItHappend.Tracks 
                SET
                Name = @Name,
                 CreatedAt = @CreatedAt,
                 CreatorId = @CreatorId,
                 AllowedCustomizations = @AllowedCustomizations
                 where Id = @Id",
            new
            {
                Id = track.Id,
                Name = track.Name,
                CreatedAt = track.CreatedAt,
                CreatorId = track.CreatorId,
                AllowedCustomizations = allowedCustomizations
            });
            return track;
        }

        public Guid TryDelete(Guid trackId)
        {
            var result = _connection.Query<TrackDb>(
                @"delete from ItHappend.Tracks
                      where Id = @Id",
                new {Id = trackId}
            );
            return trackId;
        }

        private Track FromTrackDbToTrack(TrackDb trackDb)
        {
            var EnumStrings = trackDb.AllowedCustomizations.Split().ToList();
            var Enums = EnumStrings.Select(Enum.Parse<CustomizationType>);
            var newTrack = new Track(trackDb.Id,trackDb.Name,trackDb.CreatedAt,trackDb.CreatorId, Enums);
            return newTrack;
        }
    }
}