using FluentValidation;
using MapsterMapper;
using MediatR;
using MSFA23.Application.Common.Persistence;
using Ride23.Inventory.Application.Inventories.Dtos;
using Ride23.Framework.Core.Services;
using Cust = Ride23.Inventory.Domain.Inventories;

namespace Ride23.Inventory.Application.Inventories.Features;
public static class AddInventory
{
    public sealed record Command : IRequest<InventoryDto>
    {
        public readonly AddInventoryDto AddInventoryDto;
        public Command(AddInventoryDto addInventoryDto)
        {
            AddInventoryDto = addInventoryDto;
        }
    }
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IInventoryRepository _repository)
        {
            RuleFor(p => p.AddInventoryDto.SupplierId)
                .NotEmpty()
                .WithName("SupplierId");

            RuleFor(p => p.AddInventoryDto.Amount)
                .NotEmpty()
                .WithName("Amount");
        }
    }
    public sealed class Handler : IRequestHandler<Command, InventoryDto>
    {
        private readonly IInventoryRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(
            IInventoryRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<InventoryDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var inventoryToAdd = Cust.Inventory.Create(
                request.AddInventoryDto.SupplierId,
                request.AddInventoryDto.Amount);

            await _repository.AddAsync(inventoryToAdd, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var data = _mapper.Map<InventoryDto>(inventoryToAdd);
            return data;
        }
    }
}
