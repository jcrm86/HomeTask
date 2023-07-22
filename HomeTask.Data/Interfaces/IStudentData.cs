using HomeTask.Data.Models;
using HomeTask.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask.Data.Interfaces
{
    public interface IStudentData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Result<Student>> GetStudentData(int studentId, CancellationToken token);
    }
}
