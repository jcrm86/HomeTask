using HomeTask.Data.Models;
using HomeTask.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask.Service.Interfaces
{
    public interface IActivityValidatorService
    {

        Task<Result<Activity>> ValidatesIfActivityCheckinInCorrectHour(int activityId, DateTime date, CancellationToken token);

        Task<Result<Activity>> ValidatesDuplicatedCheckinInActivity(int activityId, int studentId, CancellationToken token);

    }
}
