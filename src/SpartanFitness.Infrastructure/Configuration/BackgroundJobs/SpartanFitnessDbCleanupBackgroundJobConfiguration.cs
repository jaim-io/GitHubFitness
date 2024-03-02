using Microsoft.Extensions.Options;

using Quartz;

using SpartanFitness.Infrastructure.BackgroundJobs;

namespace SpartanFitness.Infrastructure.Configuration.BackgroundJobs;

public class SpartanFitnessDbCleanupBackgroundJobConfiguration : IConfigureOptions<QuartzOptions>
{
  public void Configure(QuartzOptions options)
  {
    var jobKey = JobKey.Create(nameof(SpartanFitnessDbCleanupBackgroundJob));
    options
      .AddJob<SpartanFitnessDbCleanupBackgroundJob>(builder => builder.WithIdentity(jobKey))
      .AddTrigger(trigger => 
          trigger
            .ForJob(jobKey)
            .WithCronSchedule("0 8 * * 0"));
  }
}