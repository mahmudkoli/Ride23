using Ride23.Customer.Application.Customers.Dtos;
using Ride23.Framework.Core.Pagination;
using MediatR;

namespace Ride23.Customer.Application.Customers.Features;
public static class GetCustomers
{
    public sealed record Query : IRequest<PagedList<CustomerDto>>
    {
        public readonly CustomersParametersDto Parameters;

        public Query(CustomersParametersDto parameters)
        {
            Parameters = parameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<CustomerDto>>
    {
        private readonly ICustomerRepository _repository;

        public Handler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<CustomerDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _repository.GetPagedCustomersAsync<CustomerDto>(request.Parameters, cancellationToken);
        }
    }
}
