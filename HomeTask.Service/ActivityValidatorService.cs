using HomeTask.Data.Interfaces;
using HomeTask.Data.Models;
using HomeTask.Service.Interfaces;
using HomeTask.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask.Service
{
    public class ActivityValidatorService : IActivityValidatorService
    {
        private readonly IActivityData _activityData;


        public ActivityValidatorService(IActivityData activityData)
        { 
            _activityData = activityData ?? throw new ArgumentNullException(nameof(activityData));
        }

        public async Task<Result<Activity>> ValidatesIfActivityCheckinInCorrectHour(int activityId, DateTime date, CancellationToken token)
        {
            //Validates if activity is between the hours
            var resultedActvity = await _activityData.GetActivity(activityId, token);

            if (resultedActvity == null) 
            {
                return new Result<Activity>((int)HttpStatusCode.BadRequest, "No data found");
            }

            if (resultedActvity.Data != null && (resultedActvity.Data.StartDateTime > date || date > resultedActvity.Data.EndDateTime))
            {
                return new Result<Activity>((int)HttpStatusCode.BadRequest, "Can't checkin to the activity because you are out of the active time range");
            }

            //validate that the activity is less than 30 minutes after begining 

            if (resultedActvity.Data != null && date > resultedActvity.Data.StartDateTime.AddMinutes(30))
            {
                return new Result<Activity>((int)HttpStatusCode.BadRequest, "Can't checkin to the activity because it started more than 30 minutes ago");
            }

            return new Result<Activity>((int)HttpStatusCode.OK, "Validation ok");
        }

        public async Task<Result<Activity>> ValidatesDuplicatedCheckinInActivity(int activityId, int studentId, CancellationToken token) 
        {
            //Get attended activity
            var attendedActivity = _activityData.GetAttendedActivity(activityId, studentId, token);

            if (attendedActivity != null && attendedActivity.HttpStatusCode == (int)HttpStatusCode.OK && attendedActivity.Message.Equals("No data found"))
            {
                //No data means that there is no record for this activity
                return new Result<Activity>((int)HttpStatusCode.OK, "Validation ok");
            }

            if (attendedActivity != null && attendedActivity.Data != null && attendedActivity.HttpStatusCode == (int)HttpStatusCode.OK)
            {
                var activity = await _activityData.GetActivity(activityId, token);
                if (activity != null && activity.HttpStatusCode == (int)HttpStatusCode.OK)
                {
                    foreach (var item in attendedActivity.Data)
                    {
                        if (activity.Data.StartDateTime < item.DateTimeRegister && activity.Data.EndDateTime > item.DateTimeRegister)
                        {
                            return new Result<Activity>((int)HttpStatusCode.BadRequest, "There is already an attendence record for this activity");
                        }
                    }
                }
                else
                {
                    return new Result<Activity>((int)HttpStatusCode.BadRequest, "Activity doesn't exist");
                }
            }

            return new Result<Activity>((int)HttpStatusCode.OK, "Validation ok");
        }
    }
}
