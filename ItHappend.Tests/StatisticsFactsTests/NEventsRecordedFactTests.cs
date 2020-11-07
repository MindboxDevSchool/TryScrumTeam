using System;
using System.Collections.Generic;
using ItHappened;
using ItHappened.Domain;
using ItHappened.Domain.StatisticsFacts.General;
using ItHappened.Domain.StatisticsFacts.SpecificToTracks;
using NUnit.Framework;

namespace ItHappend.Tests.StatisticsFactsTests
{
    public class NEventsRecordedFactTests
    {
        [SetUp]
        public void Setup()
        {
            var allowedCustomizations = new List<CustomizationType>();

            trackName = "TestTrack";

            events = new List<Event>()
            {
                new Event(
                    Guid.NewGuid(),
                    DateTime.Now, 
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 1}, allowedCustomizations)),
                new Event(
                    Guid.NewGuid(),
                    DateTime.Now,
                    Guid.NewGuid(),
                    new Customizations(new CustomizationsDto() {Rating = 4}, allowedCustomizations)),
                new Event(
                    Guid.NewGuid(),
                    DateTime.Now,
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
                null,
                new NEventsRecordedFactSettings(2));

            var nEventsRecordedFact = new NEventsRecordedFact(_settings);

            // act
            nEventsRecordedFact.Process(events, trackName);

            // assert
            Assert.IsTrue(nEventsRecordedFact.IsApplicable);
            Assert.AreEqual(Math.Log(3), nEventsRecordedFact.Priority);
            Assert.AreEqual(
                $"У вас произошло уже 3 события!",
                nEventsRecordedFact.Description);
        }
        
        [Test]
        public void Process_AmountOfEventsNotSatisfyRequiredCondition_ReturnNotApplicableFact()
        {
            // arrange
            _settings = new ItHappenedSettings(
                null,
                null,
                new NEventsRecordedFactSettings(3));

            var nEventsRecordedFact = new NEventsRecordedFact(_settings);

            // act
            nEventsRecordedFact.Process(events, trackName);

            // assert
            Assert.IsFalse(nEventsRecordedFact.IsApplicable);
        }
    }
}