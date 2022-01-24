using System;

namespace ApiDemo.Api.Common
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        { }
    }
}
