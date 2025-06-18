using Brokers.IBKR.Client.Configuration;
using Brokers.IBKR.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IBKR.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIBKRClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IBKROptions>(configuration.GetSection(IBKROptions.SectionName));

        services.AddHttpClient<IBKRClient>()
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            });

        return services;
    }
}