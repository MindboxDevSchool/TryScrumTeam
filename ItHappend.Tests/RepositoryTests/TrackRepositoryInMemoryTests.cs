using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using NUnit.Framework;

namespace ItHappend.Tests
{
    public class TrackRepositoryInMemoryTests
    {
        [Test]
        public void TryCreate_TryGetEventsByUser()
        {
            var repository = new TrackRepositoryInMemory();
            var creatorId = Guid.NewGuid();
            var trackId = Guid.NewGuid();
            var newTrack = 
                new Track(trackId,"1",DateTime.Now, creatorId,new List<CustomizationType>());
            repository.TryCreate(newTrack);
            
            var gotTracks = repository.TryGetTracksByUser(creatorId);
            
            Assert.AreEqual(trackId,gotTracks.First().Id);


        }
        
        [Test]
        public void TryUpdate()
        {
            var repository = new TrackRepositoryInMemory();
            var creatorId = Guid.NewGuid();
            var trackId = Guid.NewGuid();
            var newTrack = 
                new Track(trackId,"1",DateTime.Now, creatorId,new List<CustomizationType>());
            repository.TryCreate(newTrack);
            
            creatorId = Guid.NewGuid();
            
            newTrack = 
                new Track(trackId,"1",DateTime.Now, creatorId,new List<CustomizationType>());
            repository.TryCreate(newTrack);
            
            var gotTracks = repository.TryGetTracksByUser(creatorId);
            
            Assert.AreEqual(trackId,gotTracks.First().Id);
        }
        
        [Test]
        public void TryDelete()
        {
            var repository = new TrackRepositoryInMemory();
            var creatorId = Guid.NewGuid();
            var trackId = Guid.NewGuid();
            var newTrack = 
                new Track(trackId,"1",DateTime.Now, creatorId,new List<CustomizationType>());
            repository.TryCreate(newTrack);

            repository.TryDelete(trackId);
            
            var gotTracks = repository.TryGetTracksByUser(creatorId);
            
            Assert.IsEmpty(gotTracks);


        }
    }
}