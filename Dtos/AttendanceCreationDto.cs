using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Dtos
{
    public class AttendanceCreationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }
        public AttendanceCreationDto()
        {
            Date = DateTime.Now;
        }
    }
}

