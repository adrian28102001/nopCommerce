using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;
using Nop.Plugin.Product.Backup.Models.Settings;

namespace Nop.Plugin.Product.Backup.Factory;

public interface IProductBackupFactory
{
    Task ExportModel();

    Task<List<ProductModel>> PrepareProductBackupModel();

    Task<List<PictureModel>> PrepareImageModel(int id);
    
    Task<ProductBackupSettingsModel> PrepareProductBackupSettingsModelAsync(ProductBackupSettingsModel model = null);
}