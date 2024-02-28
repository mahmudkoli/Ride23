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
                .WithName("Name");
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
            var driverToAdd = Cust.Driver.Create(
                _currentUserService.UserId(),
                request.AddDriverDto.Name);

            await _repository.AddAsync(driverToAdd, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var data = _mapper.Map<DriverDto>(driverToAdd);
            return data;
        }
    }
}
