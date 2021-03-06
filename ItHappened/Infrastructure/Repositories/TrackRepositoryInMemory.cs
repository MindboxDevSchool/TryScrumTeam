﻿using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure.Repositories
{
    public class TrackRepositoryInMemory:ITrackRepository
    {
        private Dictionary<Guid, Track> _tracks = new Dictionary<Guid, Track>();
        
        public Track TryCreate(Track track)
        {
            _tracks[track.Id] = track;
            return track;
        }

        public IEnumerable<Track> TryGetTracksByUser(Guid userId, int? take = null, int? skip = null)
        {
            var result = _tracks
                .Where(elem => elem.Value.CreatorId == userId)
                .Select(elem => elem.Value);
            if (skip != null) result = result.Skip((int) skip);
            if (take != null) result = result.Take((int) take);
            return result;
        }

        public Track TryGetTrackById(Guid trackId)
        {
            if (!_tracks.ContainsKey(trackId))
            {
                throw new RepositoryException(RepositoryExceptionType.TrackNotFound, trackId);
            }
            return _tracks[trackId];
        }

        public Track TryUpdate(Track track)
        {
            if (!_tracks.ContainsKey(track.Id))
            {
                throw new RepositoryException(RepositoryExceptionType.TrackNotFound, track.Id);
            }

            _tracks[track.Id] = track;
            return track;
        }

        public Guid TryDelete(Guid trackId)
        {
            if (!_tracks.ContainsKey(trackId))
            {
                throw new RepositoryException(RepositoryExceptionType.TrackNotFound, trackId);
            }
            
            _tracks.Remove(trackId);
            return trackId;
        }
    }
}