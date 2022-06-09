using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Product.Backup.BackgroundTask;
using Nop.Plugin.Product.Backup.Factory;

namespace Nop.Plugin.Product.Backup.Infrastructure;

public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductBackupFactory, ProductBackupFactory>();
        services.AddHostedService<BackgroundExport>();
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order { get; }
}