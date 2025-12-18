using System.ComponentModel.DataAnnotations;

namespace TradingCompany.MVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter a valid username.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 10 characters long.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a valid password.")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}