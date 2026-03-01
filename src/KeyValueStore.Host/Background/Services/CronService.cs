using KeyValueStore.Host.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KeyValueStore.Host.Background.Services;

public class CronService(
    IEnumerable<IJob> jobs,
    IOptionsMonitor<CronOptions> options,
    ILogger<CronService> logger) : BackgroundService
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
