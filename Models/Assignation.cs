using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.API.Models
{
    public class Assignation
    {
        [Key]
        [Column(Order=1)]
        public int CourseId { get; set; }

        [Key]
        [Column(Order=2)]
        public int UserId { get; set; }
    }
}