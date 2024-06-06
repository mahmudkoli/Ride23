using MapsterMapper;
using MediatR;
using Ride23.Customer.Application.Customers.Dtos;
using Ride23.Customer.Application.Customers.Exceptions;
using Ride23.Customer.Domain.Customers;

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
            var customerToBeUpdated = await _repository.FindByIdAsync(request.Id, cancellationToken)
                    ?? throw new CustomerNotFoundException(request.Id);

            var address = new Address(
                    request.UpdateCustomerDto.Address.Street,
                    request.UpdateCustomerDto.Address.City,
                    request.UpdateCustomerDto.Address.PostalCode,
                    request.UpdateCustomerDto.Address.Country
                );



            customerToBeUpdated.Update(
                   request.UpdateCustomerDto.Name,
                   address,
                   request.UpdateCustomerDto.PhoneNumber,
                   request.UpdateCustomerDto.ProfilePhoto
               );

            await _repository.UpdateAsync(customerToBeUpdated, cancellationToken);
            return _mapper.Map<CustomerDto>(customerToBeUpdated);
        }
    }
}
