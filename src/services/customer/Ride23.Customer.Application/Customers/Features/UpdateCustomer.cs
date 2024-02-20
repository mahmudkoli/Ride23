using MapsterMapper;
using MediatR;
using Ride23.Customer.Application.Customers.Dtos;
using Ride23.Customer.Application.Customers.Exceptions;

namespace Ride23.Customer.Application.Customers.Features;
public static class UpdateCustomer
{
    public sealed record Command : IRequest<CustomerDto>
    {
        public readonly UpdateCustomerDto UpdateCustomerDto;
        public readonly Guid Id;
        public Command(UpdateCustomerDto updateCustomerDto, Guid id)
        {
            UpdateCustomerDto = updateCustomerDto;
            Id = id;
        }
    }
    public sealed class Handler : IRequestHandler<Command, CustomerDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public Handler(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var productToBeUpdated = await _repository.FindByIdAsync(request.Id, cancellationToken) ?? throw new CustomerNotFoundException(request.Id);
            productToBeUpdated.Update(
                request.UpdateCustomerDto.Name);

            await _repository.UpdateAsync(productToBeUpdated, cancellationToken);
            return _mapper.Map<CustomerDto>(productToBeUpdated);
        }
    }
}
