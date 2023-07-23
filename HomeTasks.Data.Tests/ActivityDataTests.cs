using HomeTask.Data;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HomeTasks.Data.Tests
{
    public class ActivityDataTests
    {
        public ActivityData _systemUnderTests;

        [SetUp]
        public void Setup()
        {
            _systemUnderTests = new ActivityData();
        }

        [Test]
        public void GetActivities_Return_Results()
        {
            //Arrange & Act
            var result = _systemUnderTests.GetActivities("Classroom1", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.That(result.Data.Count, Is.GreaterThan(1));
        }

        [Test]
        public void GetActivities_Return_No_Results()
        {
            //Arrange & Act
            var result = _systemUnderTests.GetActivities("Test", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public void GetActivity_Return_Results()
        {
            //Arrange & Act
            var result = _systemUnderTests.GetActivity(1, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.That(result.Data.ActivityId, Is.EqualTo(1));
        }

        [Test]
        public void GetActivity_Return_No_Results()
        {
            //Arrange & Act
            var result = _systemUnderTests.GetActivity(10, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public void GetAttendedActivity_Return_Results()
        {
            //Arrange & Act
            var result = _systemUnderTests.GetAttendedActivity(1,1, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.That(result.Data.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetAttendedActivity_Return_No_Results()
        {
            //Arrange & Act
            var result = _systemUnderTests.GetAttendedActivity(10,0, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public void CreateNewAttendence_Returns_Success()
        {
            //Arrange & Act
            var result = _systemUnderTests.CreateNewAttendence(10, 1, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.IsNotNull(result.Data);
            Assert.That(result.Data.ActivityId, Is.EqualTo(10));
            Assert.That(result.Data.StudentId, Is.EqualTo(1));
        }
    }
}
