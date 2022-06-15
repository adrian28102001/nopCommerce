using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Product.Backup.Models.Settings;

namespace Nop.Plugin.Product.Backup.Mapper;

public class AdminMapperConfiguration: Profile, IOrderedMapperProfile
{
    public AdminMapperConfiguration()
    {
        CreateProductBackUpMaps();
    }
    
    protected virtual void CreateProductBackUpMaps() 
    {
        CreateMap<ProductBackupSettings, ProductBackupSettingsModel>()
            .ForMember(model => model.BackupConfigurationEnabled_OverrideForStore,options => options.Ignore())
            .ForMember(model => model.ProcessingProductsNumber_OverrideForStore, options => options.Ignore())
            .ForMember(model => model.ProductBackupTimer_OverrideForStore, options => options.Ignore())
            .ForMember(model => model.ProductBackupStoragePath_OverrideForStore, options => options.Ignore());
        CreateMap<ProductBackupSettingsModel, ProductBackupSettings>();
    }

    public int Order { get; }
}