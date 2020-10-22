﻿using System;
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

        public TrackDto EditTrack(Guid userId, TrackDto trackDto)
        {
            var trackToEdit = _trackRepository.TryGetTrackById(trackDto.Id);
            
            if (userId != trackToEdit.CreatorId)
                throw new DomainException(DomainExceptionType.TrackAccessDenied, userId, trackDto.Id);
            
            var track = new Track(trackDto.Id, trackDto.Name, trackDto.CreatedAt, userId, trackDto.AllowedCustomizations);
            var trackWithResult = _trackRepository.TryUpdate(track);

            return new TrackDto(trackWithResult);
        }

        public Guid DeleteTrack(Guid userId, Guid trackId)
        {
            var trackToEdit = _trackRepository.TryGetTrackById(trackId);
            
            if (userId != trackToEdit.CreatorId)
                throw new DomainException(DomainExceptionType.TrackAccessDenied, userId, trackId);
            
            _eventRepository.TryDeleteByTrack(trackId);

            var deletedTrackId = _trackRepository.TryDelete(trackId);

            return deletedTrackId;
        }
    }
}