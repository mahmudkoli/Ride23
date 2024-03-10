using MapsterMapper;
using MediatR;
using Ride23.Inventory.Application.Inventories.Dtos;
using Ride23.Inventory.Application.Inventories.Exceptions;

namespace Ride23.Inventory.Application.Inventories.Features;
public static class UpdateInventory
{
    public sealed record Command : IRequest<InventoryDto>
    {
        public readonly UpdateInventoryDto UpdateInventoryDto;
        public readonly Guid Id;
        public Command(UpdateInventoryDto updateInventoryDto, Guid id)
        {
            UpdateInventoryDto = updateInventoryDto;
            Id = id;
        }
    }
    public sealed class Handler : IRequestHandler<Command, InventoryDto>
    {
        private readonly IInventoryRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IInventoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<InventoryDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var productToBeUpdated = await _repository.FindByIdAsync(request.Id, cancellationToken) ?? throw new InventoryNotFoundException(request.Id);
            productToBeUpdated.Update(
                request.UpdateInventoryDto.Status);

            await _repository.UpdateAsync(productToBeUpdated, cancellationToken);
            return _mapper.Map<InventoryDto>(productToBeUpdated);
        }
    }
}
