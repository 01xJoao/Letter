using System;
namespace LetterApp.Core.Models
{
    public class RegisterFormModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool UserAgreed { get; set; }
    }
}
