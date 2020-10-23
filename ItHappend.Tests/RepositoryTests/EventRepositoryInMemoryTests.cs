using System;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Infrastructure.Repositories;
using NUnit.Framework;

namespace ItHappend.Tests.RepositoryTests
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
                new Event(eventId, DateTime.Now, trackId, new Customizations());
            repository.TryCreate(newEvent);
            
            var gotEvents = repository.TryGetEventsByTrack(trackId);
            
            Assert.AreEqual(eventId,gotEvents.First().Id);


        }
        
        [Test]
        public void TryUpdate()
        {
            var repository = new EventRepositoryInMemory();
            var eventId = Guid.NewGuid();
            var trackId = Guid.NewGuid();
            var newEvent = 
                new Event(eventId, DateTime.Now, trackId, new Customizations());
            repository.TryCreate(newEvent);
            
            trackId = Guid.NewGuid();
            newEvent = 
                new Event(eventId, DateTime.Now, trackId, new Customizations());
            repository.TryUpdate(newEvent);
            
            var gotEvents = repository.TryGetEventsByTrack(trackId);
            
            Assert.AreEqual(eventId,gotEvents.First().Id);
        }
        
        [Test]
        public void TryDelete()
        {
            var repository = new EventRepositoryInMemory();
            var eventId = Guid.NewGuid();
            var trackId = Guid.NewGuid();
            var newEvent = 
                new Event(eventId, DateTime.Now, trackId, new Customizations());
            repository.TryCreate(newEvent);

            repository.TryDelete(eventId);
            
            var gotEvents = repository.TryGetEventsByTrack(trackId);
            
            Assert.IsEmpty(gotEvents);


        }
    }
}