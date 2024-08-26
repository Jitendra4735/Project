using System.ComponentModel.DataAnnotations;

namespace WebApi.Utilities.Models
{
    public class UserAccount
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
