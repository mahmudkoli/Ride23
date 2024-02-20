using MediatR;

namespace Ride23.Customer.Application.Customers.Features;
public static class DeleteCustomer
{
    public sealed record Command : IRequest
    {
        public readonly Guid Id;
        public Command(Guid id)
        {
            Id = id;
        }
    }
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly ICustomerRepository _repository;

        public Handler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _repository.DeleteByIdAsync(request.Id, cancellationToken);
        }
    }
}
