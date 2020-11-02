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

            var allowedCustomizations = new List<CustomizationType>() {CustomizationType.Rating};

            events = new List<Event>()
            {
                new Event(
                    Guid.NewGuid(),
                    DateTime.Today,
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 1}, allowedCustomizations)),
                new Event(
                    Guid.NewGuid(),
                    DateTime.Today,
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 4}, allowedCustomizations)),
                new Event(
                    Guid.NewGuid(),
                    DateTime.Today,
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 7}, allowedCustomizations)),
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