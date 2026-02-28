using KeyValueStore.Host;
using KeyValueStore.Host.Application.Services;
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

        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        services.AddHostedService<TcpListenerHostedService>();
        services.AddSingleton<ITcpListenerService, TcpListenerService>();
        services.AddSingleton<IStorageRepository, StorageRepository>();
    })
    .Build();

await host.RunAsync();
