using System;
using System.Collections.Generic;

namespace ItHappened.Domain.Repositories
{
    public interface ITrackRepository
    {
        Track TryCreate(Track track);
        IEnumerable<Track> TryGetTracksByUser(Guid userId, int? take = null, int? skip = null);
        Track TryGetTrackById(Guid trackId);
        Track TryUpdate(Track track);
        Guid TryDelete(Guid trackId);
    }
}