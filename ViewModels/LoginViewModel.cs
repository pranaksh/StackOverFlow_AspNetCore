using System.ComponentModel.DataAnnotations;

namespace Stack.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username can't be blank")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        public string Password { get; set; }
    }
}
