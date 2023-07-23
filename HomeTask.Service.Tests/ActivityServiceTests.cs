using HomeTask.Data.Interfaces;
using HomeTask.Data.Models;
using HomeTask.Service.Interfaces;
using HomeTask.Utils;
using HomeTask.View.Models;
using Moq;
using NUnit.Framework;
using System.Net;

namespace HomeTask.Service.Tests
{
    public class ActivityServiceTests
    {
        public Mock<IActivityValidatorService> _mockedActivityValidatorService;
        public Mock<IStudentValidatorService> _mockedStudentValodatorService;
        public Mock<IActivityData> _mockedActivityData;
        public ActivityService _systemUnderTests;


        [SetUp]
        public void Setup()
        {
            _mockedActivityData = new Mock<IActivityData>();
            _mockedStudentValodatorService = new Mock<IStudentValidatorService>();
            _mockedActivityValidatorService = new Mock<IActivityValidatorService>();
            _systemUnderTests = new ActivityService(_mockedActivityValidatorService.Object, _mockedStudentValodatorService.Object, _mockedActivityData.Object);
        }


        [Test]
        public void ValidatesIfActivityCheckinInCorrectHour_No_Activity()
        {
            //Arrange
            var fakeStudentsPasswordResult = new Result<StudentView>((int)HttpStatusCode.BadRequest, "Fake bad request");
            _mockedStudentValodatorService.Setup(s => s.StudentPasswordValidation(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeStudentsPasswordResult);

            //Act
            var result = _systemUnderTests.ActivityCheckin(1, 1, "1223", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Message, Is.EqualTo(fakeStudentsPasswordResult.Message));
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }


        [Test]
        public void ValidatesIfActivityCheckinInCorrectHour_Hours_Are_out_of_range_for_checkin()
        {
            //Arrange
            var fakeStudentsPasswordResult = new Result<StudentView>((int)HttpStatusCode.OK, "Student ok");
            fakeStudentsPasswordResult.Data = new StudentView()
            {
                StudentId = 1,
                Name = "Test",
                State = "Active"
            };
            _mockedStudentValodatorService.Setup(s => s.StudentPasswordValidation(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeStudentsPasswordResult);

            var fakeActivityValidatorResult = new Result<Activity>((int)HttpStatusCode.BadRequest, "Fake bad request");
            _mockedActivityValidatorService.Setup(a => a.ValidatesIfActivityCheckinInCorrectHour(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivityValidatorResult);

            //Act
            var result = _systemUnderTests.ActivityCheckin(1, 1, "1223", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Message, Is.EqualTo(fakeActivityValidatorResult.Message));
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public void ValidatesIfActivityCheckinInCorrectHour_There_is_Already_A_Checkin_For_this_Activity()
        {
            //Arrange
            var fakeStudentsPasswordResult = new Result<StudentView>((int)HttpStatusCode.OK, "Student ok");
            fakeStudentsPasswordResult.Data = new StudentView()
            {
                StudentId = 1,
                Name = "Test",
                State = "Active"
            };
            _mockedStudentValodatorService.Setup(s => s.StudentPasswordValidation(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeStudentsPasswordResult);

            var fakeActivityValidatorResult = new Result<Activity>((int)HttpStatusCode.OK, "Hours ok");
            fakeActivityValidatorResult.Data = new Activity()
            {
                ActivityId = 1,
                Name = "Calculus",
                ClassrooomId = "Classroom1",
                StartDateTime = DateTime.Parse("07/22/2023 14:00"),
                EndDateTime = DateTime.Parse("07/22/2023 15:00"),
                State = "Active"
            };

            _mockedActivityValidatorService.Setup(a => a.ValidatesIfActivityCheckinInCorrectHour(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivityValidatorResult);


            var fakeDuplicatedCheckin = new Result<Activity>((int)HttpStatusCode.BadRequest, "Fake bad request");
            _mockedActivityValidatorService.Setup(a => a.ValidatesDuplicatedCheckinInActivity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeDuplicatedCheckin);

            //Act
            var result = _systemUnderTests.ActivityCheckin(1, 1, "1223", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Message, Is.EqualTo(fakeDuplicatedCheckin.Message));
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public void ValidatesIfActivityCheckinInCorrectHour_Insert_New_Checkin_Failed()
        {
            //Arrange
            var fakeStudentsPasswordResult = new Result<StudentView>((int)HttpStatusCode.OK, "Student ok");
            fakeStudentsPasswordResult.Data = new StudentView()
            {
                StudentId = 1,
                Name = "Test",
                State = "Active"
            };
            _mockedStudentValodatorService.Setup(s => s.StudentPasswordValidation(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeStudentsPasswordResult);

            var fakeActivityValidatorResult = new Result<Activity>((int)HttpStatusCode.OK, "Hours ok");
            fakeActivityValidatorResult.Data = new Activity()
            {
                ActivityId = 1,
                Name = "Calculus",
                ClassrooomId = "Classroom1",
                StartDateTime = DateTime.Parse("07/22/2023 14:00"),
                EndDateTime = DateTime.Parse("07/22/2023 15:00"),
                State = "Active"
            };

            _mockedActivityValidatorService.Setup(a => a.ValidatesIfActivityCheckinInCorrectHour(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivityValidatorResult);


            var fakeDuplicatedCheckin = new Result<Activity>((int)HttpStatusCode.OK, "Duplicated ok");
            _mockedActivityValidatorService.Setup(a => a.ValidatesDuplicatedCheckinInActivity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeDuplicatedCheckin);

            var fakeCreationOfNewAttendence = new Result<AttendedActivities>((int)HttpStatusCode.BadRequest, "Fake bad request");
            _mockedActivityData.Setup(a => a.CreateNewAttendence(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeCreationOfNewAttendence);

            //Act
            var result = _systemUnderTests.ActivityCheckin(1, 1, "1223", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Message, Is.EqualTo(fakeCreationOfNewAttendence.Message));
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public void ValidatesIfActivityCheckinInCorrectHour_Insert_New_Checkin_Succeded()
        {
            //Arrange
            var fakeStudentsPasswordResult = new Result<StudentView>((int)HttpStatusCode.OK, "Student ok");
            fakeStudentsPasswordResult.Data = new StudentView()
            {
                StudentId = 1,
                Name = "Test",
                State = "Active"
            };
            _mockedStudentValodatorService.Setup(s => s.StudentPasswordValidation(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeStudentsPasswordResult);

            var fakeActivityValidatorResult = new Result<Activity>((int)HttpStatusCode.OK, "Hours ok");
            fakeActivityValidatorResult.Data = new Activity()
            {
                ActivityId = 1,
                Name = "Calculus",
                ClassrooomId = "Classroom1",
                StartDateTime = DateTime.Parse("07/22/2023 14:00"),
                EndDateTime = DateTime.Parse("07/22/2023 15:00"),
                State = "Active"
            };

            _mockedActivityValidatorService.Setup(a => a.ValidatesIfActivityCheckinInCorrectHour(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivityValidatorResult);


            var fakeDuplicatedCheckin = new Result<Activity>((int)HttpStatusCode.OK, "Duplicated ok");
            _mockedActivityValidatorService.Setup(a => a.ValidatesDuplicatedCheckinInActivity(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeDuplicatedCheckin);

            var fakeCreationOfNewAttendence = new Result<AttendedActivities>((int)HttpStatusCode.OK, "Creation ok");
            _mockedActivityData.Setup(a => a.CreateNewAttendence(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeCreationOfNewAttendence);

            //Act
            var result = _systemUnderTests.ActivityCheckin(1, 1, "1223", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Message, Is.EqualTo("Attendence ok"));
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public void GetActivities_Failed()
        {
            //Arrange
            var fakeActivities = new Result<List<Activity>>((int)HttpStatusCode.BadRequest, "Fake bad request");
            _mockedActivityData.Setup(a => a.GetActivities(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivities);

            //Act
            var result = _systemUnderTests.GetActivities("1223", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Message, Is.EqualTo(fakeActivities.Message));
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }


        [Test]
        public void GetActivities_Succeded()
        {
            //Arrange
            var fakeActivities = new Result<List<Activity>>((int)HttpStatusCode.OK, "Activities ok");
            fakeActivities.Data = new List<Activity>()
            {
                new Activity()
                {
                    ActivityId = 1,
                    Name = "Calculus",
                    ClassrooomId = "Classroom1",
                    StartDateTime = DateTime.Parse("07/22/2023 14:00"),
                    EndDateTime = DateTime.Parse("07/22/2023 15:00"),
                    State = "Active"
                }
            };

            _mockedActivityData.Setup(a => a.GetActivities(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivities);

            //Act
            var result = _systemUnderTests.GetActivities("1223", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Message, Is.EqualTo("Request ok"));
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.IsNotNull(result.Data);
            Assert.That(result.Data.First().Name, Is.EqualTo(fakeActivities.Data.First().Name));
            Assert.That(result.Data.First().Id, Is.EqualTo(fakeActivities.Data.First().ActivityId));
        }

        [Test]
        public void GetActivities_Succeded_With_Empty_Activity()
        {
            //Arrange
            var fakeActivities = new Result<List<Activity>>((int)HttpStatusCode.OK, "Activities ok");
            fakeActivities.Data = new List<Activity>();

            _mockedActivityData.Setup(a => a.GetActivities(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivities);

            //Act
            var result = _systemUnderTests.GetActivities("1223", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Message, Is.EqualTo("Request ok"));
            Assert.That(result.HttpStatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.IsNotNull(result.Data);
        }
    }
}
