using System;
using System.Collections.Generic;
using ItHappened;
using ItHappened.Domain;
using ItHappened.Domain.StatisticsFacts;
using NUnit.Framework;

namespace ItHappend.Tests.StatisticsFactsTests
{
    public class WorstEventFactTests
    {
        [SetUp]
        public void Setup()
        {
            var allowedCustomizations = new List<CustomizationType>() {CustomizationType.Rating};

            trackName = "TestTrack";

            events = new List<Event>()
            {
                new Event(
                    Guid.NewGuid(),
                    DateTime.Now - TimeSpan.FromDays(40),
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 4}, allowedCustomizations)),
                new Event(
                    Guid.NewGuid(),
                    DateTime.Now - TimeSpan.FromDays(15), 
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 1}, allowedCustomizations)),
                new Event(
                    Guid.NewGuid(),
                    DateTime.Now - TimeSpan.FromDays(10),
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 7}, allowedCustomizations)),
            };
        }
        
        private ItHappenedSettings _settings;
        private List<Event> events;
        private string trackName;

        [Test]
        public void Process_ReturnApplicableFactForCorrectData()
        {
            // arrange
            _settings = new ItHappenedSettings(
                null,
                new TrackSettingsForRatingFacts(
                    3, 
                    30, 
                    5
                    ));

            var worstEventFact = new WorstEventFact(_settings);

            // act
            worstEventFact.Process(events, trackName);

            // assert
            Assert.IsTrue(worstEventFact.IsApplicable);
            Assert.AreEqual(9, worstEventFact.Priority);
            Assert.AreEqual(
                $"Событие TestTrack с самым низким рейтингом 1 произошло {events[1].CreatedAt.ToString()}",
                worstEventFact.Description);
        }
        
        [Test]
        public void Process_EventsCountWithRatingNotSatisfyRequiredCondition_ReturnNotApplicableFact()
        {
            // arrange
            _settings = new ItHappenedSettings(
                null,
                new TrackSettingsForRatingFacts(
                    4, 
                    30, 
                    5
                ));
            
            var worstEventFact = new WorstEventFact(_settings);

            // act
            worstEventFact.Process(events, trackName);

            // assert
            Assert.IsFalse(worstEventFact.IsApplicable);
        }
        
        [Test]
        public void Process_DaysSinceEarliestEventNotSatisfyRequiredCondition_ReturnNotApplicableFact()
        {
            // arrange
            _settings = new ItHappenedSettings(
                null,
                new TrackSettingsForRatingFacts(
                    3, 
                    60, 
                    5
                ));

            var worstEventFact = new WorstEventFact(_settings);

            // act
            worstEventFact.Process(events, trackName);

            // assert
            Assert.IsFalse(worstEventFact.IsApplicable);
        }
        
        [Test]
        public void Process_DaysSinceWorstEventNotSatisfyRequiredCondition_ReturnNotApplicableFact()
        {
            // arrange
            _settings = new ItHappenedSettings(
                null,
                new TrackSettingsForRatingFacts(
                    3, 
                    30, 
                    20
                ));

            var worstEventFact = new WorstEventFact(_settings);

            // act
            worstEventFact.Process(events, trackName);

            // assert
            Assert.IsFalse(worstEventFact.IsApplicable);
        }
        
        [Test]
        public void Process_TwoEventsWithMinimalRating_ReturnApplicableFact()
        {
            // arrange
            _settings = new ItHappenedSettings(
                null,
                new TrackSettingsForRatingFacts(
                    3, 
                    30, 
                    7
                ));

            events.Add(new Event(
                Guid.NewGuid(), 
                DateTime.Now - TimeSpan.FromDays(30),
                Guid.NewGuid(),
                new Customizations(new CustomizationsDto() {Rating = 1}, 
                    new List<CustomizationType>() {CustomizationType.Rating})));

            var worstEventFact = new WorstEventFact(_settings);

            // act
            worstEventFact.Process(events, trackName);

            // assert
            Assert.IsTrue(worstEventFact.IsApplicable);
            Assert.AreEqual(
                $"Событие TestTrack с самым низким рейтингом 1 произошло {events[3].CreatedAt.ToString()}",
                worstEventFact.Description);
        }
    }
}