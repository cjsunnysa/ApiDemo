using System;

namespace ApiDemo.Api.Domain.Exceptions
{
    public class InvalidTitleException : Exception
    {
        public InvalidTitleException(int id)
            : base($"Title Id {id} is invalid.")
        { }
    }
}
