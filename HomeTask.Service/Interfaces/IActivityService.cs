
using HomeTask.Utils;
using HomeTask.View.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask.Service.Interfaces
{
    public interface IActivityService
    {
        /// <summary>
        ///     Request all activities for a specific code
        /// </summary>
        /// <param name="classroomId"></param>
        /// <param name="token">
        /// </param>
        /// <returns></returns>
        Task<Result<IList<ActivityView>>> GetActivities(string classroomId, CancellationToken token);

        /// <summary>
        ///     Checkin to a new activity
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="studentId"></param>
        /// <param name="studentPassword"></param>
        /// <returns></returns>
        Task<Result<ActivityView>> ActivityCheckin(int activityId, int studentId, string studentPassword, CancellationToken token);
    }
}
