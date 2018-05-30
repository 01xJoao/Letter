using System;
namespace LetterApp.Core.Models
{
    public class RegisterFormModel
    {
        public RegisterFormModel(string firstName = "", string lastName = "", string emailAddress = "", int? phoneNumber = null, string password = "", string verifyPassword = "", bool userAgreed = false)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            Password = password;
            VerifyPassword = verifyPassword;
            UserAgreed = userAgreed;
        }

        public string ReturnValue(int index)
        {
            switch (index)
            {
                case 0: return FirstName;
                case 1: return LastName;
                case 2: return EmailAddress;
                case 3: return PhoneNumber.ToString();
                case 4: return Password;
                case 5: return VerifyPassword;
                default: return string.Empty;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
        public bool UserAgreed { get; set; }
    }
}
