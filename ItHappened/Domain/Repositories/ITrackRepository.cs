using System;
using System.Collections.Generic;

namespace ItHappened.Domain.Repositories
{
    public interface ITrackRepository
    {
        Track TryCreate(Track track);
        IEnumerable<Track> TryGetTracksByUser(Guid userId);
        Track TryGetTrackById(Guid trackId);
        Track TryUpdate(Track track);
        Guid TryDelete(Guid trackId);
    }
}