using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface ITracksService
    {
        Result<IEnumerable<TrackDto>> GetTracks(Guid userId);
        Result<TrackDto> CreateTrack(Guid userId, string name, DateTime createdAt, IEnumerable<CustomizationType> allowedCustomizations);
        Result<TrackDto> EditTrack(Guid userId, TrackDto trackDto);
        Result<bool> DeleteTrack(Guid userId, Guid trackId);
    }
}