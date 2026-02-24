using KeyValueStore.Host;
using KeyValueStore.Host.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddOptions<ConnectionOptions>().BindConfiguration(nameof(ConnectionOptions));

        services.AddSingleton<ITcpListenerService, TcpListenerService>();
        services.AddHostedService<TcpListenerHostedService>();
    })
    .Build();

await host.RunAsync();
