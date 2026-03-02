using KeyValueStore.Cron;
using KeyValueStore.Cron.Abstractions;
using KeyValueStore.Host;
using KeyValueStore.Host.Application.Services;
using KeyValueStore.Host.Background.Jobs;
using KeyValueStore.Host.Configuration;
using KeyValueStore.Host.Domain;
using KeyValueStore.Host.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddOptions<ConnectionOptions>().BindConfiguration(nameof(ConnectionOptions));
        services.AddOptions<StorageOptions>().BindConfiguration(nameof(StorageOptions));
        services.AddOptions<CronOptions>().BindConfiguration(nameof(CronOptions));

        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        services.AddHostedService<TcpListenerBackgroundService>();
        services.AddHostedService<CronBackgroundService>();

        services.AddSingleton<ITcpListenerService, TcpListenerService>();
        services.AddSingleton<IStorageRepository, StorageRepository>();

        services.AddTransient<IJob, TtlJob>();
    })
    .Build();

await host.RunAsync();
