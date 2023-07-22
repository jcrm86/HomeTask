using HomeTask.Data.Models;
using HomeTask.Utils;
using HomeTask.View.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask.Service.Interfaces
{
    public interface IStudentValidatorService
    {
        /// <summary>
        ///     Validates if students password is correct
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="studentPassword"></param>
        /// <returns>
        ///     StatusCode 200 if its correct, 500 if it's failing
        /// </returns>
        Task<Result<StudentView>> StudentPasswordValidation(int studentId, string studentPassword, CancellationToken token);
    }
}
