using System.Net;

namespace Ride23.Framework.Core.Exceptions;
public class GrpcException : CustomException
{
    public GrpcException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, statusCode)
    {
    }
}