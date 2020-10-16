using System;
using System.Collections.Generic;

namespace ItHappened.Domain.Repositories
{
    public interface ITrackRepository
    {
        Result<Track> TryCreate(Track track);
        Result<IEnumerable<Track>> TryGetTracksByUser(Guid userId);
        Result<Track> TryUpdate(Track track);
        Result<bool> TryDelete(Guid trackId);
    }
}