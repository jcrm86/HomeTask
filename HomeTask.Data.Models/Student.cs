using System.ComponentModel.DataAnnotations;

namespace HomeTask.Data.Models
{
    public class Student
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string State { get; set; }
    }
}
