using StudentApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime LastActive { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Faculty { get; set; }

        public string Specialization { get; set; }

        public int Year { get; set; }

        public int Group { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhotoUrl { get; set; }

        public ICollection<PhotoForDetailedDto> Photos { get; set; }
    }
}
