using System.ComponentModel.DataAnnotations;

namespace DattingApp.Dto
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
