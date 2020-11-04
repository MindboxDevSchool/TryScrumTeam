using System;
using System.Collections.Generic;
using System.Linq;
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
            moqTrackRepository = new Mock<ITrackRepository>();
            
            userId = Guid.NewGuid();
            var allowedCustomizations = new List<CustomizationType>() {CustomizationType.Rating};
            
            track = new Track(
                Guid.NewGuid(),
                "TestTrack",
                DateTime.Now, 
                userId,
                allowedCustomizations
            );

            events = new List<Event>()
            {
                new Event(
                    Guid.NewGuid(),
                    DateTime.Now - TimeSpan.FromDays(40), 
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 1}, allowedCustomizations)),
                new Event(
                    Guid.NewGuid(),
                    DateTime.Now - TimeSpan.FromDays(15),
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 4}, allowedCustomizations)),
                new Event(
                    Guid.NewGuid(),
                    DateTime.Now - TimeSpan.FromDays(10),
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 7}, allowedCustomizations)),
            };
        }

        private Mock<IEventRepository> moqEventRepository;
        private Mock<ITrackRepository> moqTrackRepository;
        private ItHappenedSettings _settings;
        private List<Event> events;
        private Track track;
        private Guid userId;

        [Test]
        public void GetTrackStatistics_ReturnStatisticsForCorrectData()
        {
            // arrange
            moqEventRepository.Setup(method => method.TryGetEventsByTrack(It.IsAny<Guid>(), null, null))
                .Returns(events);
            moqTrackRepository.Setup(method => method.TryGetTrackById(It.IsAny<Guid>()))
                .Returns(track);
            
            _settings = new ItHappenedSettings(
                3, 
                30, 
                5);
            
            var statisticsService = new StatisticsService(
                moqEventRepository.Object, 
                moqTrackRepository.Object, 
                _settings);

            // act
            var statistics = statisticsService.GetTrackStatistics(userId, track.Id);

            // assert
            Assert.AreEqual(1, statistics.Count());
            Assert.AreEqual(
                $"Событие TestTrack с самым высоким рейтингом 7 произошло {events[2].CreatedAt.ToString()}",
                statistics[0]);
        }
    }
}