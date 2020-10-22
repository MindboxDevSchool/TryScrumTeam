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
        public TracksService(ITrackRepository trackRepository, 
            IEventRepository eventRepository, 
            IUserRepository userRepository)
        {
            _trackRepository = trackRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }

        private readonly ITrackRepository _trackRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;

        public IEnumerable<TrackDto> GetTracks(AuthData authData)
        {
            var userTracksWithResult = _trackRepository.TryGetTracksByUser(authData.Id);

            return userTracksWithResult.Select(track => new TrackDto(track));
        }

        public TrackDto CreateTrack(AuthData authData, string name, DateTime createdAt, IEnumerable<CustomizationType> allowedCustomizations)
        {
            var track = new Track(Guid.NewGuid(), name, createdAt, authData.Id, allowedCustomizations);
            var createdTrack = _trackRepository.TryCreate(track);

            return new TrackDto(createdTrack);
        }

        public TrackDto EditTrack(AuthData authData, TrackDto trackDto)
        {
            var trackToEdit = _trackRepository.TryGetTrackById(trackDto.Id);
            
            if (authData.Id != trackToEdit.CreatorId)
                throw new DomainException(DomainExceptionType.TrackAccessDenied, authData.Id, trackDto.Id);

            var track = new Track(trackDto.Id, trackDto.Name, trackDto.CreatedAt, authData.Id, trackDto.AllowedCustomizations);
            var trackWithResult = _trackRepository.TryUpdate(track);

            return new TrackDto(trackWithResult);
        }

        public Guid DeleteTrack(AuthData authData, Guid trackId)
        {
            var trackToEdit = _trackRepository.TryGetTrackById(trackId);
            
            if (authData.Id != trackToEdit.CreatorId)
                throw new DomainException(DomainExceptionType.TrackAccessDenied, authData.Id, trackId);
            
            _eventRepository.TryDeleteByTrack(trackId);
            
            var deletedTrackId = _trackRepository.TryDelete(trackId);

            return deletedTrackId;
        }
    }
}