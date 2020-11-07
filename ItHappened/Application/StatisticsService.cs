using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;
using ItHappened.Domain.StatisticsFacts;
using ItHappened.Domain.StatisticsFacts.General;
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
            _trackStatisticsFacts = new List<IStatisticsFact>
            {
                new BestEventFact(settings),
                new WorstEventFact(settings)
            };
            _generalStatisticsFacts = new List<IStatisticsFact>()
            {
                new NEventsRecordedFact(settings)
            };
        }

        private readonly IEventRepository _eventRepository;
        private readonly ITrackRepository _trackRepository;
        private readonly List<IStatisticsFact> _trackStatisticsFacts;
        private readonly List<IStatisticsFact> _generalStatisticsFacts;

        public List<string> GetTrackStatistics(Guid userId, Guid trackId)
        {
            var track = TryGetAccessToTrack(userId, trackId);
            var trackEvents = _eventRepository.TryGetEventsByTrack(trackId);

            return ProcessFacts(trackEvents, _trackStatisticsFacts, track.Name);
        }

        public List<string> GetGeneralStatistics(Guid userId)
        {
            var events = _eventRepository.TryGetEventsByUser(userId);

            return ProcessFacts(events, _generalStatisticsFacts);
        }

        private List<string> ProcessFacts(
            IEnumerable<Event> events,
            IEnumerable<IStatisticsFact> facts,
            string trackName = "")
        {
            foreach (var fact in facts)
            {
                fact.Process(events, trackName);
            }
            
            return facts
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