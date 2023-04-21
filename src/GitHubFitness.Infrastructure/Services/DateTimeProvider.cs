using GitHubFitness.Application.Common.Interfaces.Services;

namespace GitHubFitness.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}