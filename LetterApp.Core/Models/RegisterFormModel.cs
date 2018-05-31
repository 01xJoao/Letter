using System;
namespace LetterApp.Core.Models
{
    public class RegisterFormModel
    {
        public string ReturnValue(int index)
        {
            switch (index)
            {
                case 0: return FirstName;
                case 1: return LastName;
                case 2: return EmailAddress;
                case 3: return Password;
                case 4: return VerifyPassword;
                case 5: return PhoneNumber;
                default: return string.Empty;
            }
        }

        public void SetValue(int index, string value)
        {
            switch (index)
            {
                case 0: FirstName = value; break;
                case 1: LastName = value; break;
                case 2: EmailAddress = value; break;
                case 3: Password = value; break;
                case 4: VerifyPassword = value; break;
                case 5: PhoneNumber = value; break;
                default: break;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool UserAgreed { get; set; }
    }
}
