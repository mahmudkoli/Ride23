using Ride23.Framework.Core.Services;

namespace Ride23.Framework.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
    public DateOnly DateOnlyUtcNow => DateOnly.FromDateTime(DateTimeUtcNow);
}
