using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure.Repositories
{
    public class TrackRepositoryEfDataBase : ITrackRepository
    {
        private TrackDbContext _trackDbContext;
        
        public TrackRepositoryEfDataBase(TrackDbContext trackDbContext)
        {
            _trackDbContext = trackDbContext;
        }

        public Track TryCreate(Track track)
        {
            _trackDbContext.Tracks.Add(track);
            return track;
        }

        public IEnumerable<Track> TryGetTracksByUser(Guid userId)
        {
            return 
                _trackDbContext
                    .Tracks
                    .Where(track => track.CreatorId == userId);
        }

        public Track TryGetTrackById(Guid trackId)
        {
            return
                _trackDbContext
                    .Tracks
                    .Single(track => track.Id == trackId);
        }

        public Track TryUpdate(Track track)
        {
            return track;
        }

        public Guid TryDelete(Guid trackId)
        {
            throw new NotImplementedException();
        }
    }
}