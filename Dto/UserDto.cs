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
    }
}
