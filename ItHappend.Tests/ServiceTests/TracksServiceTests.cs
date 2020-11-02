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
    public class TracksServiceTests
    {
        private void SetupEntities()
        {
            _testTrackId = Guid.NewGuid();
            _testUserId = Guid.NewGuid();
            _testInvalidUserId = Guid.NewGuid();
            _testTrack = new Track(
                _testTrackId,
                "Test track",
                DateTime.Now,
                _testUserId,
                new List<CustomizationType>());
            _testTrackUpdated = new Track(
                _testTrackId,
                "Test track Updated",
                _testTrack.CreatedAt,
                _testUserId,
                new List<CustomizationType>());
        }

        private void SetupMoqEventRepository()
        {
            var mock = new Mock<IEventRepository>();
            mock.Setup(method => method.TryDeleteByTrack(It.IsAny<Guid>()))
                .Returns(_testTrackId);

            _eventRepository = mock.Object;
        }

        private void SetupMoqTrackRepository()
        {
            var mock = new Mock<ITrackRepository>();
            mock.Setup(method => method.TryGetTrackById(_testTrackId))
                .Returns(_testTrack);
            mock.Setup(method => method.TryGetTracksByUser(_testUserId, null, null))
                .Returns(new List<Track>() {_testTrack});
            
            mock.Setup(method => method.TryCreate(
                    It.Is<Track>(tr=> _testTrack.Name == tr.Name 
                                      && Equals(_testTrack.AllowedCustomizations, tr.AllowedCustomizations) 
                                      && _testTrack.CreatedAt == tr.CreatedAt
                                      && _testTrack.CreatorId == tr.CreatorId)))
                .Returns(_testTrack);
            
            mock.Setup(method => method.TryUpdate(
                    It.Is<Track>(tr=> _testTrackUpdated.Name == tr.Name 
                                      && Equals(_testTrackUpdated.AllowedCustomizations, tr.AllowedCustomizations) 
                                      && _testTrackUpdated.CreatedAt == tr.CreatedAt
                                      && _testTrackUpdated.CreatorId == tr.CreatorId)))
                .Returns(_testTrackUpdated);
            
            mock.Setup(method => method.TryDelete(_testTrackId))
                .Returns(_testTrackId);

            _trackRepository = mock.Object;
        }

        private Guid _testUserId;
        private Guid _testInvalidUserId;
        private Guid _testTrackId;
        private Track _testTrack;
        private Track _testTrackUpdated;
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
        public void GetTracks_SuccessfulTracksReceiving()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);
            
            // act
            var result = tracksService.GetTracks(_testUserId);

            // assert
            var trackDtos = result.ToList();
            Assert.AreEqual(1, trackDtos.Count());
            Assert.AreEqual(_testTrack.Name, trackDtos.Single().Name);
            Assert.AreEqual(_testUserId, trackDtos.Single().CreatorId);
            Assert.AreEqual(_testTrack.AllowedCustomizations, trackDtos.Single().AllowedCustomizations);
            Assert.AreEqual(_testTrack.CreatedAt, trackDtos.Single().CreatedAt);
        }
        
        
        [Test]
        public void CreateTrack_SuccessfulTrackCreation()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);

            // act
            var result =
                tracksService.CreateTrack(_testUserId, _testTrack.Name, _testTrack.CreatedAt, _testTrack.AllowedCustomizations);

            // assert
            Assert.AreEqual(_testTrack.Name, result.Name);
            Assert.AreEqual(_testUserId, result.CreatorId);
            Assert.AreEqual(_testTrack.AllowedCustomizations, result.AllowedCustomizations);
            Assert.AreEqual(_testTrack.CreatedAt, result.CreatedAt);
        }

        [Test]
        public void EditTrack_SuccessfulTrackEditing()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);
            var trackDto = new TrackToEditDto(_testTrackUpdated.Id, _testTrackUpdated.Name, _testTrackUpdated.AllowedCustomizations);

            // act
            var result = tracksService.EditTrack(_testUserId, trackDto);

                // assert
            Assert.AreEqual(result.Id,_testTrackUpdated.Id);
            Assert.AreEqual(result.Name,_testTrackUpdated.Name);
            Assert.AreEqual(result.AllowedCustomizations,_testTrackUpdated.AllowedCustomizations);
            Assert.AreEqual(result.CreatedAt,_testTrackUpdated.CreatedAt);
            Assert.AreEqual(result.CreatorId,_testTrackUpdated.CreatorId);
        }

        [Test]
        public void DeleteTrack_SuccessfulTrackAndEventsDeletion()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);

            // act
            var result = tracksService.DeleteTrack(_testUserId, _testTrackId);

            // assert
            Assert.AreEqual(_testTrackId, result);
        }
        
        [Test]
        public void DeleteTrack_AccessDeny()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);
            DomainException exception = null;

            // act
            try
            {
                var result = tracksService.DeleteTrack(_testInvalidUserId, _testTrackId);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            // assert
            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }
        
        [Test]
        public void EditTrack_AccessDeny()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);
            var trackDto = new TrackToEditDto(_testTrack.Id, _testTrack.Name, _testTrack.AllowedCustomizations);
            DomainException exception = null;

            // act
            try
            {
                var result = tracksService.EditTrack(_testInvalidUserId, trackDto);
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