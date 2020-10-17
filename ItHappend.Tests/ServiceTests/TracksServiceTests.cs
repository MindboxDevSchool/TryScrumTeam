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
            _authData = new AuthData(Guid.Parse("00000000000000000000000000000002"), "01");
        }
        
        private ITrackRepository _trackRepository;
        private IEventRepository _eventRepository;
        private AuthData _authData;

        [Test]
        public void GetTracks_SuccessfulTracksReceiving()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);
            
            // act
            var result = tracksService.GetTracks(_authData);

            // assert
            Assert.AreEqual(2, result.Value.Count());
            Assert.AreEqual("Track1", result.Value.First().Name);
        }

        [Test]
        public void CreateTrack_SuccessfulTrackCreation()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);

            // act
            var result =
                tracksService.CreateTrack(_authData, "NewTrack", DateTime.Now, new List<CustomType>());

            // assert
            Assert.IsTrue(result.IsSuccessful());
            Assert.AreEqual("NewTrack", result.Value.Name);
        }

        [Test]
        public void EditTrack_SuccessfulTrackEditing()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);
            var trackDto = new TrackDto(new Track(new Guid(), "Track", DateTime.Parse("2020-10-16 0:0:0Z"), _authData.Id, new List<CustomType>()));
            
            // act
            var result = tracksService.EditTrack(_authData, trackDto);

            // assert
            Assert.IsTrue(result.IsSuccessful());
            Assert.AreEqual("Track", result.Value.Name);
        }
        
        [Test]
        public void EditTrack_UnsuccessfulTrackEditing_TryingToUpdateCreationDate()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);
            var trackDto = new TrackDto(new Track(new Guid(), "Track", DateTime.Parse("2020-10-17 0:0:0Z"), _authData.Id, new List<CustomType>()));
            
            // act
            var result = tracksService.EditTrack(_authData, trackDto);

            // assert
            Assert.IsFalse(result.IsSuccessful());
            Assert.IsTrue(result.Exception is EditingImmutableDataException);
        }

        [Test]
        public void DeleteTrack_SuccessfulTrackAndEventsDeletion()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository, _eventRepository);

            // act
            var result = tracksService.DeleteTrack(_authData, new Guid());

            // assert
            Assert.IsTrue(result.Value);
        }

        private class MockTrackRepository : ITrackRepository
        {
            public Result<Track> TryCreate(Track track)
            {
                return new Result<Track>(track);
            }

            public Result<IEnumerable<Track>> TryGetTracksByUser(Guid userId)
            {
                return new Result<IEnumerable<Track>>(
                    new List<Track>()
                    {
                        new Track(new Guid(), "Track1", DateTime.Now, new Guid(), new List<CustomType>()),
                        new Track(new Guid(), "Track2", DateTime.Now, new Guid(), new List<CustomType>()),
                    }
                );
            }

            public Result<Track> TryGetTrackById(Guid trackId)
            {
                return new Result<Track>(
                    new Track(new Guid(), "Track1", DateTime.Parse("2020-10-16 0:0:0Z"), new Guid(), new List<CustomType>())
                );
            }

            public Result<Track> TryUpdate(Track track)
            {
                return new Result<Track>(track);
            }

            public Result<bool> TryDelete(Guid trackId)
            {
                return new Result<bool>(true);
            }
        }

        private class MockEventRepository : IEventRepository
        {
            public Result<bool> TryDeleteByTrack(Guid trackId)
            {
                return new Result<bool>(true);
            }

            public Result<Event> TryCreate(Event @event)
            {
                return new Result<Event>(new Exception());
            }

            public Result<IEnumerable<Event>> TryGetEventsByTrack(Guid trackId)
            {
                return new Result<IEnumerable<Event>>(new Exception());
            }

            public Result<Event> TryGetById(Guid id)
            {
                throw new NotImplementedException();
            }

            public Result<Event> TryUpdate(Event @event)
            {
                return new Result<Event>(new Exception());
            }

            public Result<bool> TryDelete(Guid eventId)
            {
                return new Result<bool>(new Exception());
            }
        }
    }
}