using Ride23.Inventory.Application.Inventories.Dtos;
using Ride23.Framework.Core.Pagination;
using MediatR;

namespace Ride23.Inventory.Application.Inventories.Features;
public static class GetInventories
{
    public sealed record Query : IRequest<PagedList<InventoryDto>>
    {
        public readonly InventoriesParametersDto Parameters;

        public Query(InventoriesParametersDto parameters)
        {
            Parameters = parameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<InventoryDto>>
    {
        private readonly IInventoryRepository _repository;

        public Handler(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<InventoryDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _repository.GetPagedInventoriesAsync<InventoryDto>(request.Parameters, cancellationToken);
        }
    }
}
