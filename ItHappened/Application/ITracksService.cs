using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface ITracksService
    {
        IEnumerable<TrackDto> GetTracks(AuthData authData);
        TrackDto CreateTrack(AuthData authData, string name, DateTime createdAt, IEnumerable<CustomizationType> allowedCustomizations);
        TrackDto EditTrack(AuthData authData, TrackDto trackDto);
        Guid DeleteTrack(AuthData authData, Guid trackId);
    }
}