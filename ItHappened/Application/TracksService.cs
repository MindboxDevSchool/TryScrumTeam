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

        public Result<IEnumerable<TrackDto>> GetTracks(AuthData authData)
        {
            var userTracksWithResult = _trackRepository.TryGetTracksByUser(authData.Id);

            if (!userTracksWithResult.IsSuccessful())
                return new Result<IEnumerable<TrackDto>>(userTracksWithResult.Exception);

            return new Result<IEnumerable<TrackDto>>(userTracksWithResult.Value.Select(track => new TrackDto(track)));
        }

        public Result<TrackDto> CreateTrack(AuthData authData, string name, DateTime createdAt, IEnumerable<CustomType> allowedCustoms)
        {
            var track = new Track(new Guid(), name, createdAt, authData.Id, allowedCustoms);
            var trackWithResult = _trackRepository.TryCreate(track);
            
            if (!trackWithResult.IsSuccessful())
                return new Result<TrackDto>(trackWithResult.Exception);
            
            return new Result<TrackDto>(new TrackDto(trackWithResult.Value));
        }

        public Result<TrackDto> EditTrack(AuthData authData, TrackDto trackDto)
        {
            if (authData.Id != trackDto.CreatorId)
            {
                return new Result<TrackDto>(new TrackAccessDeniedException(authData.Id, trackDto.Id));
            }

            var track = new Track(trackDto.Id, trackDto.Name, trackDto.CreatedAt, authData.Id, trackDto.AllowedCustoms);
            var trackWithResult = _trackRepository.TryUpdate(track);
            
            if (!trackWithResult.IsSuccessful())
                return new Result<TrackDto>(trackWithResult.Exception);
            
            return new Result<TrackDto>(new TrackDto(trackWithResult.Value));
        }

        public Result<bool> DeleteTrack(AuthData authData, Guid trackId)
        {
            var eventsDeletingResult = _eventRepository.TryDeleteByTrack(trackId);

            if (!eventsDeletingResult.IsSuccessful())
                return eventsDeletingResult;
            
            return _trackRepository.TryDelete(trackId);
        }

        private readonly ITrackRepository _trackRepository;
        private readonly IEventRepository _eventRepository;
    }
}