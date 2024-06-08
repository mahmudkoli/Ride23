using FluentValidation;
using MapsterMapper;
using MediatR;
using MSFA23.Application.Common.Persistence;
using Ride23.Customer.Application.Customers.Dtos;
using Ride23.Customer.Domain.Customers;
using Ride23.Framework.Core.Services;
using Cust = Ride23.Customer.Domain.Customers;

namespace Ride23.Customer.Application.Customers.Features;
public static class AddCustomer
{
    public sealed record Command : IRequest<CustomerDto>
    {
        public readonly AddCustomerDto AddCustomerDto;
        public Command(AddCustomerDto addCustomerDto)
        {
            AddCustomerDto = addCustomerDto;
        }
    }
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(ICustomerRepository _repository)
        {
            RuleFor(p => p.AddCustomerDto.Name)
                .NotEmpty()
                .MaximumLength(75)
                .WithName("Name");
        }
    }
    public sealed class Handler : IRequestHandler<Command, CustomerDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public Handler(
            ICustomerRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<CustomerDto> Handle(Command request, CancellationToken cancellationToken)
        {

            var address = new Address(
                    request.AddCustomerDto.Address.Street,
                    request.AddCustomerDto.Address.City,
                    request.AddCustomerDto.Address.PostalCode,
                    request.AddCustomerDto.Address.Country
                );


            var customerToAdd = Cust.Customer.Create(
                _currentUserService.UserId(),
                request.AddCustomerDto.Name,
                address,
                request.AddCustomerDto.PhoneNumber,
                request.AddCustomerDto.ProfilePhoto);

            await _repository.AddAsync(customerToAdd, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var data = _mapper.Map<CustomerDto>(customerToAdd);
            return data;
        }
    }
}
