using HomeTask.Data.Interfaces;
using HomeTask.Data.Models;
using HomeTask.Service.Interfaces;
using HomeTask.Utils;
using HomeTask.View.Models;
using System.Net;

namespace HomeTask.Service
{
    public class ActivityService : IActivityService
    {

        private readonly IActivityValidatorService _activityValidatorService;
        private readonly IStudentValidatorService _studentValidatorService;
        private readonly IActivityData _activityData;

        /// <summary>
        ///     Creates an instance of activity service
        /// </summary>
        /// <param name="activityValidatorService"></param>
        /// <param name="studentValidatorService"></param>
        /// <param name="activityData"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ActivityService(IActivityValidatorService activityValidatorService, IStudentValidatorService studentValidatorService, IActivityData activityData) 
        {
            _activityValidatorService = activityValidatorService ?? throw new ArgumentNullException(nameof(activityValidatorService));
            _studentValidatorService = studentValidatorService ?? throw new ArgumentNullException(nameof(studentValidatorService));
            _activityData = activityData ?? throw new ArgumentNullException(nameof(activityData));
        }

        /// <inheritdoc/>
        public async Task<Result<ActivityView>> ActivityCheckin(int activityId, int studentId, string studentPassword, CancellationToken token)
        {
            //Validate student password
            var studentValidation = await _studentValidatorService.StudentPasswordValidation(studentId, studentPassword, token);

            if (studentValidation.HttpStatusCode != (int)HttpStatusCode.OK)
            { 
                //Student validation didn't pass
                return new Result<ActivityView>(studentValidation.HttpStatusCode, studentValidation.Message);
            }

            //validate hour of check-in
            var hoursValidation = await _activityValidatorService.ValidatesIfActivityCheckinInCorrectHour(activityId, DateTime.Now, token);
            if (hoursValidation.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                //Hours for check-in are out of range
                return new Result<ActivityView>(hoursValidation.HttpStatusCode, hoursValidation.Message);
            }

            //valdate duplicated check-in
            var duplicatedAttendence = await _activityValidatorService.ValidatesDuplicatedCheckinInActivity(activityId, studentId, token);
            if (duplicatedAttendence.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                //There is already a record of attendence for this activity
                return new Result<ActivityView>(duplicatedAttendence.HttpStatusCode, duplicatedAttendence.Message);
            }

            var newAttendenceResult = await _activityData.CreateNewAttendence(activityId, studentId, token);

            if (newAttendenceResult.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                //There is already a record of attendence for this activity
                return new Result<ActivityView>(newAttendenceResult.HttpStatusCode, newAttendenceResult.Message);
            }

            var result = new Result<ActivityView>(newAttendenceResult.HttpStatusCode, "Attendence ok");
            return result;
        }

        /// <inheritdoc/>
        public async Task<Result<IList<ActivityView>>> GetActivities(string classroomId, CancellationToken token)
        {
            var activityDataResult = await _activityData.GetActivities(classroomId, token);

            if (activityDataResult.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                return new Result<IList<ActivityView>>(activityDataResult.HttpStatusCode, activityDataResult.Message);
            }

            var result = new Result<IList<ActivityView>>(activityDataResult.HttpStatusCode, "Request ok");

            result.Data = activityDataResult.Data != null ? MapActivities(activityDataResult.Data) : new List<ActivityView>();

            return result;
        }

        /// <summary>
        ///     Maps from data model to view model
        /// </summary>
        /// <param name="dataActivities"></param>
        /// <returns></returns>
        private IList<ActivityView> MapActivities(List<Activity> dataActivities)
        {
            var result = new List<ActivityView>();
            foreach (var activity in dataActivities)
            {
                var newActivity = new ActivityView
                {
                    Id = activity.ActivityId,
                    Name = activity.Name ?? string.Empty,
                    StartDateTime = activity.StartDateTime,
                    EndDateTime = activity.EndDateTime
                };

                result.Add(newActivity);
            }

            return result;
        }
    }
}
