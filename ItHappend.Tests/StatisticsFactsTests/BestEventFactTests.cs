﻿using System;
using System.Collections.Generic;
using ItHappened;
using ItHappened.Domain;
using ItHappened.Domain.StatisticsFacts;
using NUnit.Framework;

namespace ItHappend.Tests.StatisticsFactsTests
{
    public class BestEventFactTests
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
        
        private ItHappenedSettings _settings;
        private List<Event> events;
        private string trackName;

        [Test]
        public void Process_ReturnApplicableFactForCorrectData()
        {
            // arrange
            _settings = new ItHappenedSettings(
                new BestEventSettings(
                    3, 
                    30, 
                    5
                    ));

            var bestEventFact = new BestEventFact(_settings);

            // act
            bestEventFact.Process(events, trackName);

            // assert
            Assert.IsTrue(bestEventFact.IsApplicable);
            Assert.AreEqual(7, bestEventFact.Priority);
            Assert.AreEqual(
                $"Событие TestTrack с самым высоким рейтингом 7 произошло {events[2].CreatedAt.ToString()}",
                bestEventFact.Description);
        }
        
        [Test]
        public void Process_EventsCountWithRatingNotSatisfyRequiredCondition_ReturnNotApplicableFact()
        {
            // arrange
            _settings = new ItHappenedSettings(
                new BestEventSettings(
                    4, 
                    30, 
                    5
                ));
            
            var bestEventFact = new BestEventFact(_settings);

            // act
            bestEventFact.Process(events, trackName);

            // assert
            Assert.IsFalse(bestEventFact.IsApplicable);
        }
        
        [Test]
        public void Process_DaysSinceLastEventNotSatisfyRequiredCondition_ReturnNotApplicableFact()
        {
            // arrange
            _settings = new ItHappenedSettings(
                new BestEventSettings(
                    3, 
                    60, 
                    5
                ));

            var bestEventFact = new BestEventFact(_settings);

            // act
            bestEventFact.Process(events, trackName);

            // assert
            Assert.IsFalse(bestEventFact.IsApplicable);
        }
        
        [Test]
        public void Process_DaysSinceBestEventNotSatisfyRequiredCondition_ReturnNotApplicableFact()
        {
            // arrange
            _settings = new ItHappenedSettings(
                new BestEventSettings(
                    3, 
                    30, 
                    15
                ));

            var bestEventFact = new BestEventFact(_settings);

            // act
            bestEventFact.Process(events, trackName);

            // assert
            Assert.IsFalse(bestEventFact.IsApplicable);
        }
        
        [Test]
        public void Process_TwoEventsWithMaximumRating_ReturnNotApplicableFact()
        {
            // arrange
            _settings = new ItHappenedSettings(
                new BestEventSettings(
                    3, 
                    30, 
                    7
                ));

            events.Add(new Event(
                Guid.NewGuid(), 
                DateTime.Now - TimeSpan.FromDays(20),
                Guid.NewGuid(),
                new Customizations(new CustomizationsDto() {Rating = 7}, 
                    new List<CustomizationType>() {CustomizationType.Rating})));

            var bestEventFact = new BestEventFact(_settings);

            // act
            bestEventFact.Process(events, trackName);

            // assert
            Assert.IsTrue(bestEventFact.IsApplicable);
            Assert.AreEqual(
                $"Событие TestTrack с самым высоким рейтингом 7 произошло {events[3].CreatedAt.ToString()}",
                bestEventFact.Description);
        }
    }
}