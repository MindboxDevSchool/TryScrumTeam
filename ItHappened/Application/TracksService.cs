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

        public Result<IEnumerable<TrackDto>> GetTracks(AuthData authData)
        {
            var isAuthValid = _userRepository.IsUserAuthDataValid(authData);
            if (!isAuthValid.IsSuccessful())
                return new Result<IEnumerable<TrackDto>>(new UserNotFoundException(authData.Id));
            if (isAuthValid.IsSuccessful() && isAuthValid.Value == false)
                return new Result<IEnumerable<TrackDto>>(new InvalidAuthDataException(authData));

            var userTracksWithResult = _trackRepository.TryGetTracksByUser(authData.Id);

            if (!userTracksWithResult.IsSuccessful())
                return new Result<IEnumerable<TrackDto>>(userTracksWithResult.Exception);

            return new Result<IEnumerable<TrackDto>>(userTracksWithResult.Value.Select(track => new TrackDto(track)));
        }

        public Result<TrackDto> CreateTrack(AuthData authData, string name, DateTime createdAt, IEnumerable<CustomizationType> allowedCustomizations)
        {
            var isAuthValid = _userRepository.IsUserAuthDataValid(authData);
            if (!isAuthValid.IsSuccessful())
                return new Result<TrackDto>(new UserNotFoundException(authData.Id));
            if (isAuthValid.IsSuccessful() && isAuthValid.Value == false)
                return new Result<TrackDto>(new InvalidAuthDataException(authData));

            var track = new Track(Guid.NewGuid(), name, createdAt, authData.Id, allowedCustomizations);
            var trackWithResult = _trackRepository.TryCreate(track);
            
            if (!trackWithResult.IsSuccessful())
                return new Result<TrackDto>(trackWithResult.Exception);
            
            return new Result<TrackDto>(new TrackDto(trackWithResult.Value));
        }

        public Result<TrackDto> EditTrack(AuthData authData, TrackDto trackDto)
        {
            var isAuthValid = _userRepository.IsUserAuthDataValid(authData);
            if (!isAuthValid.IsSuccessful())
                return new Result<TrackDto>(new UserNotFoundException(authData.Id));
            if (isAuthValid.IsSuccessful() && isAuthValid.Value == false)
                return new Result<TrackDto>(new InvalidAuthDataException(authData));

            if (authData.Id != trackDto.CreatorId)
                return new Result<TrackDto>(new TrackAccessDeniedException(authData.Id, trackDto.Id));

            var trackToEdit = _trackRepository.TryGetTrackById(trackDto.Id);

            if (trackToEdit.CreatedAt != trackDto.CreatedAt)
                return new Result<TrackDto>(new EditingImmutableDataException(nameof(trackDto.CreatedAt)));

            var track = new Track(trackDto.Id, trackDto.Name, trackDto.CreatedAt, authData.Id, trackDto.AllowedCustomizations);
            var trackWithResult = _trackRepository.TryUpdate(track);
            
            if (!trackWithResult.IsSuccessful())
                return new Result<TrackDto>(trackWithResult.Exception);
            
            return new Result<TrackDto>(new TrackDto(trackWithResult.Value));
        }

        public Result<bool> DeleteTrack(AuthData authData, Guid trackId)
        {
            var isAuthValid = _userRepository.IsUserAuthDataValid(authData);
            if (!isAuthValid.IsSuccessful())
                return new Result<bool>(new UserNotFoundException(authData.Id));
            if (isAuthValid.IsSuccessful() && isAuthValid.Value == false)
                return new Result<bool>(new InvalidAuthDataException(authData));

            var eventsDeletingResult = _eventRepository.TryDeleteByTrack(trackId);

            return _trackRepository.TryDelete(trackId);
        }
    }
}