using FluentValidation;
using MapsterMapper;
using MediatR;
using MSFA23.Application.Common.Persistence;
using Ride23.Driver.Application.Common;
using Ride23.Driver.Application.Drivers.Dtos;
using Ride23.Framework.Core.Services;
using Cust = Ride23.Driver.Domain.Drivers;

namespace Ride23.Driver.Application.Drivers.Features
{
    public static class RegisterDriver
    {
        public sealed record Command : IRequest<DriverDto>
        {
            public readonly RegisterDriverDto RegisterDriverDto;
            public Command(RegisterDriverDto registerDriverDto)
            {
                RegisterDriverDto = registerDriverDto;
            }
        }
        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IDriverRepository _repository)
            {
                RuleFor(p => p.RegisterDriverDto.Name)
                    .NotEmpty()
                    .MaximumLength(75)
                    .WithName("Name")
                    .WithMessage("Name should be between 2 and 100 characters.");

                RuleFor(p => p.RegisterDriverDto.PhoneNumber)
                    .NotEmpty()
                    .Length(11)
                    .Matches(@"^01\d{9}$")
                    .WithMessage("Phone number should be a valid number with 11 digits.");

                RuleFor(p => p.RegisterDriverDto.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("Invalid email address format.");

                RuleFor(p => p.RegisterDriverDto.Password)
                    .NotEmpty()
                    .MinimumLength(6)
                    .WithMessage("Password should be at least 6 characters long.");

                RuleFor(p => p.RegisterDriverDto.Address)
                    .NotNull()
                    .WithMessage("Address is required.");

                RuleFor(p => p.RegisterDriverDto.LicenseNo)
                    .NotEmpty()
                    .Length(1, 20)
                    .WithMessage("License number should be a valid string with a maximum of 20 characters.");

                RuleFor(p => p.RegisterDriverDto.LicenseExpiryDate)
                    .NotEmpty()
                    .Must(date => date > DateTime.Now)
                    .WithMessage("License expiry date should be a future date.");

                RuleFor(p => p.RegisterDriverDto.NoOfRides)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Number of rides must be a non-negative integer.");
            }
        }
        public sealed class Handler : IRequestHandler<Command, DriverDto>
        {
            private readonly IDriverRepository _repository;
            private readonly ICurrentUserService _currentUserService;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly IUserService _userService;

            public Handler(
                IDriverRepository repository,
                IMapper mapper,
                IUnitOfWork unitOfWork,
                ICurrentUserService currentUserService,
                IUserService userService
                )
            {
                _repository = repository;
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _currentUserService = currentUserService;
                _userService = userService;
            }

            public async Task<DriverDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var identityId = await _userService.CreateUserAsync(request.RegisterDriverDto.Name, request.RegisterDriverDto.Email, request.RegisterDriverDto.Email, request.RegisterDriverDto.Password, request.RegisterDriverDto.PhoneNumber);

                var driverToAdd = Cust.Driver.Create(
                    identityId,
                    request.RegisterDriverDto.Name,
                    request.RegisterDriverDto.PhoneNumber,
                    new Cust.ValueObjects.Address(request.RegisterDriverDto.Address.Street, request.RegisterDriverDto.Address.City, request.RegisterDriverDto.Address.PostalCode, request.RegisterDriverDto.Address.Country),
                    request.RegisterDriverDto.LicenseNo,
                    request.RegisterDriverDto.LicenseExpiryDate,
                    request.RegisterDriverDto.NoOfRides,
                    request.RegisterDriverDto.ProfilePhoto);

                await _repository.AddAsync(driverToAdd, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var data = _mapper.Map<DriverDto>(driverToAdd);
                return data;
            }
        }
    }
}
