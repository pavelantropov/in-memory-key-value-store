using KeyValueStore.Cron.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KeyValueStore.Cron;

public class CronBackgroundService(
    IEnumerable<IJob> jobs,
    ILogger<CronBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            foreach (var job in jobs)
            {
                if (job.ShouldRun())
                {
                    job.ExecuteAsync(token);
                }
            }
        }
    }
}
