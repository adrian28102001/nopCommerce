using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Data.Extensions;
using Nop.Plugin.Product.Backup.Models;

namespace Nop.Plugin.Product.Backup.Services;

public static class ProductServiceExtension
{

    public static async Task<List<Core.Domain.Catalog.Product>> GetFiveUnexportedProductsAsync(
        this Nop.Services.Catalog.IProductService productService,
        IRepository<Core.Domain.Catalog.Product> productRepository,
        ProductBackupSettings productBackupSettings)
    {
        var query = (from p in productRepository.Table
            where p.Exported.Equals(false)
            orderby p.Id
            select p).Take(productBackupSettings.ProcessingProductsNumber);
        var products = await query.ToListAsync();
        return products; 
    }
}