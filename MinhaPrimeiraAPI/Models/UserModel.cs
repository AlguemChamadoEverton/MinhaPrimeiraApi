using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class UserModel
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
