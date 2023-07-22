using HomeTask.Data.Interfaces;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeTask.Data;
using HomeTask.Data.Models;
using HomeTask.Utils;
using System.Net;

namespace HomeTask.Service.Tests
{
    public class StudentValidatorServiceTests
    {
        private Mock<IStudentData> _mockedStudentData;
        private StudentValidatorService _systemUnderTest;

        [SetUp]
        public void Setup()
        {
            _mockedStudentData = new Mock<IStudentData>();
            _systemUnderTest = new StudentValidatorService(_mockedStudentData.Object);
        }

        [Test]
        public void Student_Password_Validation_Failed()
        {
            //Arrange
            var fakeStudent = new Student()
            {
                StudentId = 1,
                Name = "Juan",
                Password = "password1",
                State = "Active"
            };

            var fakeResult = new Result<Student>((int)HttpStatusCode.BadRequest, "Fake bad request");

            _mockedStudentData.Setup(x => x.GetStudentData(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(fakeResult);

            //Act
            var result = _systemUnderTest.StudentPasswordValidation(1, "12234", It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.HttpStatusCode, Is.EqualTo(fakeResult.HttpStatusCode));
            Assert.That(result.Message, Is.EqualTo(fakeResult.Message));
        }

    }
}
