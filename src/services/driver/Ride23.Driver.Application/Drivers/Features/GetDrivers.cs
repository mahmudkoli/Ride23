using Ride23.Driver.Application.Drivers.Dtos;
using Ride23.Framework.Core.Pagination;
using MediatR;

namespace Ride23.Driver.Application.Drivers.Features;
public static class GetDrivers
{
    public sealed record Query : IRequest<PagedList<DriverDto>>
    {
        public readonly DriversParametersDto Parameters;

        public Query(DriversParametersDto parameters)
        {
            Parameters = parameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<DriverDto>>
    {
        private readonly IDriverRepository _repository;

        public Handler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<DriverDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _repository.GetPagedDriversAsync<DriverDto>(request.Parameters, cancellationToken);
        }
    }
}
