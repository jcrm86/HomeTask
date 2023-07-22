using System.ComponentModel.DataAnnotations;

namespace HomeTask.Data.Models
{
    public class Activity
    {
        [Required]
        public int ActivityId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ClassrooomId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public string State { get; set; }
    }
}
