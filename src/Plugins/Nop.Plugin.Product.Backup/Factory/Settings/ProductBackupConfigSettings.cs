using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Product.Backup.Models.Settings;
using Nop.Services.Configuration;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace Nop.Plugin.Product.Backup.Factory.Settings;

public class ProductBackupConfigSettings : IProductBackupConfigSettings
{
    private readonly IStoreContext _storeContext;
    private readonly ISettingService _settingService;

    public ProductBackupConfigSettings(IStoreContext storeContext, ISettingService settingService)
    {
        _storeContext = storeContext;
        _settingService = settingService;
    }

    public async Task<ProductBackupSettingsModel> PrepareProductBackupSettingsModel(
        ProductBackupSettingsModel model = null)
    {
        //load settings for a chosen store scope
        var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var productBackupSettings =
            await _settingService.LoadSettingAsync<Models.Settings.ProductBackupSettings>(storeId);

        //fill in model values from the entity
        model ??= productBackupSettings.ToSettingsModel<ProductBackupSettingsModel>();

        //fill in additional values (not existing in the entity)
        model.ActiveStoreScopeConfiguration = storeId;

        if (storeId <= 0)
            return model;

        model.BackupConfigurationEnabled_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.BackupConfigurationEnabled, storeId);
        model.ProcessingProductsNumber_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.ProcessingProductsNumber, storeId);
        model.ProductBackupTimer_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.ProductBackupTimer, storeId);
        model.ProductBackupStoragePath_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.ProductBackupStoragePath, storeId);

        return model;
    }
}