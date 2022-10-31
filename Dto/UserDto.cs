using System;
using System.ComponentModel.DataAnnotations;

namespace DattingApp.Dto
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8,MinimumLength = 4)]
        public string Password { get; set; }
        [Required]
        public string KnowAs { get; set; }
        [Required]
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

    }
}
