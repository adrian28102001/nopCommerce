using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Product.Backup.BackgroundTask;
using Nop.Plugin.Product.Backup.Factory;
using Nop.Plugin.Product.Backup.Factory.Settings;
using Nop.Plugin.Product.Backup.Mapper;
using Nop.Plugin.Product.Backup.Services.Export;
using Nop.Plugin.Product.Backup.Services.Helpers;
using Nop.Plugin.Product.Backup.Services.Import;
using Nop.Plugin.Product.Backup.Services.Picture;
using Nop.Plugin.Product.Backup.Services.Product;
using Nop.Services.Media;

namespace Nop.Plugin.Product.Backup.Infrastructure;

public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPictureService, PictureService>();
        services.AddScoped<IExportService, ExportService>();
        services.AddScoped<IFileHelper, FileHelper>();
        services.AddScoped<IMapping, Mapping>();
        services.AddScoped<IImportManufacturesFromZip, ImportManufacturesFromZip>();
        services.AddScoped<IProductBackupFactory, ProductBackupFactory>();
        services.AddScoped<IProductBackupConfigSettings, ProductBackupConfigSettings>();
        services.AddScoped<IBackupPictureService, BackupPictureService>();
        services.AddHostedService<BackgroundExport>();
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order { get; }
}