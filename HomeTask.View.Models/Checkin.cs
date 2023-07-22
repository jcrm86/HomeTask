using System.ComponentModel.DataAnnotations;

namespace HomeTask.View.Models
{
    public class Checkin
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int ActivityId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
