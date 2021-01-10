using StudentApp.API.Models;

namespace StudentApp.API.Dtos
{
    public class CourseToReturnDto
    {
        public int Id { get; set; }

        public string CourseName { get; set; }
        public string CourseType { get; set; }
        public string Teacher { get; set; }
        public string Semester { get; set; }
        public string Faculty { get; set; }

        public string Specialization { get; set; }        
        public string City { get; set; }

        public string Country { get; set; }

    }
}