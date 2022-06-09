using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Product.Backup.Models;
using Nop.Plugin.Product.Backup.Services;
using Nop.Services.Configuration;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace Nop.Plugin.Product.Backup.Factory;

public class ProductBackupFactory : IProductBackupFactory
{
    private readonly ProductBackupSettings _productBackupSettings;
    private readonly IStoreContext _storeContext;
    private readonly ISettingService _settingService;
    private readonly IProductService _productService;
    private readonly IRepository<Core.Domain.Catalog.Product> _productRepository;

    public ProductBackupFactory(IStoreContext storeContext, ISettingService settingService,
        ProductBackupSettings productBackupSettings, IProductService productService,
        IRepository<Core.Domain.Catalog.Product> productRepository)
    {
        _storeContext = storeContext;
        _settingService = settingService;
        _productBackupSettings = productBackupSettings;
        _productService = productService;
        _productRepository = productRepository;
    }

    public async Task<ProductBackupSettingsModel> PrepareProductBackupSettingsModelAsync(
        ProductBackupSettingsModel model = null)
    {
        //load settings for a chosen store scope
        var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var productBackupSettings = await _settingService.LoadSettingAsync<ProductBackupSettings>(storeId);

        //fill in model values from the entity
        model ??= productBackupSettings.ToSettingsModel<ProductBackupSettingsModel>();

        //fill in additional values (not existing in the entity)
        model.ActiveStoreScopeConfiguration = storeId;

        if (storeId <= 0)
            return model;

        model.BackupConfigurationEnabled_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.BackupConfigurationEnabled, storeId);
        model.ProcessingProductsNumber_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.ProcessingProductsNumber, storeId);
        model.ProductBackupStoragePath_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.ProductBackupStoragePath, storeId);

        return model;
    }

    public async Task<IList<ProductModel>> PrepareProductBackupModel()
    {
        if (_productBackupSettings.BackupConfigurationEnabled)
        {
            var modelList = new List<ProductModel>();
            var models = await _productService.GetFiveUnexportedProductsAsync();

            foreach (var model in models)
            {
                var mappedModel = new ProductModel
                {
                    ProductTypeId = model.ProductTypeId,
                    Name = model.Name,
                    ShortDescription = model.ShortDescription,
                    FullDescription = model.ShortDescription,
                    Sku = model.Sku,
                    StockQuantity = model.StockQuantity,
                    OldPrice = model.OldPrice,
                    Price = model.Price,
                    Exported = model.Exported,
                    CreatedOnUtc = model.CreatedOnUtc,
                    UpdatedOnUtc = model.UpdatedOnUtc,
                };
                modelList.Add(mappedModel);


                await System.IO.File.WriteAllTextAsync(
                    $"{_productBackupSettings.ProductBackupStoragePath}/" + model.Id + ".json",
                    JsonConvert.SerializeObject(mappedModel));
                await using var file =
                    System.IO.File.CreateText($"{_productBackupSettings.ProductBackupStoragePath}/" + model.Id +
                                              ".json");
                var serializer = new JsonSerializer();
                serializer.Serialize(file, mappedModel);
            }

            return modelList;
        }
        return null;
    }
    
}


