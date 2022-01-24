using System;

namespace ApiDemo.Api.Domain.Exceptions
{
    public class InvalidDeliveryAddressException : Exception
    {
        public InvalidDeliveryAddressException(string customerName, int customerId)
            : base($"Customer {customerName} ({customerId}) does not have a valid devlivery address.")
        { }
    }
}
