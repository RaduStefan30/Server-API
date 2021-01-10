using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Dtos
{
    public class UserForRegistrationDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(32, MinimumLength =4, ErrorMessage ="The password lenght must be between 4 and 32 characters!")]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Faculty { get; set; }
        [Required]
        public string Specialization { get; set; }

        [Required]
        public int Group { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public DateTime DateOfBirth  { get; set; }

        public DateTime LastActive  { get; set; }

        public UserForRegistrationDto()
        {
            LastActive = DateTime.Now;
        }

    }
}
