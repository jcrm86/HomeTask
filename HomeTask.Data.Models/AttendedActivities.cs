using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask.Data.Models
{
    public class AttendedActivities
    {
        public int ActivityId { get; set; }

        public int StudentId { get; set; }

        public DateTime DateTimeRegister { get; set; }
    }
}
