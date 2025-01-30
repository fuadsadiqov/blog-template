namespace GP.Core.Constants
{
    public class RegexConstants
    {

        public const string UserNameRegex = @"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{4,32}$";

        public const string EmailRegex = @"(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

        public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,100}$";
        //public const string PasswordRegex = @"^.{5,100}$";
        public const string LocalPhoneNumberRegex = @"^\+994(50|51|10|55|99|70|77)[0-9]{7}$";
        public const string LanguageKeyRegex = "^[a-zA-Z._]{3,128}$";
    }
}
