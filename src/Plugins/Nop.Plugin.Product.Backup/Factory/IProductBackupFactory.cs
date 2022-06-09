using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;

namespace Nop.Plugin.Product.Backup.Factory;

public interface IProductBackupFactory
{
    /// <summary>
    /// Prepare product backup model
    /// </summary>
    /// <param name="model">Product backup model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the webhook settings model
    /// </returns>
    Task<IList<ProductModel>> PrepareProductBackupModel();

    /// <summary>
    /// Prepare product backup settings model
    /// </summary>
    /// <param name="model">Product backup model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the webhook settings model
    /// </returns>
    Task<ProductBackupSettingsModel> PrepareProductBackupSettingsModelAsync(ProductBackupSettingsModel model = null);
}