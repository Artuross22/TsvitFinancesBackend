﻿using Brokers.IBKR.Client.Configuration;
using Brokers.IBKR.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IBKR.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIBKRClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IBKROptions>(configuration.GetSection(nameof(IBKROptions)));
        services.AddScoped<PaperTradingDemoService>();
        services.AddScoped<IBKRPaperTradingService>();
        services.AddScoped<IBKRClient>();


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