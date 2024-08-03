using Application.Interfaces.Providers;

namespace Infrastructure.Implements.Providers;

public class TimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}