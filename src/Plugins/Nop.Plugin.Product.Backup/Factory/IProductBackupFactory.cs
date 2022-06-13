using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;

namespace Nop.Plugin.Product.Backup.Factory;

public interface IProductBackupFactory
{
    Task ExportModel();

    Task<List<ProductModel>> PrepareProductBackupModel();

    Task<List<PictureModel>> PrepareImageModel();
    
    Task<ProductBackupSettingsModel> PrepareProductBackupSettingsModelAsync(ProductBackupSettingsModel model = null);
}