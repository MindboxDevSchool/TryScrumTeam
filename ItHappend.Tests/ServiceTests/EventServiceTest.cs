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
            _authData = new AuthData(Guid.Parse("00000000000000000000000000000002"), "01");
            _authDataWrong = new AuthData(Guid.Parse("00000000000000000000000000000010"), "05");
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
        private AuthData _authData;
        private AuthData _authDataWrong;
        private Track _track;
        private Event _event;

        [Test]
        public void GetEvents_SuccessfulEventsReceiving()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);
            
            // act
            var result = eventService.GetEvents(_authData,_track.Id);

            // assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(_track.Id, result.First().TrackId);
        }
        
        [Test]
        public void GetEvents_AccessDeny()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);
            DomainException exception = null;
            
            // act
            try
            {
                var result = eventService.GetEvents(_authDataWrong, _track.Id);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            // assert
            //Assert.True(result.Exception is TrackAccessDeniedException);
            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }

        [Test]
        public void CreateEvent_AccessDeny()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);
            DomainException exception = null;

            // act
            try
            {
                var result =
                    eventService.CreateEvent(_authDataWrong, _track.Id, DateTime.Now, new Customizations());
            }
            catch (DomainException e)
            {
                exception = e;
            }

            // assert
            //Assert.True(result.Exception is TrackAccessDeniedException);
            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }
        
        [Test]
        public void CreateEvent_SuccessfulEventCreation()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);

            // act
            var result =
                eventService.CreateEvent(_authData, _track.Id, DateTime.Now, new Customizations());
            var eventFromRepository =new EventDto(_eventRepository.TryGetById(result.Id));
            
            // assert
            Assert.AreEqual(result.Id,eventFromRepository.Id);
            Assert.AreEqual(result.CreatedAt,eventFromRepository.CreatedAt);
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
            var result = eventService.EditEvent(_authData, new EventDto(newEvent));
            var eventFromRepository =new EventDto(_eventRepository.TryGetById(result.Id));
            // assert
            Assert.AreEqual(result.Id,eventFromRepository.Id);
            Assert.AreEqual(result.CreatedAt,eventFromRepository.CreatedAt);
            Assert.AreEqual(result.Customization,eventFromRepository.Customization);

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
            var result = eventService.EditEvent(_authData, new EventDto(newEvent));
            // assert
            //Assert.IsTrue(result.Exception is EditingImmutableDataException);
        }

        [Test]
        public void DeleteEvent_SuccessfulTrackAndEventsDeletion()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);

            // act
            var result =
                eventService.DeleteEvent(_authData,_event.Id);
            var events = _eventRepository.TryGetEventsByTrack(_track.Id);
            
            // assert
            Assert.IsEmpty(events);
        }
        
        [Test]
        public void DeleteEvent_AccessDeny()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);
            DomainException exception = null;
            
            // act
            try
            {
                var result =
                    eventService.DeleteEvent(_authDataWrong, _event.Id);
                var events = _eventRepository.TryGetEventsByTrack(_track.Id);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            // assert
            //Assert.True(result.Exception is EventAccessDeniedException);
            Assert.AreEqual(DomainExceptionType.EventAccessDenied, exception.Type);
        }
    }
    
    public class EventRepositoryMock :IEventRepository
    {
        private Dictionary<Guid, Event> _events = new Dictionary<Guid, Event>();
        
        public Event TryCreate(Event @event)
        {
            _events[@event.Id] = @event;
            return @event;
        }

        public IEnumerable<Event> TryGetEventsByTrack(Guid trackId)
        {
            var result = _events
                .Where(elem => elem.Value.TrackId == trackId)
                .Select(elem => elem.Value);
            return result;
        }

        public Event TryGetById(Guid id)
        {
            return _events[id];
        }

        public Event TryUpdate(Event @event)
        {
            _events[@event.Id] = @event;
            return @event;
        }

        public Guid TryDelete(Guid eventId)
        {
            _events.Remove(eventId);
            return eventId;
        }

        public Guid TryDeleteByTrack(Guid trackId)
        {
            var eventsToDelete = _events.Where(elem => elem.Value.TrackId == trackId);
            foreach (var element in eventsToDelete)
            {
                _events.Remove(element.Key);
            }
            return trackId;
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

        public Track TryGetTrackById(Guid trackId)
        {
            return _tracks[trackId];
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