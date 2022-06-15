using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Product.Backup.Models.Settings;

public record ProductBackupSettingsModel : BaseNopModel, ISettingsModel
{
    [NopResourceDisplayName("Admin.Configuration.Settings.Product.Backup.Enabled")]
    public bool BackupConfigurationEnabled { get; set; }
    public bool BackupConfigurationEnabled_OverrideForStore { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Settings.Product.Backup.Number")]
    public int ProcessingProductsNumber { get; set; }
    public bool ProcessingProductsNumber_OverrideForStore { get; set; }
    
    [NopResourceDisplayName("Admin.Configuration.Settings.Product.Backup.Timer")]
    public int ProductBackupTimer { get; set; }
    public bool ProductBackupTimer_OverrideForStore { get; set; }
    
    [NopResourceDisplayName("Admin.Configuration.Settings.Product.Backup.StoragePath")]
    public string ProductBackupStoragePath { get; set; }
    public bool ProductBackupStoragePath_OverrideForStore { get; set; }
    
    public int ActiveStoreScopeConfiguration { get; set; }
}