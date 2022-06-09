using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Data.Extensions;
using Nop.Plugin.Product.Backup.Models;

namespace Nop.Plugin.Product.Backup.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Core.Domain.Catalog.Product> _productRepository;
    private readonly ProductBackupSettings _productBackupSettings;

    public ProductService(IRepository<Core.Domain.Catalog.Product> productRepository, ProductBackupSettings productBackupSettings)
    {
        _productRepository = productRepository;
        _productBackupSettings = productBackupSettings;
    }
    public async Task<List<Core.Domain.Catalog.Product>> GetFiveUnexportedProductsAsync()
    {
        var query = (from p in _productRepository.Table
            where p.Exported.Equals(false)
            orderby p.Id
            select p).Take(_productBackupSettings.ProcessingProductsNumber);
        var products = await query.ToListAsync();
        return products;
    }
}