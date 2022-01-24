using System;

namespace ApiDemo.Api.Domain.Exceptions
{
    public class InvalidProvinceException : Exception
    {
        public InvalidProvinceException(int id)
            : base($"Province Id {id} is invalid.")
        { }
    }
}
