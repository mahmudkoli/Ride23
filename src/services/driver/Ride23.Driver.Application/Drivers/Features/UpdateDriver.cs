using MapsterMapper;
using MediatR;
using Ride23.Driver.Application.Drivers.Dtos;
using Ride23.Driver.Application.Drivers.Exceptions;

namespace Ride23.Driver.Application.Drivers.Features;
public static class UpdateDriver
{
    public sealed record Command : IRequest<DriverDto>
    {
        public readonly UpdateDriverDto UpdateDriverDto;
        public readonly Guid Id;
        public Command(UpdateDriverDto updateDriverDto, Guid id)
        {
            UpdateDriverDto = updateDriverDto;
            Id = id;
        }
    }
    public sealed class Handler : IRequestHandler<Command, DriverDto>
    {
        private readonly IDriverRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IDriverRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DriverDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var productToBeUpdated = await _repository.FindByIdAsync(request.Id, cancellationToken) ?? throw new DriverNotFoundException(request.Id);
            productToBeUpdated.Update(
                request.UpdateDriverDto.Name);

            await _repository.UpdateAsync(productToBeUpdated, cancellationToken);
            return _mapper.Map<DriverDto>(productToBeUpdated);
        }
    }
}
