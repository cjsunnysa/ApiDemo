using System;

namespace ApiDemo.Api.Common
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string entityName, string identifier)
            : base($"{entityName} ({identifier}) was not found.")
        {
        }
    }
}
