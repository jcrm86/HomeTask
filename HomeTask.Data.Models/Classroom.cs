using System.ComponentModel.DataAnnotations;

namespace HomeTask.Data.Models
{
    public class Classroom
    {
        [Required]
        public int ClassroomId { get; set; }

        public string State { get; set; }
    }
}
