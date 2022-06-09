using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Product.Backup.Services;

public interface IProductService
{
    /// <summary>
    /// Get first five unexported products
    /// </summary>
    /// <param name="product">Product</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public Task<List<Core.Domain.Catalog.Product>> GetFiveUnexportedProductsAsync();
}