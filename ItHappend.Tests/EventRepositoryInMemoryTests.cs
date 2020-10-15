using System;
using System.Collections.Generic;
using ItHappened.Domain;
using NUnit.Framework;
using ItHappened.Infrastructure;
namespace ItHappend.Tests
{
    public class EventRepositoryInMemoryTests
    {
        [Test]
        public void TryCreate_TryGetEventsByTrack()
        {
            var repository = new EventRepositoryInMemory();
            var eventId = Guid.NewGuid();
            var trackId = Guid.NewGuid();
            var newEvent = 
                new Event(eventId, DateTime.Now, trackId, new List<Customs>());
            repository.TryCreate(newEvent);
            
            var gotEvents = repository.TryGetEventsByTrack(trackId);
            
            Assert.AreEqual(eventId,gotEvents.Value[0].Id);


        }
        
        [Test]
        public void TryUpdate()
        {
            var repository = new EventRepositoryInMemory();
            var eventId = Guid.NewGuid();
            var trackId = Guid.NewGuid();
            var newEvent = 
                new Event(eventId, DateTime.Now, trackId, new List<Customs>());
            repository.TryCreate(newEvent);
            
            trackId = Guid.NewGuid();
            newEvent = 
                new Event(eventId, DateTime.Now, trackId, new List<Customs>());
            repository.TryUpdate(newEvent);
            
            var gotEvents = repository.TryGetEventsByTrack(trackId);
            
            Assert.AreEqual(eventId,gotEvents.Value[0].Id);
        }
        
        [Test]
        public void TryDelete()
        {
            var repository = new EventRepositoryInMemory();
            var eventId = Guid.NewGuid();
            var trackId = Guid.NewGuid();
            var newEvent = 
                new Event(eventId, DateTime.Now, trackId, new List<Customs>());
            repository.TryCreate(newEvent);

            repository.TryDelete(eventId);
            
            var gotEvents = repository.TryGetEventsByTrack(trackId);
            
            Assert.AreEqual(0,gotEvents.Value.Count);


        }
    }
}