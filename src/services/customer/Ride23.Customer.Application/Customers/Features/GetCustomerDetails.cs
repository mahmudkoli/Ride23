using Ride23.Customer.Application.Customers.Dtos;
using Ride23.Customer.Application.Customers.Exceptions;
using MapsterMapper;
using MediatR;

namespace Ride23.Customer.Application.Customers.Features;
public static class GetCustomerDetails
{
    public sealed record Query : IRequest<CustomerDetailsDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, CustomerDetailsDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public Handler(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var customer = await _repository.FindByIdAsync(request.Id, cancellationToken) ?? throw new CustomerNotFoundException(request.Id);
            var customerDto = _mapper.Map<CustomerDetailsDto>(customer);
            return customerDto;
        }
    }
}
