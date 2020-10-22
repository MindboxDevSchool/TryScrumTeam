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
    public class TracksServiceTests
    {
        [SetUp]
        public void Setup()
        {
            _trackRepository = new MockTrackRepository();
            _eventRepository = new MockEventRepository();
            _userRepository = new MockUserRepository();
            _authData = new AuthData(Guid.Parse("00000000000000000000000000000002"), "01");
            _authDataWrong = new AuthData(Guid.Parse("00000000000000000000000000000010"), "03");
        }
        
        private ITrackRepository _trackRepository;
        private IEventRepository _eventRepository;
        private IUserRepository _userRepository;
        private AuthData _authData;
        private AuthData _authDataWrong;

        [Test]
        public void GetTracks_SuccessfulTracksReceiving()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository, _userRepository);
            
            // act
            var result = tracksService.GetTracks(_authData);

            // assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Track1", result.First().Name);
        }
        
        
        [Test]
        public void CreateTrack_SuccessfulTrackCreation()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository, _userRepository);

            // act
            var result =
                tracksService.CreateTrack(_authData, "NewTrack", DateTime.Now, new List<CustomizationType>());

            // assert
            Assert.AreEqual("NewTrack", result.Name);
        }

        [Test]
        public void EditTrack_SuccessfulTrackEditing()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository, _userRepository);
            var trackDto = new TrackDto(new Track(Guid.NewGuid(), "Track", DateTime.Parse("2020-10-16 0:0:0Z"), _authData.Id, new List<CustomizationType>()));
            DomainException exception = null;
            
            // act
            try
            {
                var result = tracksService.EditTrack(_authData, trackDto);
            }
            catch (DomainException e)
            {
                exception = e;
            }

            // assert
            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }
        
        [Test]
        public void EditTrack_UnsuccessfulTrackEditing_TryingToUpdateCreationDate()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository, _userRepository);
            var trackDto = new TrackDto(new Track(Guid.NewGuid(), "Track", DateTime.Parse("2020-10-17 0:0:0Z"), _authData.Id, new List<CustomizationType>()));
            DomainException exception = null;
            
            // act
            try
            {
                var result = tracksService.EditTrack(_authData, trackDto);
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
            var tracksService = new TracksService(_trackRepository, _eventRepository, _userRepository);
            DomainException exception = null;

            // act
            try
            {
                var result = tracksService.DeleteTrack(_authData, Guid.NewGuid());
            }
            catch (DomainException e)
            {
                exception = e;
            }


            // assert
            Assert.AreEqual(DomainExceptionType.TrackAccessDenied, exception.Type);
        }

        private class MockTrackRepository : ITrackRepository
        {
            public Track TryCreate(Track track)
            {
                return track;
            }

            public IEnumerable<Track> TryGetTracksByUser(Guid userId)
            {
                return new List<Track>(
                    new List<Track>()
                    {
                        new Track(Guid.NewGuid(), "Track1", DateTime.Now, Guid.NewGuid(), new List<CustomizationType>()),
                        new Track(Guid.NewGuid(), "Track2", DateTime.Now, Guid.NewGuid(), new List<CustomizationType>()),
                    }
                );
            }

            public Track TryGetTrackById(Guid trackId)
            {
                return new Track(
                    Guid.NewGuid(), 
                    "Track1", 
                    DateTime.Parse("2020-10-16 0:0:0Z"), 
                    Guid.NewGuid(),
                    new List<CustomizationType>());
            }

            public Track TryUpdate(Track track)
            {
                return track;
            }

            public Guid TryDelete(Guid trackId)
            {
                return trackId;
            }
        }

        private class MockEventRepository : IEventRepository
        {
            public Guid TryDeleteByTrack(Guid trackId)
            {
                return trackId;
            }

            public Event TryCreate(Event @event)
            {
                return @event;
            }

            public IEnumerable<Event> TryGetEventsByTrack(Guid trackId)
            {
                return new List<Event>();
            }

            public Event TryGetById(Guid id)
            {
                throw new NotImplementedException();
            }

            public Event TryUpdate(Event @event)
            {
                return @event;
            }

            public Guid TryDelete(Guid eventId)
            {
                return eventId;
            }
        }

        private class MockUserRepository : IUserRepository
        {
            public Result<User> TryCreate(User user)
            {
                throw new NotImplementedException();
            }

            public Result<User> TryGetByLogin(string login)
            {
                throw new NotImplementedException();
            }

            public Result<User> TryGetById(Guid id)
            {
                throw new NotImplementedException();
            }

            public Result<bool> IsUserAuthDataValid(AuthData data)
            {
                return new Result<bool>(true);
            }
        }
    }
}