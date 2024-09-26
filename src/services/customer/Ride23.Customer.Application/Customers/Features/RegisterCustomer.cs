using FluentValidation;
using MapsterMapper;
using MediatR;
using MSFA23.Application.Common.Persistence;
using Ride23.Customer.Application.Common;
using Ride23.Customer.Application.Customers.Dtos;
using Ride23.Customer.Application.Customers.Exceptions;
using Ride23.Customer.Domain.Customers;
using Ride23.Framework.Core.Exceptions;
using Cust = Ride23.Customer.Domain.Customers;

namespace Ride23.Customer.Application.Customers.Features
{
    public class RegisterCustomer
    {
        public sealed class Command : IRequest<CustomerDto>
        {
            public readonly RegisterCustomerDto RegisterCustomer;

            public Command(RegisterCustomerDto registerCustomerDto)
            {
                RegisterCustomer = registerCustomerDto;
            }
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ICustomerRepository _repository)
            {
                RuleFor(p => p.RegisterCustomer.Name)
                    .NotEmpty()
                    .MaximumLength(75)
                    .WithName("Name")
                    .WithMessage("Name should be between 2 and 100 characters.");

                RuleFor(p => p.RegisterCustomer.PhoneNumber)
                    .NotEmpty()
                    .Length(11)
                    .Matches(@"^01\d{9}$")
                    .WithMessage("Phone number should be a valid number with 11 digits.");

                RuleFor(p => p.RegisterCustomer.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("Invalid email address format.");

                RuleFor(p => p.RegisterCustomer.Password)
                    .NotEmpty()
                    .MinimumLength(6)
                    .WithMessage("Password should be at least 6 characters long.");

                RuleFor(p => p.RegisterCustomer.Address)
                    .NotNull()
                    .WithMessage("Address is required.");

            }
        }

        public sealed class Handler : IRequestHandler<Command, CustomerDto>
        {
            private readonly ICustomerRepository _repository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly IUserService _userService;

            public Handler(
            ICustomerRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserService userService)
            {
                _repository = repository;
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userService = userService;
            }

            public async Task<CustomerDto> Handle(Command request, CancellationToken cancellationToken)
            {
                string userId;
                try
                {
                    userId = await _userService.CreateUserAsync(
                        request.RegisterCustomer.Name,
                        request.RegisterCustomer.Name,
                        request.RegisterCustomer.Email,
                        request.RegisterCustomer.Password,
                        request.RegisterCustomer.PhoneNumber
                    );
                }
                catch (GrpcException ex)
                {
                    throw new CustomerRegistrationException(ex.Message);
                }

                var address = new Address(request.RegisterCustomer.Address.Street,
                                          request.RegisterCustomer.Address.City,
                                          request.RegisterCustomer.Address.PostalCode,
                                          request.RegisterCustomer.Address.Country);


                var customerToRegister = Cust.Customer.Create(userId,
                                                         request.RegisterCustomer.Name,
                                                         address,
                                                         request.RegisterCustomer.PhoneNumber,
                                                         request.RegisterCustomer.ProfilePhoto);

                await _repository.AddAsync(customerToRegister, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var data = _mapper.Map<CustomerDto>(customerToRegister);
                return data;
            }
        }
    }
}
