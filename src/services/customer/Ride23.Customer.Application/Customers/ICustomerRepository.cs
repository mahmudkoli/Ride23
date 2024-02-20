using Ride23.Customer.Application.Customers.Dtos;
using Cust = Ride23.Customer.Domain.Customers;
using Ride23.Framework.Core.Database;
using Ride23.Framework.Core.Pagination;

namespace Ride23.Customer.Application.Customers;
public interface ICustomerRepository : IRepository<Cust.Customer, Guid>
{
    Task<PagedList<CustomerDto>> GetPagedCustomersAsync<CustomerDto>(CustomersParametersDto parameters, CancellationToken cancellationToken = default);
}