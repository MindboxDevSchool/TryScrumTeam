using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
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

        public Result<List<Track>> TryGetTracksByUser(Guid userId)
        {
            var res = _tracks
                .Where(elem => elem.Value.CreatorId == userId)
                .Select(elem=> elem.Value)
                .ToList();
            return new Result<List<Track>>(res);
        }

        public Result<Track> TryUpdate(Track track)
        {
            _tracks[track.Id] = track;
            return new Result<Track>(track);
        }

        public bool TryDelete(Guid trackId)
        {
            _tracks.Remove(trackId);
            return true;
        }
    }
}