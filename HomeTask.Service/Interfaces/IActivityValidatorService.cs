using HomeTask.Data.Models;
using HomeTask.Utils;

namespace HomeTask.Service.Interfaces
{
    public interface IActivityValidatorService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="date"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Result<Activity>> ValidatesIfActivityCheckinInCorrectHour(int activityId, DateTime date, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="studentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Result<Activity>> ValidatesDuplicatedCheckinInActivity(int activityId, int studentId, CancellationToken token);

    }
}
