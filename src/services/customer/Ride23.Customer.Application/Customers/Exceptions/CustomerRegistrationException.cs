using Ride23.Framework.Core.Exceptions;
using System.Net;

namespace Ride23.Customer.Application.Customers.Exceptions
{
    public class CustomerRegistrationException : CustomException
    {
        public CustomerRegistrationException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, statusCode)
        {
        }
    }

}


