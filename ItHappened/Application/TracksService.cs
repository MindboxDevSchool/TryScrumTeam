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

        public Result<IEnumerable<TrackDto>> GetTracks(Guid userId)
        {
            var userTracksWithResult = _trackRepository.TryGetTracksByUser(userId);

            if (!userTracksWithResult.IsSuccessful())
                return new Result<IEnumerable<TrackDto>>(userTracksWithResult.Exception);

            return new Result<IEnumerable<TrackDto>>(userTracksWithResult.Value.Select(track => new TrackDto(track)));
        }

        public Result<TrackDto> CreateTrack(Guid userId, string name, DateTime createdAt, IEnumerable<CustomizationType> allowedCustomizations)
        {
            var track = new Track(Guid.NewGuid(), name, createdAt, userId, allowedCustomizations);
            var trackWithResult = _trackRepository.TryCreate(track);
            
            if (!trackWithResult.IsSuccessful())
                return new Result<TrackDto>(trackWithResult.Exception);
            
            return new Result<TrackDto>(new TrackDto(trackWithResult.Value));
        }

        public Result<TrackDto> EditTrack(Guid userId, TrackDto trackDto)
        {
            if (userId != trackDto.CreatorId)
                return new Result<TrackDto>(new TrackAccessDeniedException(userId, trackDto.Id));

            var trackToEdit = _trackRepository.TryGetTrackById(trackDto.Id);
            
            if (!trackToEdit.IsSuccessful())
                return new Result<TrackDto>(trackToEdit.Exception);
            
            if (trackToEdit.Value.CreatedAt != trackDto.CreatedAt)
                return new Result<TrackDto>(new EditingImmutableDataException(nameof(trackDto.CreatedAt)));

            var track = new Track(trackDto.Id, trackDto.Name, trackDto.CreatedAt, userId, trackDto.AllowedCustomizations);
            var trackWithResult = _trackRepository.TryUpdate(track);
            
            if (!trackWithResult.IsSuccessful())
                return new Result<TrackDto>(trackWithResult.Exception);
            
            return new Result<TrackDto>(new TrackDto(trackWithResult.Value));
        }

        public Result<bool> DeleteTrack(Guid userId, Guid trackId)
        {
            var eventsDeletingResult = _eventRepository.TryDeleteByTrack(trackId);

            if (!eventsDeletingResult.IsSuccessful())
                return eventsDeletingResult;
            
            return _trackRepository.TryDelete(trackId);
        }
    }
}