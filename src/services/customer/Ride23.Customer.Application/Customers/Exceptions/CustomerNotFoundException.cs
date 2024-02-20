using Ride23.Framework.Core.Exceptions;

namespace Ride23.Customer.Application.Customers.Exceptions;
internal class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(object customerId) : base($"Customer with ID '{customerId}' is not found.")
    {
    }
}
