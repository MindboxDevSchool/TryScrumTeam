using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application;
using ItHappened.Domain;
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
        }

        [Test]
        public void GetTracks()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository);
            
            // act
            var result = tracksService.GetTracks(new AuthData(new Guid(), "01"));

            // assert
            Assert.AreEqual(2, result.Value.Count());
            Assert.AreEqual("Track1", result.Value.First().Name);
        }

        [Test]
        public void CreateTrack()
        {
            // arrange
            var tracksService = new TracksService(_trackRepository);
            var authData = new AuthData(new Guid(), "01");
            
            // act
            var result =
                tracksService.CreateTrack(authData, "NewTrack", DateTime.Now, new List<CustomType>());

            // assert
            Assert.IsTrue(result.IsSuccessful());
            Assert.AreEqual("NewTrack", result.Value.Name);
        }

        private ITrackRepository _trackRepository;

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

            public Result<Track> TryUpdate(Track track)
            {
                throw new NotImplementedException();
            }

            public Result<bool> TryDelete(Guid trackId)
            {
                throw new NotImplementedException();
            }
        }
    }
}