using System;
namespace LetterApp.Core.Exceptions
{
    public class WrongCredentialsException : Exception
    {
        public WrongCredentialsException() {}
        public WrongCredentialsException(string message) : base(message) {}
        public WrongCredentialsException(string message, Exception inner) : base(message, inner) {}
    }
}
