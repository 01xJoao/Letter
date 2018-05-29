using System;
using System.Net;

namespace LetterApp.Core.Exceptions
{
    public class ServerErrorException : Exception
    {
        public HttpStatusCode ResponseStatusCode { get; }

        public ServerErrorException() {}
        public ServerErrorException(string message, HttpStatusCode responseStatusCode) : base(message) => ResponseStatusCode = responseStatusCode;
        public ServerErrorException(string message, Exception inner) : base(message, inner){}
    } 
}
