using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Plugin.Product.Backup.Models;
using Nop.Plugin.Product.Backup.Models.Settings;
using Nop.Plugin.Product.Backup.Services.Picture;
using Nop.Plugin.Product.Backup.Services.Product;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace Nop.Plugin.Product.Backup.Factory;

public class ProductBackupFactory : IProductBackupFactory
{
    private readonly ProductBackupSettings _productBackupSettings;
    private readonly IStoreContext _storeContext;
    private readonly ISettingService _settingService;
    private readonly IProductService _productService;
    private readonly IPictureService _pictureService;
    private readonly IBackupPictureService _backupPictureService;

    public ProductBackupFactory(ProductBackupSettings productBackupSettings, IStoreContext storeContext,
        ISettingService settingService, IProductService productService, IPictureService pictureService,
        IBackupPictureService backupPictureService)
    {
        _productBackupSettings = productBackupSettings;
        _storeContext = storeContext;
        _settingService = settingService;
        _productService = productService;
        _pictureService = pictureService;
        _backupPictureService = backupPictureService;
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
        model.ProductBackupTimer_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.ProductBackupTimer, storeId);
        model.ProductBackupStoragePath_OverrideForStore =
            await _settingService.SettingExistsAsync(productBackupSettings, x => x.ProductBackupStoragePath, storeId);

        return model;
    }

    public async Task<List<ProductModel>> PrepareProductBackupModel()
    {
        if (!_productBackupSettings.BackupConfigurationEnabled) return null;

        var models = await _productService.GetNotExportedProducts();
        var productModelList = new List<ProductModel>();

        foreach (var model in models)
        {
            var pictureModelList = await PrepareImageModel(model.Id);

            var mappedModel = new ProductModel
            {
                Id = model.Id,
                ProductTypeId = model.ProductTypeId,
                Name = model.Name,
                ShortDescription = model.ShortDescription,
                FullDescription = model.ShortDescription,
                Sku = model.Sku,
                StockQuantity = model.StockQuantity,
                OldPrice = model.OldPrice,
                Price = model.Price,
                CreatedOnUtc = model.CreatedOnUtc,
                UpdatedOnUtc = model.UpdatedOnUtc,
                PictureModelList = pictureModelList
            };
            // model.Exported = true;
            productModelList.Add(mappedModel);
        }

        return productModelList;
    }

    public async Task<List<PictureModel>> PrepareImageModel(int id)
    {
        var pictureModelList = new List<PictureModel>();

        var pictures = await _pictureService.GetPicturesByProductIdAsync(id);

        foreach (var picture in pictures)
        {
            var pictureUrl = await _backupPictureService.GetPictureUrl(picture);

            var pictureModel = new PictureModel()
            {
                Id = picture.Id,
                AltAttribute = picture.AltAttribute,
                IsNew = picture.IsNew,
                MimeType = picture.MimeType,
                SeoFilename = picture.SeoFilename,
                TitleAttribute = picture.TitleAttribute,
                VirtualPath = picture.VirtualPath,
                UrlString = pictureUrl
            };
            pictureModelList.Add(pictureModel);
        }

        return pictureModelList;
    }

    public async Task ExportModel()
    {
        var productModels = await PrepareProductBackupModel();
        var rootUrl = _productBackupSettings.ProductBackupStoragePath;

        foreach (var product in productModels)
        {
            foreach (var picture in product.PictureModelList)
            {
                var destination = $"{rootUrl}/" + product.Id + "_" + picture.Id + ".jpg";
                if (!string.IsNullOrEmpty(destination))
                    File.Copy(picture.UrlString, destination);
            }

            await File.WriteAllTextAsync($"{rootUrl}/" + product.Id + ".json",
                JsonConvert.SerializeObject(product));
            await using var file = File.CreateText($"{rootUrl}/" + product.Id + ".json");
            var serializer = new JsonSerializer();
            serializer.Serialize(file, product);
        }
    }
}