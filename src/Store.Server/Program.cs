using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Store.Server;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ITcpListenerService, TcpListenerService>();
        services.AddHostedService<TcpListenerHostedService>();
    })
    .Build();

await host.RunAsync();
