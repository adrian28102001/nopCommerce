using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Orders.Webhook.Factory;

namespace Nop.Plugin.Orders.Webhook.Infrastructure;

public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IWebhookFactory, WebhookFactory>();
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order { get; }
}