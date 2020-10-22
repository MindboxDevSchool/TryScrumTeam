using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;
using NUnit.Framework;

namespace ItHappend.Tests
{
    public class EventServiceTest
    {
        [SetUp]
        public void Setup()
        {
            _trackRepository = new TrackRepositoryMock();
            _eventRepository = new EventRepositoryMock();
            _userId = Guid.Parse("00000000000000000000000000000002");
            _userInvalidId = Guid.Parse("00000000000000000000000000000010");
            _track = new Track(
                Guid.Parse("00000000000000000000000000000003"),
                "00",
                DateTime.Now,
                Guid.Parse("00000000000000000000000000000002"),
                new List<CustomizationType>());
            _event = new Event(Guid.Parse("00000000000000000000000000000003"),
                DateTime.Now, 
                _track.Id,
                new Customizations());
            _trackRepository.TryCreate(_track);
            _eventRepository.TryCreate(_event);

        }
        
        private ITrackRepository _trackRepository;
        private IEventRepository _eventRepository;
        private Guid _userId;
        private Guid _userInvalidId;
        private Track _track;
        private Event _event;

        [Test]
        public void GetEvents_SuccessfulEventsReceiving()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);
            
            // act
            var result = eventService.GetEvents(_userId,_track.Id);

            // assert
            Assert.AreEqual(1, result.Value.Count());
            Assert.AreEqual(_track.Id, result.Value.First().TrackId);
        }
        
        [Test]
        public void GetEvents_AccessDeny()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);
            
            // act
            var result = eventService.GetEvents(_userInvalidId,_track.Id);

            // assert
            Assert.True(result.Exception is TrackAccessDeniedException);
            
        }

        [Test]
        public void CreateEvent_AccessDeny()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);

            // act
            var result =
                eventService.CreateEvent(_userInvalidId, _track.Id, DateTime.Now, new Customizations());
            
            // assert
            Assert.True(result.Exception is TrackAccessDeniedException);
        }
        
        [Test]
        public void CreateEvent_SuccessfulEventCreation()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);

            // act
            var result =
                eventService.CreateEvent(_userId, _track.Id, DateTime.Now, new Customizations());
            var eventFromRepository =new EventDto(_eventRepository.TryGetById(result.Value.Id).Value);
            
            // assert
            Assert.IsTrue(result.IsSuccessful());
            Assert.AreEqual(result.Value.Id,eventFromRepository.Id);
            Assert.AreEqual(result.Value.CreatedAt,eventFromRepository.CreatedAt);
        }

        [Test]
        public void EditEvent_SuccessfulEventEditing()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);
            
            // act
            var customizations = new Customizations();
            
            var newEvent = new Event(Guid.Parse("00000000000000000000000000000003"),
                _event.CreatedAt, 
                _track.Id,
                customizations);
            var result = eventService.EditEvent(_userId, new EventDto(newEvent));
            var eventFromRepository =new EventDto(_eventRepository.TryGetById(result.Value.Id).Value);
            // assert
            Assert.IsTrue(result.IsSuccessful());
            Assert.AreEqual(result.Value.Id,eventFromRepository.Id);
            Assert.AreEqual(result.Value.CreatedAt,eventFromRepository.CreatedAt);
            Assert.AreEqual(result.Value.Customization,eventFromRepository.Customization);

        }
        
        [Test]
        public void EditEvent_UnsuccessfulEventEditing_TryingToUpdateImmutableField()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);
            
            // act
            var customizations = new Customizations();
            
            var newEvent = new Event(Guid.Parse("00000000000000000000000000000003"),
                DateTime.Now, 
                _track.Id,
                customizations);
            var result = eventService.EditEvent(_userId, new EventDto(newEvent));
            // assert
            Assert.IsFalse(result.IsSuccessful());
            Assert.IsTrue(result.Exception is EditingImmutableDataException);
        }

        [Test]
        public void DeleteEvent_SuccessfulTrackAndEventsDeletion()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);

            // act
            var result =
                eventService.DeleteEvent(_userId,_event.Id);
            var events = _eventRepository.TryGetEventsByTrack(_track.Id);
            
            // assert
            Assert.IsTrue(result.IsSuccessful());
            Assert.IsEmpty(events.Value);
        }
        
        [Test]
        public void DeleteEvent_AccessDeny()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);

            // act
            var result =
                eventService.DeleteEvent(_userInvalidId,_event.Id);
            var events = _eventRepository.TryGetEventsByTrack(_track.Id);
            
            // assert
            Assert.True(result.Exception is EventAccessDeniedException);
        }
    }
    
    public class EventRepositoryMock :IEventRepository
    {
        private Dictionary<Guid, Event> _events = new Dictionary<Guid, Event>();
        
        public Result<Event> TryCreate(Event @event)
        {
            _events[@event.Id] = @event;
            return new Result<Event>(@event);
        }

        public Result<IEnumerable<Event>> TryGetEventsByTrack(Guid trackId)
        {
            var result = _events
                .Where(elem => elem.Value.TrackId == trackId)
                .Select(elem => elem.Value);
            return new Result<IEnumerable<Event>>(result);
        }

        public Result<Event> TryGetById(Guid id)
        {
            return new Result<Event>(_events[id]);
        }

        public Result<Event> TryUpdate(Event @event)
        {
            _events[@event.Id] = @event;
            return new Result<Event>(@event);
        }

        public Result<bool> TryDelete(Guid eventId)
        {
            _events.Remove(eventId);
            return new Result<bool>(true);
        }

        public Result<bool> TryDeleteByTrack(Guid trackId)
        {
            var eventsToDelete = _events.Where(elem => elem.Value.TrackId == trackId);
            foreach (var element in eventsToDelete)
            {
                _events.Remove(element.Key);
            }
            return new Result<bool>(true);
        }
    }
    
    public class TrackRepositoryMock:ITrackRepository
    {
        private Dictionary<Guid, Track> _tracks = new Dictionary<Guid, Track>();
        
        public Result<Track> TryCreate(Track track)
        {
            _tracks[track.Id] = track;
            return new Result<Track>(track);
        }

        public Result<IEnumerable<Track>> TryGetTracksByUser(Guid userId)
        {
            var res = _tracks
                .Where(elem => elem.Value.CreatorId == userId)
                .Select(elem => elem.Value);
            return new Result<IEnumerable<Track>>(res);
        }

        public Result<Track> TryGetTrackById(Guid trackId)
        {
            return new Result<Track>(_tracks[trackId]);
        }

        public Result<Track> TryUpdate(Track track)
        {
            _tracks[track.Id] = track;
            return new Result<Track>(track);
        }

        public Result<bool> TryDelete(Guid trackId)
        {
            _tracks.Remove(trackId);
            return new Result<bool>(true);
        }
    }

}