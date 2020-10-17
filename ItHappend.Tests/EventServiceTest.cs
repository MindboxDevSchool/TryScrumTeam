using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;
using ItHappened.Infrastructure;
using NUnit.Framework;

namespace ItHappend.Tests
{
    public class EventServiceTest
    {
        [SetUp]
        public void Setup()
        {
            _trackRepository = new TrackRepositoryInMemory();
            _eventRepository = new EventRepositoryInMemory();
            _authData = new AuthData(Guid.Parse("00000000000000000000000000000002"), "01");
            _authDataWrong = new AuthData(Guid.Parse("00000000000000000000000000000010"), "05");
            _track = new Track(
                Guid.Parse("00000000000000000000000000000003"),
                "00",
                DateTime.Now,
                Guid.Parse("00000000000000000000000000000002"),
                new List<CustomType>());
            _event = new Event(Guid.Parse("00000000000000000000000000000003"),
                DateTime.Now, 
                _track.Id,
                new List<Customs>());
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
            Assert.AreEqual(1, result.Value.Count());
            Assert.AreEqual(_track.Id, result.Value.First().TrackId);
        }
        
        [Test]
        public void GetEvents_AccessDeny()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);
            
            // act
            var result = eventService.GetEvents(_authDataWrong,_track.Id);

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
                eventService.CreateEvent(_authDataWrong, _track.Id, DateTime.Now, new List<Customs>());
            
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
                eventService.CreateEvent(_authData, _track.Id, DateTime.Now, new List<Customs>());
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
            var customs = new List<Customs>();
            customs.Append(new Customs());
            var newEvent = new Event(Guid.Parse("00000000000000000000000000000003"),
                _event.CreatedAt, 
                _track.Id,
                customs);
            var result = eventService.EditEvent(_authData, new EventDto(newEvent));
            var eventFromRepository =new EventDto(_eventRepository.TryGetById(result.Value.Id).Value);
            // assert
            Assert.IsTrue(result.IsSuccessful());
            Assert.AreEqual(result.Value.Id,eventFromRepository.Id);
            Assert.AreEqual(result.Value.CreatedAt,eventFromRepository.CreatedAt);
            Assert.AreEqual(result.Value.Customization.Count(),eventFromRepository.Customization.Count());

        }
        
        [Test]
        public void EditEvent_UnsuccessfulEventEditing_TryingToUpdateImmutableField()
        {
            // arrange
            var eventService = new EventService(_eventRepository,_trackRepository);
            
            // act
            var customs = new List<Customs>();
            customs.Append(new Customs());
            var newEvent = new Event(Guid.Parse("00000000000000000000000000000003"),
                DateTime.Now, 
                _track.Id,
                customs);
            var result = eventService.EditEvent(_authData, new EventDto(newEvent));
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
                eventService.DeleteEvent(_authData,_event.Id);
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
                eventService.DeleteEvent(_authDataWrong,_event.Id);
            var events = _eventRepository.TryGetEventsByTrack(_track.Id);
            
            // assert
            Assert.True(result.Exception is EventAccessDeniedException);
        }
    }
}