using Ride23.Customer.Application.Customers.Dtos;
using Cust = Ride23.Customer.Domain.Customers;
using FluentValidation;
using Ride23.Framework.Core.Events;
using MapsterMapper;
using MediatR;
using Ride23.Framework.Core.Messaging;

namespace Ride23.Customer.Application.Customers.Features;
public static class AddCustomer
{
    public sealed record Command : IRequest<CustomerDto>
    {
        public readonly AddCustomerDto AddCustomerDto;
        public Command(AddCustomerDto addCustomerDto)
        {
            AddCustomerDto = addCustomerDto;
        }
    }
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(ICustomerRepository _repository)
        {
            RuleFor(p => p.AddCustomerDto.Name)
                .NotEmpty()
                .MaximumLength(75)
                .WithName("Name");
        }
    }
    public sealed class Handler : IRequestHandler<Command, CustomerDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        //private readonly IEventPublisher _eventBus;
        private readonly IKafkaMessageBus<string, CustomerDto> _message;

        public Handler(ICustomerRepository repository, IMapper mapper
            , IKafkaMessageBus<string, CustomerDto> message
            //, IEventPublisher eventBus
            )
        {
            _repository = repository;
            _mapper = mapper;
            _message = message;
            //_eventBus = eventBus;
        }

        public async Task<CustomerDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var customerToAdd = Cust.Customer.Create(
                Guid.Empty,
                request.AddCustomerDto.Name);

            await _repository.AddAsync(customerToAdd, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            //foreach (var @event in customerToAdd.DomainEvents)
            //{
            //    await _eventBus.PublishAsync(@event, token: cancellationToken);
            //}
            var data = _mapper.Map<CustomerDto>(customerToAdd);
            await _message.PublishAsync(data.Id.ToString(), data);
            return data;
        }
    }
}
