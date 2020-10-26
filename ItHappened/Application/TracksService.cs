using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public class TracksService: ITracksService
    {
        public TracksService(ITrackRepository trackRepository, IEventRepository eventRepository)
        {
            _trackRepository = trackRepository;
            _eventRepository = eventRepository;
        }

        private readonly ITrackRepository _trackRepository;
        private readonly IEventRepository _eventRepository;

        public IEnumerable<TrackDto> GetTracks(Guid userId)
        {
            var userTracksWithResult = _trackRepository.TryGetTracksByUser(userId);

            return userTracksWithResult.Select(track => new TrackDto(track));
        }

        public TrackDto CreateTrack(Guid userId, string name, DateTime createdAt, IEnumerable<CustomizationType> allowedCustomizations)
        {
            var track = new Track(Guid.NewGuid(), name, createdAt, userId, allowedCustomizations);
            var createdTrack = _trackRepository.TryCreate(track);

            return new TrackDto(createdTrack);
        }

        public TrackDto EditTrack(Guid userId, TrackToEditDto trackDto)
        {
            var trackToEdit = TryGetAccessToTrack(userId, trackDto.Id);
            
            var track = new Track(trackDto.Id, trackDto.Name, trackToEdit.CreatedAt, userId, trackDto.AllowedCustomizations);
            var trackWithResult = _trackRepository.TryUpdate(track);

            return new TrackDto(trackWithResult);
        }

        public Guid DeleteTrack(Guid userId, Guid trackId)
        {
            var trackToDelete = TryGetAccessToTrack(userId, trackId);
            
            _eventRepository.TryDeleteByTrack(trackId);

            var deletedTrackId = _trackRepository.TryDelete(trackId);

            return deletedTrackId;
        }
        
        private Track TryGetAccessToTrack(Guid userId, Guid trackId)
        {
            var track = _trackRepository.TryGetTrackById(trackId);

            if (track.CreatorId != userId)
            {
                throw new DomainException(DomainExceptionType.TrackAccessDenied, userId, trackId);
            }

            return track;
        }
    }
}