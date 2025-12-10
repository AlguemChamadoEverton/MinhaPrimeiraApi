using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Entities
{
    public class User
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
