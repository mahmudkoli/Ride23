using MediatR;

namespace Ride23.Driver.Application.Drivers.Features;
public static class DeleteDriver
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
        private readonly IDriverRepository _repository;

        public Handler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _repository.DeleteByIdAsync(request.Id, cancellationToken);
        }
    }
}
