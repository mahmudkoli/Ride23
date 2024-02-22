using Ride23.Framework.Infrastructure.Options;

namespace Ride23.Framework.Persistence.EFCore;

public class EFCoreOptions : IOptionsRoot
{
    public string DBProvider { get; set; } = null!;
    public string DefaultSchema { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
}
