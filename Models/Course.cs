using System.Collections.Generic;

namespace StudentApp.API.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseType { get; set; }
        public int TeacherId { get; set; }
        public string Semester { get; set; }
        public string Faculty { get; set; }

        public string Specialization { get; set; }        
        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Assignation> Assignations { get; set; }
    }
}