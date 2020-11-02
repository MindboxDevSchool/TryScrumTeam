using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface ITracksService
    {
        IEnumerable<TrackDto> GetTracks(Guid userId, int? take = null, int? skip = null);
        TrackDto CreateTrack(Guid userId, string name, DateTime createdAt, IEnumerable<CustomizationType> allowedCustomizations);
        TrackDto EditTrack(Guid userId, TrackToEditDto trackDto);
        Guid DeleteTrack(Guid userId, Guid trackId);
    }
}