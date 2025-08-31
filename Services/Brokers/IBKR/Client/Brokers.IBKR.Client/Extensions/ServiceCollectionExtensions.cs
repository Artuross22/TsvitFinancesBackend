using Brokers.IBKR.Client.Configuration;
using Brokers.IBKR.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IBKR.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIBKRClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IBKROptions>(configuration.GetSection("IBKR"));
        services.AddScoped<PaperTradingDemoService>();
        services.AddScoped<IBKRPaperTradingService>();
        services.AddScoped<IBKRClient>();

        services.AddHttpClient<IBKRClient>((serviceProvider, client) =>
        {
            ConfigureHttpClient(serviceProvider, client);
        })
        .ConfigurePrimaryHttpMessageHandler(serviceProvider => CreateHttpHandler(serviceProvider));

        services.AddHttpClient<IBKRPaperTradingService>((serviceProvider, client) =>
        {
            ConfigureHttpClient(serviceProvider, client);
        })
        .ConfigurePrimaryHttpMessageHandler(serviceProvider => CreateHttpHandler(serviceProvider));

        return services;
    }

    private static void ConfigureHttpClient(IServiceProvider serviceProvider, HttpClient client)
    {
        var options = serviceProvider.GetRequiredService<IOptions<IBKROptions>>().Value;
        client.BaseAddress = new Uri(options.BaseUrl);
        client.Timeout = TimeSpan.FromMinutes(options.TimeoutMinutes);
        client.DefaultRequestHeaders.Add("User-Agent", options.UserAgent);
    }

    private static HttpClientHandler CreateHttpHandler(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetRequiredService<IOptions<IBKROptions>>().Value;
        
        var handler = new HttpClientHandler();
        if (options.IgnoreSSLErrors)
        {
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        }
        
        return handler;
    }
}