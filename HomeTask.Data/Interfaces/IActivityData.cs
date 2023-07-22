using HomeTask.Data.Models;
using HomeTask.Utils;

namespace HomeTask.Data.Interfaces
{
    public interface IActivityData
    {
        /// <summary>
        ///     Gets the activities available according to the classroomId
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Result<List<Activity>>> GetActivities(string classRoomId, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Result<Activity>> GetActivity(int activityId, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="studentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Result<List<AttendedActivities>> GetAttendedActivity(int activityId, int studentId, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="studentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Result<AttendedActivities>> CreateNewAttendence(int activityId, int studentId, CancellationToken token);
    }
}
