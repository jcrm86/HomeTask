using HomeTask.Data;
using Moq;
using NUnit.Framework;

namespace HomeTasks.Data.Tests
{
    public class StudentDataTests
    {
        public StudentData _systemUnderTests;

        [SetUp]
        public void Setup()
        { 
            _systemUnderTests = new StudentData();
        }

        [Test]
        public void GetStudentData_Return_Results()
        {
            //Arrange & Act
            var result = _systemUnderTests.GetStudentData(1, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.That(result.Data.StudentId, Is.EqualTo(1));
        }
    }
}
