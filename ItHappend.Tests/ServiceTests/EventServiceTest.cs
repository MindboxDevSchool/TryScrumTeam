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
    public class EventServiceTestAccessDeny
    {
        private Guid _creatorId;
        private Guid _invalidCreatorId;
        private Track _track;
        private Event _event;

        private IEventService _eventService;

        [SetUp]
        public void Setup()
        {
            _creatorId = Guid.NewGuid();
            _invalidCreatorId = Guid.NewGuid();

            _track = new Track(
                Guid.NewGuid(),
                "Test track",
                DateTime.Now,
                _creatorId,
                new List<CustomizationType>());

            _event = new Event(
                Guid.NewGuid(),
                DateTime.Now,
                _track.Id,
                new Customizations());

            var trackRepositoryMock = new Mock<ITrackRepository>();
            trackRepositoryMock.Setup(method => method.TryGetTrackById(_track.Id))
                .Returns(_track);

            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(method => method.TryGetById(_event.Id))
                .Returns(_event);

            _eventService = new EventService(eventRepositoryMock.Object, trackRepositoryMock.Object);
        }

        [Test]
        public void TestGetEvents_ForInvalidCreatorId_ThrowsAccessDeny()
        {
            DomainException exception = null;

            try
            {
                _eventService.GetEvents(_invalidCreatorId, _track.Id);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }

        [Test]
        public void TestCreateEvent_ForInvalidCreatorId_ThrowsAccessDeny()
        {
            DomainException exception = null;

            try
            {
                _eventService.CreateEvent(_invalidCreatorId, _track.Id, DateTime.Now, new CustomizationsDto());
            }
            catch (DomainException e)
            {
                exception = e;
            }

            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }

        [Test]
        public void TestDeleteEvent_ForInvalidCreatorId_ThrowsAccessDeny()
        {
            DomainException exception = null;

            try
            {
                _eventService.DeleteEvent(_invalidCreatorId, _event.Id);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }

        [Test]
        public void TestEditEvent_ForInvalidCreatorId_ThrowsAccessDeny()
        {
            DomainException exception = null;
            var editedEvent = new EventToEditDto(_event);

            try
            {
                _eventService.EditEvent(_invalidCreatorId, editedEvent);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }
    }

    public class EventServiceTestGetAndDelete
    {
        private Guid _creatorId;
        private Track _track;
        private Event _event;

        private IEventService _eventService;

        [SetUp]
        public void Setup()
        {
            _creatorId = Guid.NewGuid();

            _track = new Track(
                Guid.NewGuid(),
                "Test track",
                DateTime.Now,
                _creatorId,
                new List<CustomizationType>());

            _event = new Event(
                Guid.NewGuid(),
                DateTime.Now,
                _track.Id,
                new Customizations());

            var trackRepositoryMock = new Mock<ITrackRepository>();
            trackRepositoryMock.Setup(method => method.TryGetTrackById(_track.Id))
                .Returns(_track);

            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(method => method.TryGetEventsByTrack(_track.Id, null, null))
                .Returns(new List<Event>() {_event});
            eventRepositoryMock.Setup(method => method.TryDelete(_event.Id))
                .Returns(_event.Id);
            eventRepositoryMock.Setup(method => method.TryGetById(_event.Id))
                .Returns(_event);

            _eventService = new EventService(eventRepositoryMock.Object, trackRepositoryMock.Object);
        }


        [Test]
        public void TestGetEvents_ForValidUserAndTrack_Successful()
        {
            DomainException exception = null;

            var result = _eventService.GetEvents(_creatorId, _track.Id);

            Assert.AreEqual(1, result.Count());
            var eventDto = result.First();
            Assert.AreEqual(_track.Id, eventDto.TrackId);
            Assert.AreEqual(_event.Id, eventDto.Id);
        }

        [Test]
        public void TestDeleteEvents_ForValidUserAndTrack_Successful()
        {
            var result = _eventService.DeleteEvent(_creatorId, _event.Id);

            Assert.AreEqual(_event.Id, result);
        }
    }

    public class TestConvertingDomainEntitiesAndDto
    {
        private IEnumerable<CustomizationType> _allTypes;

        [SetUp]
        public void SetUp()
        {
            _allTypes = new List<CustomizationType>()
            {
                CustomizationType.Comment,
                CustomizationType.Photo,
                CustomizationType.Rating,
                CustomizationType.Scale,
                CustomizationType.Geotag
            };
        }

        [Test]
        public void TestEventDto_ForEventWithEmptyCustomizations_Successful()
        {
            var @event = new Event(Guid.NewGuid(), DateTime.Now, Guid.NewGuid(), new Customizations());

            var eventDto = new EventDto(@event);
            Assert.AreEqual(@event.Id, eventDto.Id);
            Assert.AreEqual(@event.CreatedAt, eventDto.CreatedAt);
            Assert.AreEqual(@event.TrackId, eventDto.TrackId);
            Assert.IsNull(eventDto.CustomizationDto.Comment);
            Assert.IsNull(eventDto.CustomizationDto.Rating);
            Assert.IsNull(eventDto.CustomizationDto.Scale);
            Assert.IsNull(eventDto.CustomizationDto.PhotoUrl);
            Assert.IsNull(eventDto.CustomizationDto.GeotagLatitude);
            Assert.IsNull(eventDto.CustomizationDto.GeotagLongitude);
        }

        [Test]
        public void TestEventDto_ForEventWithAllCustomizations_Successful()
        {
            var customizations = new Customizations("comment", 10, 20, "photoUrl", 5, 4);
            var @event = new Event(Guid.NewGuid(), DateTime.Now, Guid.NewGuid(), customizations);

            var eventDto = new EventDto(@event);
            Assert.AreEqual(@event.Id, eventDto.Id);
            Assert.AreEqual(@event.CreatedAt, eventDto.CreatedAt);
            Assert.AreEqual(@event.TrackId, eventDto.TrackId);
            Assert.AreEqual(customizations.Comment.Value, eventDto.CustomizationDto.Comment);
            Assert.AreEqual(customizations.Rating.Value, eventDto.CustomizationDto.Rating);
            Assert.AreEqual(customizations.Scale.Value, eventDto.CustomizationDto.Scale);
            Assert.AreEqual(customizations.Photo.Value, eventDto.CustomizationDto.PhotoUrl);
            Assert.AreEqual(customizations.Geotag.Latitude, eventDto.CustomizationDto.GeotagLatitude);
            Assert.AreEqual(customizations.Geotag.Longitude, eventDto.CustomizationDto.GeotagLongitude);
        }

        [Test]
        public void TestCustomizations_ForEmptyCustomizationsAndEmptyAllowedCustomizations_Successful()
        {
            var customizationsDto = new CustomizationsDto();

            var customizations = new Customizations(customizationsDto, _allTypes);

            Assert.IsNull(customizations.Comment);
            Assert.IsNull(customizations.Rating);
            Assert.IsNull(customizations.Scale);
            Assert.IsNull(customizations.Photo);
            Assert.IsNull(customizations.Geotag);
        }

        [Test]
        public void TestCustomizations_ForEmptyCustomizationsAndFullAllowedCustomizations_Successful()
        {
            var customizationsDto = new CustomizationsDto();

            var customizations = new Customizations(customizationsDto, new List<CustomizationType>());

            Assert.IsNull(customizations.Comment);
            Assert.IsNull(customizations.Rating);
            Assert.IsNull(customizations.Scale);
            Assert.IsNull(customizations.Photo);
            Assert.IsNull(customizations.Geotag);
        }

        [Test]
        public void TestCustomizations_ForAllCustomizations_Successful()
        {
            var customizationsDto = new CustomizationsDto();
            customizationsDto.Comment = "comment";
            customizationsDto.Rating = 3;
            customizationsDto.Scale = 10;
            customizationsDto.PhotoUrl = "photo";
            customizationsDto.GeotagLatitude = 30;
            customizationsDto.GeotagLongitude = 30;

            var customizations = new Customizations(customizationsDto, _allTypes);

            Assert.AreEqual(customizationsDto.Comment, customizations.Comment.Value);
            Assert.AreEqual(customizationsDto.Comment, customizations.Comment.Value);
            Assert.AreEqual(customizationsDto.Rating, customizations.Rating.Value);
            Assert.AreEqual(customizationsDto.Scale, customizations.Scale.Value);
            Assert.AreEqual(customizationsDto.PhotoUrl, customizations.Photo.Value);
            Assert.AreEqual(customizationsDto.GeotagLatitude, customizations.Geotag.Latitude);
            Assert.AreEqual(customizations.Geotag.Longitude, customizations.Geotag.Longitude);
        }

        [Test]
        public void TestCustomizations_ForCustomizationsAndSomeAllowedCustomizations_Successful()
        {
            var customizationsDto = new CustomizationsDto();
            customizationsDto.Comment = "comment";
            customizationsDto.Rating = 3;

            var customizations = new Customizations(customizationsDto, new List<CustomizationType>()
            {
                CustomizationType.Comment
            });

            Assert.AreEqual(customizationsDto.Comment, customizations.Comment.Value);
            Assert.IsNull(customizations.Rating);
            Assert.IsNull(customizations.Scale);
            Assert.IsNull(customizations.Photo);
            Assert.IsNull(customizations.Geotag);
        }
    }

    public class EventServiceTestCreateAndUpdate
    {
        private Guid _creatorId;
        private Track _track;
        private Event _event;

        private IEventService _eventService;

        [SetUp]
        public void Setup()
        {
            _creatorId = Guid.NewGuid();

            var allowedCustomizations = new List<CustomizationType>()
                {CustomizationType.Comment, CustomizationType.Scale, CustomizationType.Rating};
            _track = new Track(
                Guid.NewGuid(),
                "Test track",
                DateTime.Now,
                _creatorId,
                allowedCustomizations);
            var customizationsDto = new CustomizationsDto();
            customizationsDto.Comment = "comment";
            customizationsDto.Scale = 5;
            customizationsDto.PhotoUrl = "photo";

            var customizationsForDto = new List<CustomizationType>(allowedCustomizations) {CustomizationType.Photo};
            _event = new Event(
                Guid.NewGuid(),
                DateTime.Now,
                _track.Id,
                new Customizations(customizationsDto, customizationsForDto));

            var trackRepositoryMock = new Mock<ITrackRepository>();
            trackRepositoryMock.Setup(method => method.TryGetTrackById(_track.Id))
                .Returns(_track);

            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(method => method.TryCreate(It.IsAny<Event>()))
                .Returns(_event);
            eventRepositoryMock.Setup(method => method.TryGetById(_event.Id))
                .Returns(_event);
            eventRepositoryMock.Setup(method => method.TryUpdate(It.IsAny<Event>()))
                .Returns(_event);


            _eventService = new EventService(eventRepositoryMock.Object, trackRepositoryMock.Object);
        }

        [Test]
        public void CreateEvent_NotAllowedCustomizations_ThrowsNotAllowedCustomizations()
        {
            DomainException exception = null;
            var customizationsDto = new CustomizationsDto();
            customizationsDto.PhotoUrl = "p";

            try
            {
                _eventService.CreateEvent(_creatorId, _track.Id, DateTime.Now, customizationsDto);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            Assert.AreEqual(DomainExceptionType.NotAllowedCustomizations, exception.Type);
        }

        [Test]
        public void CreateEvent_WithEmptyCustomizations_SuccessfulEventCreation()
        {
            var customizationsDto = new CustomizationsDto();

            var result =
                _eventService.CreateEvent(_creatorId, _track.Id, DateTime.Now, customizationsDto);

            Assert.AreEqual(_event.Id, result.Id);
        }

        [Test]
        public void CreateEvent_WithAllowedCustomizations_SuccessfulEventCreation()
        {
            var customizationsDto = new CustomizationsDto();
            customizationsDto.Comment = "p";

            var result =
                _eventService.CreateEvent(_creatorId, _track.Id, DateTime.Now, customizationsDto);

            Assert.AreEqual(_event.Id, result.Id);
        }

        [Test]
        public void CreateEvent_InvalidRating_ThrowsArgumentOutOfRangeException()
        {
            var customizationsDto = new CustomizationsDto();
            customizationsDto.Rating = 20;

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _eventService.CreateEvent(_creatorId, _track.Id, DateTime.Now, customizationsDto));
        }

        [Test]
        public void EditEvent_UpdateEmptyCustomizations_Successful()
        {
            var customizations = new CustomizationsDto();

            var result = _eventService.EditEvent(_creatorId, new EventToEditDto(_event.Id, customizations));

            Assert.AreEqual(_event.Id, result.Id);
            Assert.AreEqual(_event.CreatedAt, result.CreatedAt);
            Assert.AreEqual(_event.TrackId, result.TrackId);
        }

        [Test]
        public void EditEvent_SavedEventHasCustomizations_SuccessfulEventEditing()
        {
            var customizations = new CustomizationsDto();
            customizations.Comment = "comment 2";
            customizations.PhotoUrl = "photo url";

            var result = _eventService.EditEvent(_creatorId, new EventToEditDto(_event.Id, customizations));

            Assert.AreEqual(_event.Id, result.Id);
            Assert.AreEqual(_event.CreatedAt, result.CreatedAt);
            Assert.AreEqual(_event.TrackId, result.TrackId);
        }

        [Test]
        public void EditEvent_NotAllowedCustomizations()
        {
            var customizations = new CustomizationsDto();
            customizations.GeotagLatitude = 1;
            customizations.GeotagLongitude = 2;
            customizations.PhotoUrl = "photo url";

            DomainException exception = null;

            try
            {
                _eventService.EditEvent(_creatorId, new EventToEditDto(_event.Id, customizations));
            }
            catch (DomainException e)
            {
                exception = e;
            }

            Assert.AreEqual(DomainExceptionType.NotAllowedCustomizations, exception.Type);
        }
    }
}