using HomeTask.Data.Interfaces;
using HomeTask.Data.Models;
using HomeTask.Utils;
using System.Net;

namespace HomeTask.Data
{
    public class ActivityData : IActivityData
    {
        private static List<AttendedActivities> _mockedAttendenceActivity = MockedAttendedActivitie();

        public ActivityData() 
        {

        }

        /// <inheritdoc/>
        public async Task<Result<List<Activity>>> GetActivities(string classRoomId, CancellationToken token)
        {
            var activities = await MockedActivitiesData();

            var filteredActivities = activities.Where(a => a.ClassrooomId.Equals(classRoomId));

            if(filteredActivities != null && filteredActivities.Count() > 0)
            {
                var result = new Result<List<Activity>>((int)HttpStatusCode.OK, "Activities ok");
                result.Data = filteredActivities.ToList();
                return result;
            }

            return new Result<List<Activity>>((int)HttpStatusCode.BadRequest, "No data found");
        }

        /// <inheritdoc/>
        public async Task<Result<Activity>> GetActivity(int activityId, CancellationToken token)
        {
            var activities = await MockedActivitiesData();

            var filteredActivities = activities.FirstOrDefault(a => a.ActivityId.Equals(activityId));

            if (filteredActivities != null)
            {
                var result = new Result<Activity>((int)HttpStatusCode.OK, "Activities ok");
                result.Data = filteredActivities;
                return result;
            }

            return new Result<Activity>((int)HttpStatusCode.BadRequest, "No data found");
        }

        public Result<List<AttendedActivities>> GetAttendedActivity(int activityId, int studentId, CancellationToken token)
        {
                var attendedActivity = _mockedAttendenceActivity.Where(a => a.ActivityId == activityId && a.StudentId == studentId);
                if (attendedActivity != null && attendedActivity.Any())
                {
                    var result = new Result<List<AttendedActivities>>((int)HttpStatusCode.OK, "Attended activity ok");
                    result.Data = attendedActivity.ToList();
                    return result;
                }

                return new Result<List<AttendedActivities>>((int)HttpStatusCode.OK, "No data found");
        }

        public async Task<Result<AttendedActivities>> CreateNewAttendence(int activityId, int studentId, CancellationToken token)
        {
            var newAttendence = new AttendedActivities()
            { 
                ActivityId = activityId,
                StudentId = studentId,
                DateTimeRegister = DateTime.Now
            };

            await Task.Run(() =>
            {
                _mockedAttendenceActivity.Add(newAttendence);
            });

            var result = new Result<AttendedActivities>((int)HttpStatusCode.OK, "Insert completed");
            result.Data = newAttendence;
            return result;
        }

        private async Task<List<Activity>> MockedActivitiesData()
        {
            var result = new List<Activity>();

            await Task.Run(() =>
            {
                result.Add(new Activity()
                {
                    ActivityId = 1,
                    Name = "Calculus",
                    ClassrooomId = "Classroom1",
                    StartDateTime = DateTime.Parse("07/22/2023 14:00"),
                    EndDateTime = DateTime.Parse("07/22/2023 15:00"),
                    State = "Active"
                });

                result.Add(new Activity()
                {
                    ActivityId = 2,
                    Name = "Calculus - 2",
                    ClassrooomId = "Classroom1",
                    StartDateTime = DateTime.Parse("07/21/2023 10:00"),
                    EndDateTime = DateTime.Parse("07/21/2023 12:00"),
                    State = "Active"
                });

                result.Add(new Activity()
                {
                    ActivityId = 3,
                    Name = "Mechanical Physics",
                    ClassrooomId = "Classroom2",
                    StartDateTime = DateTime.Parse("07/22/2023 12:00"),
                    EndDateTime = DateTime.Parse("07/22/2023 13:00"),
                    State = "Active"
                });

            });

            return result;
        }

        private static List<AttendedActivities> MockedAttendedActivitie()
        {
            var result = new List<AttendedActivities>();

                result.Add(new AttendedActivities()
                {
                    ActivityId = 1,
                    StudentId = 1,
                    DateTimeRegister = DateTime.Parse("07/22/2023 10:15")
                });
                result.Add(new AttendedActivities()
                {
                    ActivityId = 2,
                    StudentId = 1,
                    DateTimeRegister = DateTime.Parse("07/21/2023 10:10")
                });
                result.Add(new AttendedActivities()
                {
                    ActivityId = 3,
                    StudentId = 2,
                    DateTimeRegister = DateTime.Parse("07/21/2023 12:05")
                });

            return result;
        }
    }
}
