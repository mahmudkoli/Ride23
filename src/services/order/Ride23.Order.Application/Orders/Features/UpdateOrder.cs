using MapsterMapper;
using MediatR;
using Ride23.Order.Application.Orders.Dtos;
using Ride23.Order.Application.Orders.Exceptions;

namespace Ride23.Order.Application.Orders.Features;
public static class UpdateOrder
{
    public sealed record Command : IRequest<OrderDto>
    {
        public readonly UpdateOrderDto UpdateOrderDto;
        public readonly Guid Id;
        public Command(UpdateOrderDto updateOrderDto, Guid id)
        {
            UpdateOrderDto = updateOrderDto;
            Id = id;
        }
    }
    public sealed class Handler : IRequestHandler<Command, OrderDto>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var productToBeUpdated = await _repository.FindByIdAsync(request.Id, cancellationToken) ?? throw new OrderNotFoundException(request.Id);
            productToBeUpdated.Update(
                request.UpdateOrderDto.Status);

            await _repository.UpdateAsync(productToBeUpdated, cancellationToken);
            return _mapper.Map<OrderDto>(productToBeUpdated);
        }
    }
}
