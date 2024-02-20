using Ride23.Customer.Application.Customers;
using Ride23.Customer.Application.Customers.Dtos;
using Cust = Ride23.Customer.Domain.Customers;
using Ride23.Framework.Core.Pagination;
using Ride23.Framework.Core.Services;
using Ride23.Framework.Persistence.EFCore;

namespace Ride23.Customer.Infrastructure.Repositories;

public class CustomerRepository : EFCoreRepository<Cust.Customer, Guid>, ICustomerRepository
{
    private readonly IEFCoreDbContext _dbContext;
    public CustomerRepository(IEFCoreDbContext context, IDateTimeService dateTimeService) : base(context, dateTimeService)
    {
        _dbContext = context;
    }

    public async Task<PagedList<CustomerDto>> GetPagedCustomersAsync<CustomerDto>(CustomersParametersDto parameters, CancellationToken cancellationToken = default)
    {
        var queryable = _dbContext.DbSet<Cust.Customer>().AsQueryable();
        if (!string.IsNullOrEmpty(parameters.Keyword))
        {
            string keyword = parameters.Keyword.ToLower();
            queryable = queryable.Where(t => t.Name.ToLower().Contains(keyword));
        }
        queryable = queryable.OrderBy(p => p.CreatedOn);
        return await queryable.ApplyPagingAsync<Cust.Customer, CustomerDto>(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }
}
