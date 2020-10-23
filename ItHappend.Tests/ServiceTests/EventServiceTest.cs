using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace ItHappend.Tests.ServiceTests
{
    public class EventServiceTest
    {
        private void SetupEntities()
        {
            _testTrackId = Guid.NewGuid();
            _testEventId = Guid.NewGuid();
            _testUserId = Guid.NewGuid();
            _testInvalidUserId = Guid.NewGuid();
            _testTrack = new Track(
                _testTrackId,
                "Test track",
                DateTime.Now,
                _testUserId,
                new List<CustomizationType>());
            _testEvent = new Event(
                _testEventId,
                DateTime.Today,
                _testTrackId,
                new Customizations());
            _testEventToUpdate = new Event(
                _testEventId,
                DateTime.Today,
                _testTrackId,
                new Customizations());
        }

        private void SetupMoqEventRepository()
        {
            var mock = new Mock<IEventRepository>();
            mock.Setup(method => method.TryGetEventsByTrack(It.IsAny<Guid>()))
                .Returns(new List<Event>() {_testEvent});
            mock.Setup(method => method.TryCreate(It.IsAny<Event>()))
                .Returns(_testEvent);
            mock.Setup(method => method.TryGetById(It.IsAny<Guid>()))
                .Returns(_testEvent);
            mock.Setup(method => method.TryUpdate(It.IsAny<Event>()))
                .Returns(_testEventToUpdate);
            mock.Setup(method => method.TryDelete(It.IsAny<Guid>()))
                .Returns(_testEventId);

            _eventRepository = mock.Object;
        }

        private void SetupMoqTrackRepository()
        {
            var mock = new Mock<ITrackRepository>();
            mock.Setup(method => method.TryGetTrackById(It.IsAny<Guid>()))
                .Returns(_testTrack);

            _trackRepository = mock.Object;
        }

        private Guid _testUserId;
        private Guid _testInvalidUserId;
        private Guid _testTrackId;
        private Guid _testEventId;
        private Track _testTrack;
        private Event _testEvent;
        private Event _testEventToUpdate;
        private IEventRepository _eventRepository;
        private ITrackRepository _trackRepository;

        [SetUp]
        public void Setup()
        {
            SetupEntities();
            SetupMoqEventRepository();
            SetupMoqTrackRepository();
        }

        [Test]
        public void GetEvents_SuccessfulEventsReceiving()
        {
            // arrange
            var eventService = new EventService(_eventRepository, _trackRepository);

            // act
            var result = eventService.GetEvents(_testUserId, _testTrackId);

            // assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(_testTrackId, result.First().TrackId);
        }

        [Test]
        public void GetEvents_AccessDeny()
        {
            // arrange
            var eventService = new EventService(_eventRepository, _trackRepository);
            DomainException exception = null;

            // act
            try
            {
                var result = eventService.GetEvents(_testInvalidUserId, _testTrackId);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            // assert
            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }

        [Test]
        public void CreateEvent_AccessDeny()
        {
            // arrange
            var eventService = new EventService(_eventRepository, _trackRepository);
            DomainException exception = null;

            // act
            try
            {
                var result =
                    eventService.CreateEvent(_testInvalidUserId, _testTrackId, DateTime.Now, new CustomizationsDto());
            }
            catch (DomainException e)
            {
                exception = e;
            }

            // assert
            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }

        [Test]
        public void CreateEvent_SuccessfulEventCreation()
        {
            // arrange
            var eventService = new EventService(_eventRepository, _trackRepository);

            // act
            var result =
                eventService.CreateEvent(_testUserId, _testTrackId, DateTime.Now, new CustomizationsDto());

            // assert
            Assert.AreEqual(_testEventId, result.Id);
        }

        [Test]
        public void EditEvent_SuccessfulEventEditing()
        {
            // arrange
            var eventService = new EventService(_eventRepository, _trackRepository);

            // act
            var customizations = new Customizations();

            var result = eventService.EditEvent(_testUserId, new EventDto(_testEventToUpdate));

            // assert
            Assert.AreEqual(_testEventId, result.Id);
            Assert.AreEqual(_testEvent.CreatedAt, result.CreatedAt);
        }

        [Test]
        public void DeleteEvent_SuccessfulTrackAndEventsDeletion()
        {
            // arrange
            var eventService = new EventService(_eventRepository, _trackRepository);

            // act
            var result =
                eventService.DeleteEvent(_testUserId, _testEventId);

            // assert
            Assert.AreEqual(_testEventId, result);
        }

        [Test]
        public void DeleteEvent_AccessDeny()
        {
            // arrange
            var eventService = new EventService(_eventRepository, _trackRepository);
            DomainException exception = null;

            // act
            try
            {
                var result =
                    eventService.DeleteEvent(_testInvalidUserId, _testEventId);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            // assert
            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }
    }
}