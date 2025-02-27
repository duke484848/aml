﻿namespace Aml.Tests.Models.Api.CompanyController
{
    using Aml.Models.Api.CompanyController;
    using Aml.Services;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class SchedulingServiceTests
    {
        [Fact]
        public void When_rule_is_found_It_generates_non_empty_notification_sequence()
        {
            // Arrange
            var company = new Company()
            {
                Id = Guid.NewGuid(),
                CompanyNumber = "1234567890",
                CompanyType = CompanyType.Large,
                Market = Market.Denmark,
                Name = "Abc",
            };
            var schedulingConfigurationMock = new Mock<ISchedulingConfiguration>();
            schedulingConfigurationMock.Setup(x => x.Rules).Returns(new List<SchedulingRule>
            {
                new SchedulingRule
                {
                    CompanyTypes = new List<CompanyType>{ CompanyType.Large },
                    Market = Market.Denmark,
                    Interval = 5,
                    NumberOfRepetitions = 3
                }
            });
            var sut = new SchedulingService(schedulingConfigurationMock.Object);

            // Act
            var actual = sut.GenerateNotificationSchedule(company);

            //Assert
            var expected = new List<Notification>
            {
                new Notification{
                    Date =  DateTime.Now.AddDays(1)
                },
                new Notification{
                    Date =  DateTime.Now.AddDays(5)
                },
               new Notification{
                    Date =  DateTime.Now.AddDays(10)
                }
            };
            expected.RemoveAll(x => actual.Any(y => x.Date.Date == y.Date.Date));

            Assert.True(expected.Count == 0);
        }

        [Fact]
        public void When_rule_is_not_found_It_generates_empty_notification_sequence()
        {
            // Arrange
            var company = new Company()
            {
                Id = Guid.NewGuid(),
                CompanyNumber = "1234567890",
                CompanyType = CompanyType.Large,
                Market = Market.Denmark,
                Name = "Abc",
            };
            var schedulingConfigurationMock = new Mock<ISchedulingConfiguration>();
            schedulingConfigurationMock.Setup(x => x.Rules).Returns(new List<SchedulingRule>());
            var sut = new SchedulingService(schedulingConfigurationMock.Object);

            // Act
            var actual = sut.GenerateNotificationSchedule(company);

            //Assert
            Assert.True(actual.Count == 0);
        }
    }
}