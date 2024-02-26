using Ride23.Driver.Application.Drivers.Dtos;
using Ride23.Driver.Application.Drivers.Exceptions;
using MapsterMapper;
using MediatR;

namespace Ride23.Driver.Application.Drivers.Features;
public static class GetDriverDetails
{
    public sealed record Query : IRequest<DriverDetailsDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, DriverDetailsDto>
    {
        private readonly IDriverRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IDriverRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DriverDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var driver = await _repository.FindByIdAsync(request.Id, cancellationToken) ?? throw new DriverNotFoundException(request.Id);
            var driverDto = _mapper.Map<DriverDetailsDto>(driver);
            return driverDto;
        }
    }
}
