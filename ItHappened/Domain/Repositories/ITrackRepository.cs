using System;
using System.Collections.Generic;

namespace ItHappened.Domain.Repositories
{
    interface ITrackRepository
    {
        Result<Track> TryCreate(Track track);
        Result<List<Track>> TryGetTracksByUser(Guid userId);
        Result<Track> TryUpdate(Track track);
        bool TryDelete(Guid trackId);
    }
}