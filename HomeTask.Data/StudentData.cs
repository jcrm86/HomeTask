using HomeTask.Data.Interfaces;
using HomeTask.Data.Models;
using HomeTask.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask.Data
{
    public class StudentData : IStudentData
    {
        /// <summary>
        /// 
        /// </summary>
        public StudentData() { }

        /// <inheritdoc/>
        public async Task<Result<Student>> GetStudentData(int studentId, CancellationToken token) 
        {

            var mockedStudent = await MockedStudenData();

            var filteredResult = mockedStudent.FirstOrDefault(s => s.StudentId == studentId);

            if (filteredResult != null)
            {
                var result = new Result<Student>((int)HttpStatusCode.OK, "Student ok");
                result.Data = filteredResult;
                return result;
            }

            return new Result<Student>((int)HttpStatusCode.BadRequest, "Student not found");
        }


        private async Task<List<Student>> MockedStudenData()
        {
            var result = new List<Student>();

            await Task.Run(() =>
            {
                result.Add(new Student()
                {
                    StudentId = 1,
                    Name = "Juan",
                    Password = "password1",
                    State = "Active"
                });

                result.Add(new Student()
                {
                    StudentId = 2,
                    Name = "Pedro",
                    Password = "password2",
                    State = "Active"
                });

                result.Add(new Student()
                {
                    StudentId = 3,
                    Name = "Manuel",
                    Password = "password3",
                    State = "Inactive"
                });

                result.Add(new Student()
                {
                    StudentId = 4,
                    Name = "Maria",
                    Password = "password4",
                    State = "Active"
                });
            });
            

            return result;
        }
    }
}
