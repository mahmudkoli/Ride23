using Ride23.Order.Application.Orders.Dtos;
using Ride23.Order.Application.Orders.Exceptions;
using MapsterMapper;
using MediatR;

namespace Ride23.Order.Application.Orders.Features;
public static class GetOrderDetails
{
    public sealed record Query : IRequest<OrderDetailsDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, OrderDetailsDto>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var order = await _repository.FindByIdAsync(request.Id, cancellationToken) ?? throw new OrderNotFoundException(request.Id);
            var orderDto = _mapper.Map<OrderDetailsDto>(order);
            return orderDto;
        }
    }
}
