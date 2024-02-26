using FluentValidation;
using MapsterMapper;
using MediatR;
using MSFA23.Application.Common.Persistence;
using Ride23.Driver.Application.Drivers.Dtos;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(
            IDriverRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<DriverDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var driverToAdd = Cust.Driver.Create(
                Guid.Empty,
                request.AddDriverDto.Name);

            await _repository.AddAsync(driverToAdd, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var data = _mapper.Map<DriverDto>(driverToAdd);
            return data;
        }
    }
}
