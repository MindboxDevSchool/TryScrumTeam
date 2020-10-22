using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure
{
    public class TrackRepositoryInMemory:ITrackRepository
    {
        private Dictionary<Guid, Track> _tracks = new Dictionary<Guid, Track>();
        
        public Result<Track> TryCreate(Track track)
        {
            _tracks[track.Id] = track;
            return new Result<Track>(track);
        }

        public Result<IEnumerable<Track>> TryGetTracksByUser(Guid userId)
        {
            var res = _tracks
                .Where(elem => elem.Value.CreatorId == userId)
                .Select(elem => elem.Value);
            return new Result<IEnumerable<Track>>(res);
        }

        public Track TryGetTrackById(Guid trackId)
        {
            if (!_tracks.ContainsKey(trackId))
            {
                throw new RepositoryException(RepositoryExceptionType.TrackNotFound, trackId);
            }
            return _tracks[trackId];
        }

        public Result<Track> TryUpdate(Track track)
        {
            if (!_tracks.ContainsKey(track.Id))
            {
                return new Result<Track>(new DataException());
            }

            _tracks[track.Id] = track;
            return new Result<Track>(track);
        }

        public Result<bool> TryDelete(Guid trackId)
        {
            _tracks.Remove(trackId);
            return new Result<bool>(true);
        }
    }
}