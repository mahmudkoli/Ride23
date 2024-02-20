using Ride23.Framework.Core.Exceptions;
using System.Net;

namespace Ride23.Identity.Application.Users.Exceptions;
public class UserRegistrationException : CustomException
{
    public UserRegistrationException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, statusCode)
    {
    }
}
