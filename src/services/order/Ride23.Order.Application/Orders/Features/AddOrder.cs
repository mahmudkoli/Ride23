using FluentValidation;
using MapsterMapper;
using MediatR;
using MSFA23.Application.Common.Persistence;
using Ride23.Order.Application.Orders.Dtos;
using Ride23.Framework.Core.Services;
using Cust = Ride23.Order.Domain.Orders;
using Rebus.Bus;
using Ride23.Saga.Order;

namespace Ride23.Order.Application.Orders.Features;
public static class AddOrder
{
    public sealed record Command : IRequest<OrderDto>
    {
        public readonly AddOrderDto AddOrderDto;
        public Command(AddOrderDto addOrderDto)
        {
            AddOrderDto = addOrderDto;
        }
    }
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IOrderRepository _repository)
        {
            RuleFor(p => p.AddOrderDto.CustomerId)
                .NotEmpty()
                .WithName("CustomerId");

            RuleFor(p => p.AddOrderDto.Amount)
                .NotEmpty()
                .WithName("Amount");
        }
    }
    public sealed class Handler : IRequestHandler<Command, OrderDto>
    {
        private readonly IOrderRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public Handler(
            IOrderRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IBus bus)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _bus = bus;
        }

        public async Task<OrderDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var orderToAdd = Cust.Order.Create(
                request.AddOrderDto.CustomerId,
                request.AddOrderDto.Amount);

            await _repository.AddAsync(orderToAdd, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _bus.Send(new OrderCreatedEvent(orderToAdd.Id));

            var data = _mapper.Map<OrderDto>(orderToAdd);
            return data;
        }
    }
}
