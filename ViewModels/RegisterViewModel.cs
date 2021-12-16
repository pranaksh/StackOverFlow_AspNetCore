using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Stack.ViewModels
{
        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Username Can't be blank")]
            public string Username { get; set; }
            [Required(ErrorMessage = "Password Can't be blank")]
            public string Password { get; set; }
            [Required(ErrorMessage = "Confirm Password Can't be blank")]
            [Compare("Password", ErrorMessage = "Password does not match")]
            public string ConfirmPassword { get; set; }
            [Required(ErrorMessage = "Email Can't be blank")]
            [EmailAddress(ErrorMessage = "Invalid Email")]
            public string Email { get; set; }
            public string Mobile { get; set; }
        }
    }
