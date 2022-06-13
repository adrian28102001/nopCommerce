using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Product.Backup.Services.Product;

public interface IProductService
{
    public Task<List<Core.Domain.Catalog.Product>> GetNextProductsToExport();
}