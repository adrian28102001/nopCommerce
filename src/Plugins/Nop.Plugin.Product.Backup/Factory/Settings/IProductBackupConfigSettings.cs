using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models.Settings;

namespace Nop.Plugin.Product.Backup.Factory.Settings;

public interface IProductBackupConfigSettings
{
    Task<ProductBackupSettingsModel> PrepareProductBackupSettingsModel(ProductBackupSettingsModel model = null);
}