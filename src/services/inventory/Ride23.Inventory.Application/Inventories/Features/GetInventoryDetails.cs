using Ride23.Inventory.Application.Inventories.Dtos;
using Ride23.Inventory.Application.Inventories.Exceptions;
using MapsterMapper;
using MediatR;

namespace Ride23.Inventory.Application.Inventories.Features;
public static class GetInventoryDetails
{
    public sealed record Query : IRequest<InventoryDetailsDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, InventoryDetailsDto>
    {
        private readonly IInventoryRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IInventoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<InventoryDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var inventory = await _repository.FindByIdAsync(request.Id, cancellationToken) ?? throw new InventoryNotFoundException(request.Id);
            var inventoryDto = _mapper.Map<InventoryDetailsDto>(inventory);
            return inventoryDto;
        }
    }
}
