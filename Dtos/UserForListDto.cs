using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Dtos
{
    public class UserForListDto
    {
        public int Id { get; set; }

        public DateTime LastActive { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Faculty { get; set; }

        public string PhotoUrl { get; set; }
    }
}
