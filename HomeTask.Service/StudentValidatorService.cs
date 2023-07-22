using HomeTask.Data.Interfaces;
using HomeTask.Data.Models;
using HomeTask.Service.Interfaces;
using HomeTask.Utils;
using HomeTask.View.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask.Service
{
    public class StudentValidatorService : IStudentValidatorService
    {
        private readonly IStudentData _studentData;

        public StudentValidatorService(IStudentData studentData)
        {
            _studentData = studentData ?? throw new ArgumentNullException(nameof(studentData));
        }

        /// <inheritdoc/>
        public async Task<Result<StudentView>> StudentPasswordValidation(int studentId, string studentPassword, CancellationToken token)
        {
            var dataResult = await _studentData.GetStudentData(studentId, token);

            if (dataResult.HttpStatusCode == (int)HttpStatusCode.OK)
            {
                //validates if password is ok
                if (dataResult.Data != null && dataResult.Data.Password.Equals(studentPassword))
                {
                    var result = new Result<StudentView>((int)HttpStatusCode.OK, "Student authenticated ok");
                    result.Data = new StudentView()
                    {
                        StudentId = dataResult.Data.StudentId,
                        Name = dataResult.Data.Name,
                        State = dataResult.Data.State
                    };

                    return result;
                }
                else if(dataResult.Data != null)//returns when password is not ok
                {
                    var result = new Result<StudentView>((int)HttpStatusCode.Unauthorized, "Student authentication failed");

                    return result;
                }
            }
            
            //returns when student is not found
            return new Result<StudentView>(dataResult.HttpStatusCode, dataResult.Message);
        }
    }
}
