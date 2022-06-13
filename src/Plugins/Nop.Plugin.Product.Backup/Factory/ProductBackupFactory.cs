using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;
using Nop.Plugin.Product.Backup.Models.Settings;
using Nop.Plugin.Product.Backup.Services.Picture;
using Nop.Plugin.Product.Backup.Services.Product;
using Nop.Services.Media;

namespace Nop.Plugin.Product.Backup.Factory;

public class ProductBackupFactory : IProductBackupFactory
{
    private readonly ProductBackupSettings _productBackupSettings;
    private readonly IProductService _productService;
    private readonly Nop.Services.Catalog.IProductService _service;
    private readonly IPictureService _pictureService;
    private readonly IBackupPictureService _backupPictureService;

    public ProductBackupFactory(ProductBackupSettings productBackupSettings, IProductService productService,
        IPictureService pictureService,
        IBackupPictureService backupPictureService, Nop.Services.Catalog.IProductService service)
    {
        _productBackupSettings = productBackupSettings;
        _productService = productService;
        _pictureService = pictureService;
        _backupPictureService = backupPictureService;
        _service = service;
    }

    public async Task<List<ProductModel>> PrepareProductBackupModel()
    {
        if (!_productBackupSettings.BackupConfigurationEnabled) new List<ProductModel>();

        var models = await _productService.GetNextProductsToExport();
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
            model.Exported = false;

            await _service.UpdateProductAsync(model);
            productModelList.Add(mappedModel);
        }
        return productModelList;
    }

    public async Task<List<PictureModel>> PrepareImageModel(int imageId)
    {
        var pictureModelList = new List<PictureModel>();

        var pictures = await _pictureService.GetPicturesByProductIdAsync(imageId);

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
}