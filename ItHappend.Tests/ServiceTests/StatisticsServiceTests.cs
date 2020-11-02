using System;
using System.Collections.Generic;
using ItHappened;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;
using NUnit.Framework;
using Moq;

namespace ItHappend.Tests.ServiceTests
{
    public class StatisticsServiceTests
    {
        [SetUp]
        public void Setup()
        {
            moqEventRepository = new Mock<IEventRepository>();

            _settings = new ItHappenedSettings(
                3, 
                30, 
                0);

            events = new List<Event>()
            {
                new Event(
                    Guid.NewGuid(),
                    DateTime.Today,
                    Guid.NewGuid(),
                    new Customizations(null, null, null, null, 1, null)),
                new Event(
                    Guid.NewGuid(),
                    DateTime.Today,
                    Guid.NewGuid(),
                    new Customizations(null, null, null, null, 4, null)),
                new Event(
                    Guid.NewGuid(),
                    DateTime.Today,
                    Guid.NewGuid(),
                    new Customizations(null, null, null, null, 7, null))
            };
        }
        
        private Mock<IEventRepository> moqEventRepository;
        private ItHappenedSettings _settings;
        
        private List<Event> events;

        [Test]
        public void GetBestEvent_ReturnBestEventForCorrectData()
        {
            // arrange
            moqEventRepository.Setup(method => method.TryGetEventsByTrack(It.IsAny<Guid>()))
                .Returns(events);
            
            var statisticsService = new StatisticsService(moqEventRepository.Object, _settings);

            // act
            var bestEvent = statisticsService.GetBestEvent(Guid.NewGuid());

            // assert
            Assert.AreEqual(7, bestEvent.CustomizationDto.Rating);
        }
    }
}