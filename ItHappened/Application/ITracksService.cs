using System;
using System.Collections;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface ITracksService
    {
        Result<IEnumerable<TrackDto>> GetTracks(AuthData authData);
        Result<TrackDto> CreateTrack(AuthData authData, string name, DateTime createdAt, IEnumerable<CustomizationType> allowedCustomizations);
        Result<TrackDto> EditTrack(AuthData authData, TrackDto trackDto);
        Result<bool> DeleteTrack(AuthData authData, Guid trackId);
    }
}