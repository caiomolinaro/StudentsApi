using System.ComponentModel.DataAnnotations;

namespace StudentsApi.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "You need enter a email")]
        [EmailAddress(ErrorMessage = "Email format is not valid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "You need enter a password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}