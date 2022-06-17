using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;

namespace Nop.Plugin.Product.Backup.Factory;

public interface IProductBackupFactory
{
    Task<List<ProductModel>> PrepareProductBackupModel();

}