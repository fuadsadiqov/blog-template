namespace GP.MVC.Areas.Account.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string FullNameAz { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
