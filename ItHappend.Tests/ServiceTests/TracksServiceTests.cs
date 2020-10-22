﻿using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;
using NUnit.Framework;
using Moq;

namespace ItHappend.Tests
{
    public class TracksServiceTests
    {
        private void SetupEntities()
        {
            _testTrackId = Guid.NewGuid();
            _testUserId = Guid.NewGuid();
            _testTrack = new Track(
                _testTrackId,
                "Test track",
                DateTime.Now,
                _testUserId,
                new List<CustomizationType>());
            _authData = new AuthData(_testUserId, Guid.NewGuid().ToString());
            _authDataWrong = new AuthData(Guid.NewGuid(), Guid.NewGuid().ToString());
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
            mock.Setup(method => method.TryGetTrackById(It.IsAny<Guid>()))
                .Returns(_testTrack);
            mock.Setup(method => method.TryGetTracksByUser(It.IsAny<Guid>()))
                .Returns(new List<Track>() {_testTrack});
            mock.Setup(method => method.TryCreate(It.IsAny<Track>()))
                .Returns(_testTrack);
            mock.Setup(method => method.TryUpdate(It.IsAny<Track>()))
                .Returns(_testTrack);
            mock.Setup(method => method.TryDelete(It.IsAny<Guid>()))
                .Returns(_testTrackId);

            _trackRepository = mock.Object;
        }

        private Guid _testUserId;
        private Guid _testTrackId;
        private Track _testTrack;
        private IEventRepository _eventRepository;
        private ITrackRepository _trackRepository;
        private AuthData _authData;
        private AuthData _authDataWrong;

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
            var result = tracksService.GetTracks(_authData);

            // assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Test track", result.First().Name);
        }
        
        
        [Test]
        public void CreateTrack_SuccessfulTrackCreation()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);

            // act
            var result =
                tracksService.CreateTrack(_authData, "Test track", DateTime.Now, new List<CustomizationType>());

            // assert
            Assert.AreEqual("Test track", result.Name);
        }

        [Test]
        public void EditTrack_SuccessfulTrackEditing()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);
            var trackDto = new TrackDto(_testTrack);

            // act
            var result = tracksService.EditTrack(_authData, trackDto);

                // assert
            Assert.AreEqual(_testTrackId, result.Id);
        }
        
        [Test]
        public void EditTrack_UnsuccessfulTrackEditing_TryingToUpdateCreationDate()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);
            var trackDto = new TrackDto(_testTrack);
            DomainException exception = null;
            
            // act
            try
            {
                var result = tracksService.EditTrack(_authDataWrong, trackDto);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            // assert
            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }

        [Test]
        public void DeleteTrack_SuccessfulTrackAndEventsDeletion()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);

            // act
            var result = tracksService.DeleteTrack(_authData, Guid.NewGuid());
            
            // assert
            Assert.AreEqual(_testTrackId, result);
        }
    }
}