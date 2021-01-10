using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Dtos
{
    public class UserForUpdateDto
    {   
        public string Faculty { get; set; }

        public string Specialization { get; set; }

        public int Year { get; set; }

        public int Group { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}
