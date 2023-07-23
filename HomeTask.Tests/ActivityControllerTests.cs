using HomeTask.Controllers;
using HomeTask.Service.Interfaces;
using HomeTask.Utils;
using HomeTask.View.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using System.Net;

namespace HomeTask.Tests
{
    public class ActivityControllerTests
    {
        private Mock<IActivityService> _mockedActivityService;
        private Mock<ProblemDetailsFactory> _mockedProblemDetailsFactory;
        private ActivityController _systemUnderTest;

        [SetUp]
        public void Setup()
        {
            _mockedActivityService = new Mock<IActivityService>();
            _mockedProblemDetailsFactory = new Mock<ProblemDetailsFactory>();

            _systemUnderTest = new ActivityController(_mockedActivityService.Object);
        }

        [Test]
        public void Create_New_Attendence_Record_Failed_Validation()
        {
            //Arrange
            var expectedProblemDetails = new ProblemDetails()
            {
                Title = "Fake problem",
                Detail = "Invalid Checkin"
            };

            _mockedProblemDetailsFactory.Setup(x => x.CreateProblemDetails(It.IsAny<HttpContext>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedProblemDetails);

            _systemUnderTest.ProblemDetailsFactory = _mockedProblemDetailsFactory.Object;

            // Act
            var result = _systemUnderTest.Create(It.IsAny<Checkin>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            var objectResult = result.Result as ObjectResult;
            var problem = objectResult.Value as ProblemDetails;
            Assert.IsNotNull(problem);
            Assert.That(problem.Detail, Is.EqualTo("Invalid Checkin"));
        }

        [Test]
        public void Create_New_Attendence_Record_Failed()
        {
            //Arrange 
            var fakeActivityCheckin = new Result<ActivityView>((int)HttpStatusCode.BadRequest, "Failed to generate check-in");

            _mockedActivityService.Setup(a => a.ActivityCheckin(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivityCheckin);

            var expectedProblemDetails = new ProblemDetails()
            {
                Title = "Fake problem",
                Detail = fakeActivityCheckin.Message
            };

            _mockedProblemDetailsFactory.Setup(x => x.CreateProblemDetails(It.IsAny<HttpContext>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedProblemDetails);

            _systemUnderTest.ProblemDetailsFactory = _mockedProblemDetailsFactory.Object;

            var checkin = new Checkin()
            {
                ActivityId = 1,
                StudentId = 2,
                StudentPassword = "password"
            };

            //Act
            var result = _systemUnderTest.Create(checkin, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            var objectResult = result.Result as ObjectResult;
            var problem = objectResult.Value as ProblemDetails;
            Assert.IsNotNull(problem);
            Assert.That(problem.Detail, Is.EqualTo(fakeActivityCheckin.Message));
        }

        [Test]
        public void Create_New_Attendence_Record_Success()
        {
            //Arrange 
            var fakeActivityCheckin = new Result<ActivityView>((int)HttpStatusCode.OK, "Record ok");

            _mockedActivityService.Setup(a => a.ActivityCheckin(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivityCheckin);

            var checkin = new Checkin()
            {
                ActivityId = 1,
                StudentId = 2,
                StudentPassword = "password"
            };

            //Act
            var result = _systemUnderTest.Create(checkin, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            var objectResult = result.Result as OkObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(fakeActivityCheckin.HttpStatusCode));
        }

        [Test]
        public void Get_Record_Failed_Validation()
        {
            //Arrange
            var expectedProblemDetails = new ProblemDetails()
            {
                Title = "Fake problem",
                Detail = "Invalid Classroom Id record"
            };

            _mockedProblemDetailsFactory.Setup(x => x.CreateProblemDetails(It.IsAny<HttpContext>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedProblemDetails);

            _systemUnderTest.ProblemDetailsFactory = _mockedProblemDetailsFactory.Object;

            // Act
            var result = _systemUnderTest.Get(It.IsAny<string>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            var objectResult = result.Result as ObjectResult;
            var problem = objectResult.Value as ProblemDetails;
            Assert.IsNotNull(problem);
            Assert.That(problem.Detail, Is.EqualTo(expectedProblemDetails.Detail));
        }

        [Test]
        public void Get_Activities_Record_Failed()
        {
            //Arrange 
            var fakeActivitiesResults = new Result<IList<ActivityView>>((int)HttpStatusCode.BadRequest, "Failed to request the activities");

            _mockedActivityService.Setup(a => a.GetActivities(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(fakeActivitiesResults);

            var expectedProblemDetails = new ProblemDetails()
            {
                Title = "Fake problem",
                Detail = fakeActivitiesResults.Message
            };

            _mockedProblemDetailsFactory.Setup(x => x.CreateProblemDetails(It.IsAny<HttpContext>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedProblemDetails);

            _systemUnderTest.ProblemDetailsFactory = _mockedProblemDetailsFactory.Object;

            //Act
            var result = _systemUnderTest.Get("12345", It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            var objectResult = result.Result as ObjectResult;
            var problem = objectResult.Value as ProblemDetails;
            Assert.IsNotNull(problem);
            Assert.That(problem.Detail, Is.EqualTo(fakeActivitiesResults.Message));
        }

        [Test]
        public void Get_Activities_Record_Success()
        {
            //Arrange 
            var fakeActivities = new Result<IList<ActivityView>>((int)HttpStatusCode.OK, "Record ok");
            fakeActivities.Data = new List<ActivityView>()
            {
                new ActivityView()
                {
                    Id = 1,
                    Name = "Calculus",
                    StartDateTime = DateTime.Parse("07/22/2023 14:00"),
                    EndDateTime = DateTime.Parse("07/22/2023 15:00")

                }
            };
            _mockedActivityService.Setup(a => a.GetActivities(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeActivities);

           
            //Act
            var result = _systemUnderTest.Get("12345", It.IsAny<CancellationToken>());

            //Assert
            Assert.IsNotNull(result);
            var objectResult = result.Result as OkObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(fakeActivities.HttpStatusCode));
            Assert.That(objectResult.Value, Is.EqualTo(fakeActivities));
        }
    }
}
