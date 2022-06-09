using Nop.Core.Configuration;

namespace Nop.Plugin.Product.Backup.Models;

public class ProductBackupSettings : ISettings
{
    public bool BackupConfigurationEnabled { get; set; }
    public bool BackupConfigurationEnabled_OverrideForStore { get; set; }

    public int ProcessingProductsNumber { get; set; }
    public bool ProcessingProductsNumber_OverrideForStore { get; set; }
    
    public int ProductBackupTimer { get; set; }
    public bool ProductBackupTimer_OverrideForStore { get; set; }
    
    public string ProductBackupStoragePath { get; set; }
    public bool ProductBackupStoragePath_OverrideForStore { get; set; }
}