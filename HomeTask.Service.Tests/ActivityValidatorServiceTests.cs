using HomeTask.Data.Interfaces;
using HomeTask.Data.Models;
using HomeTask.Utils;
using Moq;
using NUnit.Framework;
using System.Net;

namespace HomeTask.Service.Tests
{
    public class ActivityValidatorServiceTests
    {
        private Mock<IActivityData> _mockedActivityData;
        private ActivityValidatorService _systemUnderTest;

        [SetUp]
        public void Setup()
        { 
            _mockedActivityData = new Mock<IActivityData>();
            _systemUnderTest = new ActivityValidatorService( _mockedActivityData.Object );
        }


        [Test]
        public void ValidatesIfActivityCheckinInCorrectHour_Without_Activity()
        {
            //Arrange
            var fakeActivities = new Result<Activity>((int)HttpStatusCode.BadRequest,"No data found");
            _mockedActivityData.Setup(a => a.GetActivity(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(fakeActivities);

            //Act
            var result = _systemUnderTest.ValidatesIfActivityCheckinInCorrectHour(1, DateTime.Now, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo(fakeActivities.HttpStatusCode));
            Assert.That(result.Message, Is.EqualTo(fakeActivities.Message));
        }

        [Test]
        public void ValidatesIfActivityCheckinInCorrectHour_With_Activity()
        {
            //Arrange
            var fakeActivities = new Result<Activity>((int)HttpStatusCode.OK, "Activity ok");
            fakeActivities.Data = new Activity()
            {
                ActivityId = 1,
                Name = "Test",
                ClassrooomId = "Classroom test",
                StartDateTime = DateTime.Now.AddMinutes(20),
                EndDateTime = DateTime.Now.AddMinutes(100),
            };

            _mockedActivityData.Setup(a => a.GetActivity(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(fakeActivities);

            //Act
            var result = _systemUnderTest.ValidatesIfActivityCheckinInCorrectHour(1, DateTime.Now, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Message, Is.EqualTo("Can't checkin to the activity because you are out of the active time range"));
        }

        [Test]
        public void ValidatesIfActivityCheckinInCorrectHour_With_Activity_And_30_minutes_delay()
        {
            //Arrange
            var fakeActivities = new Result<Activity>((int)HttpStatusCode.OK, "Activity ok");
            fakeActivities.Data = new Activity()
            {
                ActivityId = 1,
                Name = "Test",
                ClassrooomId = "Classroom test",
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddMinutes(120),
            };

            _mockedActivityData.Setup(a => a.GetActivity(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(fakeActivities);

            //Act
            var result = _systemUnderTest.ValidatesIfActivityCheckinInCorrectHour(1, DateTime.Now.AddMinutes(31), It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Message, Is.EqualTo("Can't checkin to the activity because it started more than 30 minutes ago"));
        }

        [Test]
        public void ValidatesIfActivityCheckinInCorrectHour_With_Activity_Without_restriction()
        {
            //Arrange
            var fakeActivities = new Result<Activity>((int)HttpStatusCode.OK, "Activity ok");
            fakeActivities.Data = new Activity()
            {
                ActivityId = 1,
                Name = "Test",
                ClassrooomId = "Classroom test",
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddMinutes(120),
            };

            _mockedActivityData.Setup(a => a.GetActivity(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(fakeActivities);

            //Act
            var result = _systemUnderTest.ValidatesIfActivityCheckinInCorrectHour(1, DateTime.Now.AddMinutes(15), It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(result.Message, Is.EqualTo("Validation ok"));
        }

        [Test]
        public void ValidatesDuplicatedCheckinInActivity_Success_No_Activity_Found() 
        {
            //Arrange
            var fakeAttendedActivityResult = new Result<List<AttendedActivities>>((int)HttpStatusCode.OK, "No data found");
            _mockedActivityData.Setup(a => a.GetAttendedActivity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(fakeAttendedActivityResult);

            //Act
            var result = _systemUnderTest.ValidatesDuplicatedCheckinInActivity(1, 1, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo(fakeAttendedActivityResult.HttpStatusCode));
            Assert.That(result.Message, Is.EqualTo("Validation ok"));
        }

        [Test]
        public void ValidatesDuplicatedCheckinInActivity_Failed_No_Activity_Found()
        {
            //Arrange
            var fakeAttendedActivityResult = new Result<List<AttendedActivities>>((int)HttpStatusCode.OK, "Activity ok");
            fakeAttendedActivityResult.Data = new List<AttendedActivities>()
            {
                new AttendedActivities()
                {
                    ActivityId = 1,
                    StudentId = 1,
                    DateTimeRegister = DateTime.Parse("07/22/2023 14:15")
                }
            };

            _mockedActivityData.Setup(a => a.GetAttendedActivity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(fakeAttendedActivityResult);


            var fakeGetActivityReturn = new Result<Activity>((int)HttpStatusCode.OK, "Activity ok");
            fakeGetActivityReturn.Data = new Activity()
            {
                ActivityId = 1,
                Name = "Calculus",
                ClassrooomId = "Classroom1",
                StartDateTime = DateTime.Parse("07/22/2023 14:00"),
                EndDateTime = DateTime.Parse("07/22/2023 15:00"),
                State = "Active"
            };

            _mockedActivityData.Setup(a => a.GetActivity(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(fakeGetActivityReturn);

            //Act
            var result = _systemUnderTest.ValidatesDuplicatedCheckinInActivity(1, 1, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Message, Is.EqualTo("There is already an attendence record for this activity"));
        }

        [Test]
        public void ValidatesDuplicatedCheckinInActivity_Succeded_There_Is_No_Checkin_for_The_Activity()
        {
            //Arrange
            var fakeAttendedActivityResult = new Result<List<AttendedActivities>>((int)HttpStatusCode.OK, "Activity ok");
            fakeAttendedActivityResult.Data = new List<AttendedActivities>();

            _mockedActivityData.Setup(a => a.GetAttendedActivity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(fakeAttendedActivityResult);


            var fakeGetActivityReturn = new Result<Activity>((int)HttpStatusCode.OK, "Activity ok");
            fakeGetActivityReturn.Data = new Activity()
            {
                ActivityId = 1,
                Name = "Calculus",
                ClassrooomId = "Classroom1",
                StartDateTime = DateTime.Parse("07/22/2023 14:00"),
                EndDateTime = DateTime.Parse("07/22/2023 15:00"),
                State = "Active"
            };

            _mockedActivityData.Setup(a => a.GetActivity(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(fakeGetActivityReturn);

            //Act
            var result = _systemUnderTest.ValidatesDuplicatedCheckinInActivity(1, 1, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(result.Message, Is.EqualTo("Validation ok"));
        }

        [Test]
        public void ValidatesDuplicatedCheckinInActivity_Without_Finding_Specific_Activity()
        {
            //Arrange
            var fakeAttendedActivityResult = new Result<List<AttendedActivities>>((int)HttpStatusCode.OK, "Activity ok");
            fakeAttendedActivityResult.Data = new List<AttendedActivities>();

            _mockedActivityData.Setup(a => a.GetAttendedActivity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(fakeAttendedActivityResult);


            var fakeGetActivityReturn = new Result<Activity>((int)HttpStatusCode.BadRequest, "Activity doesn't exist");
            
            _mockedActivityData.Setup(a => a.GetActivity(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(fakeGetActivityReturn);

            //Act
            var result = _systemUnderTest.ValidatesDuplicatedCheckinInActivity(1, 1, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Message, Is.EqualTo("Activity doesn't exist"));
        }
    }
}
