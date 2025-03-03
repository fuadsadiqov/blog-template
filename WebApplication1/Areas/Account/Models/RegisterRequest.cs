
using System.ComponentModel.DataAnnotations;

namespace GP.MVC.Areas.Account.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email tələb olunur")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Fullname tələb olunur")]
        public string FullNameAz { get; set; }
        [Required(ErrorMessage = "Şifrə tələb olunur")]
        [MinLength(8, ErrorMessage = "Şifrə minimum 8 simvoldan ibarət olmalıdır")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Şifrə təsdiqi minimum 8 simvoldan ibarət olmalıdır")]
        public string ConfirmPassword { get; set; }
    }
}
