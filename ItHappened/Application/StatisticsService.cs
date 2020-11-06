using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;
using ItHappened.Domain.StatisticsFacts;
using ItHappened.Domain.StatisticsFacts.SpecificToTracks;

namespace ItHappened.Application
{
    public class StatisticsService : IStatisticsService
    {
        public StatisticsService(IEventRepository eventRepository, 
            ITrackRepository trackRepository, ItHappenedSettings settings)
        {
            _eventRepository = eventRepository;
            _trackRepository = trackRepository;
            _settings = settings;
            _statisticsFacts = new List<IStatisticsFact>
            {
                new BestEventFact(settings),
                new WorstEventFact(settings)
            };
        }

        private readonly IEventRepository _eventRepository;
        private readonly ITrackRepository _trackRepository;
        private readonly ItHappenedSettings _settings;
        private readonly List<IStatisticsFact> _statisticsFacts;

        public List<string> GetTrackStatistics(Guid userId, Guid trackId)
        {
            var track = TryGetAccessToTrack(userId, trackId);
            var trackEvents = _eventRepository.TryGetEventsByTrack(trackId);

            foreach (var fact in _statisticsFacts)
            {
                fact.Process(trackEvents, track.Name);
            }

            return _statisticsFacts
                .Where(fact => fact.IsApplicable)
                .OrderByDescending(fact => fact.Priority)
                .Select(fact => fact.Description)
                .ToList();
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