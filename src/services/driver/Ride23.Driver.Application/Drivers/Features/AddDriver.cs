using FluentValidation;
using MapsterMapper;
using MediatR;
using MSFA23.Application.Common.Persistence;
using Ride23.Driver.Application.Drivers.Dtos;
using Ride23.Framework.Core.Services;
using Cust = Ride23.Driver.Domain.Drivers;

namespace Ride23.Driver.Application.Drivers.Features;
public static class AddDriver
{
    public sealed record Command : IRequest<DriverDto>
    {
        public readonly AddDriverDto AddDriverDto;
        public Command(AddDriverDto addDriverDto)
        {
            AddDriverDto = addDriverDto;
        }
    }
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IDriverRepository _repository)
        {
            RuleFor(p => p.AddDriverDto.Name)
                .NotEmpty()
                .MaximumLength(75)
                .WithName("Name")
                .WithMessage("Name should be between 2 and 100 characters.");

            RuleFor(p => p.AddDriverDto.PhoneNumber)
                .NotEmpty()
                .Length(11)
                .Matches(@"^01\d{9}$")
                .WithMessage("Phone number should be a valid number with 11 digits.");

            RuleFor(p => p.AddDriverDto.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email address format.");

            RuleFor(p => p.AddDriverDto.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password should be at least 6 characters long.");

            RuleFor(p => p.AddDriverDto.Address)
                .NotNull()
                .WithMessage("Address is required.");

            RuleFor(p => p.AddDriverDto.LicenseNo)
                .NotEmpty()
                .Length(1, 20)
                .WithMessage("License number should be a valid string with a maximum of 20 characters.");

            RuleFor(p => p.AddDriverDto.LicenseExpiryDate)
                .NotEmpty()
                .Must(date => date > DateTime.Now)
                .WithMessage("License expiry date should be a future date.");

            RuleFor(p => p.AddDriverDto.NoOfRides)
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

        public Handler(
            IDriverRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<DriverDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var identityId = "";

            var driverToAdd = Cust.Driver.Create(
                identityId,
                request.AddDriverDto.Name,
                request.AddDriverDto.PhoneNumber,
                new Cust.ValueObjects.Address(request.AddDriverDto.Address.Street, request.AddDriverDto.Address.City,request.AddDriverDto.Address.PostalCode, request.AddDriverDto.Address.Country),
                request.AddDriverDto.LicenseNo,
                request.AddDriverDto.LicenseExpiryDate,
                request.AddDriverDto.NoOfRides,
                request.AddDriverDto.ProfilePhoto);

            await _repository.AddAsync(driverToAdd, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var data = _mapper.Map<DriverDto>(driverToAdd);
            return data;
        }
    }
}
