using Ride23.Order.Application.Orders.Dtos;
using Ride23.Framework.Core.Pagination;
using MediatR;

namespace Ride23.Order.Application.Orders.Features;
public static class GetOrders
{
    public sealed record Query : IRequest<PagedList<OrderDto>>
    {
        public readonly OrdersParametersDto Parameters;

        public Query(OrdersParametersDto parameters)
        {
            Parameters = parameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<OrderDto>>
    {
        private readonly IOrderRepository _repository;

        public Handler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<OrderDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _repository.GetPagedOrdersAsync<OrderDto>(request.Parameters, cancellationToken);
        }
    }
}
