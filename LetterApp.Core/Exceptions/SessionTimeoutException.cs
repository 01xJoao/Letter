using System;
namespace LetterApp.Core.Exceptions
{
    public class SessionTimeoutException : Exception
    {
        public SessionTimeoutException()
        {
        }

        public SessionTimeoutException(string message) : base(message)
        {
        }

        public SessionTimeoutException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
