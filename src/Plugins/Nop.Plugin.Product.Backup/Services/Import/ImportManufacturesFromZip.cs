using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Product.Backup.Mapper;
using Nop.Plugin.Product.Backup.Models;
using Nop.Plugin.Product.Backup.Models.Settings;
using Nop.Plugin.Product.Backup.Services.Helpers;
using Nop.Services.Catalog;
using Nop.Services.Media;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Nop.Plugin.Product.Backup.Services.Import;

public class ImportManufacturesFromZip : IImportManufacturesFromZip
{
    private IFileHelper FileHelper { get; set; }
    private IProductService ProductService { get; set; }
    private ProductBackupSettings ProductBackupSettings { get; set; }
    private readonly IProductService _productService;
    private readonly IPictureService _pictureService;
    private readonly IMapping _mapping;

    public ImportManufacturesFromZip(IFileHelper fileHelper, ProductBackupSettings productBackupSettings,
        IProductService productService, IPictureService pictureService, IMapping mapping)
    {
        FileHelper = fileHelper;
        ProductBackupSettings = productBackupSettings;
        _productService = productService;
        _pictureService = pictureService;
        _mapping = mapping;
        ProductService = productService;
    }

    public async Task<Task> DecompressFile(IFormFile importZipFiles)
    {
        var temporaryDirectory = Path.GetTempPath();
        var folderName = Guid.NewGuid().ToString();
        var directory = Path.Combine(temporaryDirectory, folderName);

        Directory.CreateDirectory(directory);

        if (importZipFiles.Length > 0)
        {
            var filePath = Path.Combine(directory, importZipFiles.FileName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                await importZipFiles.CopyToAsync(fileStream);
            }

            ZipFile.ExtractToDirectory(filePath, directory);

            var decompressedDirectory = filePath.Substring(0, filePath.IndexOf(".", StringComparison.Ordinal));

            await ProcessFile(decompressedDirectory);
        }

        return Task.CompletedTask;
    }

    private async Task ProcessFile(string directory)
    {
        var filePathsJson = Directory.GetFiles($"{directory}", @"*.json");
        foreach (var file in filePathsJson)
        {
            var jsonString = await File.ReadAllTextAsync(file);
            var productJson = JsonSerializer.Deserialize<ProductModel>(jsonString)!;
            var product = await _mapping.MapProducts(productJson);
            product.Published = true;
            var skuExists = await _productService.GetProductBySkuAsync(product.Sku);
            if (skuExists == null)
            {
                await _productService.InsertProductAsync(product);
            }
            else
            {
                await _productService.UpdateProductAsync(product);
            }

            var counter = 0;

            foreach (var picture in productJson.PictureModelList)
            {
                var mappedPicture = await _mapping.MapPictures(picture);
                var pictureId = await _pictureService.GetPictureByIdAsync(mappedPicture.Id);

                var pictureCopy = await _pictureService.InsertPictureAsync(
                    await _pictureService.LoadPictureBinaryAsync(pictureId),
                    pictureId.MimeType,
                    await _pictureService.GetPictureSeNameAsync(pictureId.SeoFilename),
                    pictureId.AltAttribute,
                    pictureId.TitleAttribute);

                var productPicture = new ProductPicture
                {
                    PictureId = pictureCopy.Id,
                    DisplayOrder = counter,
                    ProductId = skuExists == null ? product.Id : productJson.Id
                };

                counter++;
                await _productService.InsertProductPictureAsync(productPicture);
            }
        }
    }
}