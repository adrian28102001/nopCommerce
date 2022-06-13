using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nop.Plugin.Product.Backup.Models;
using Nop.Plugin.Product.Backup.Models.Settings;

namespace Nop.Plugin.Product.Backup.Services.Helpers;

public class FileHelper : IFileHelper
{
    private readonly ProductBackupSettings _productBackupConfigSettings;

    public FileHelper(ProductBackupSettings productBackupConfigSettings)
    {
        _productBackupConfigSettings = productBackupConfigSettings;
    }

    public void DownloadFile(int productId,int pictureId, string source)
    {
        var rootUrl = _productBackupConfigSettings.ProductBackupStoragePath;
        var destination = $"{rootUrl}/" + productId + "_" + pictureId + ".jpg";
        if (!string.IsNullOrEmpty(destination))
            File.Copy(source, destination);
    }

    public async Task SerializeToJson(ProductModel productModel)
    {
        var rootUrl = _productBackupConfigSettings.ProductBackupStoragePath;
        await File.WriteAllTextAsync($"{rootUrl}/" + productModel.Id + ".json",
            JsonConvert.SerializeObject(productModel));
        await using var file = File.CreateText($"{rootUrl}/" + productModel.Id + ".json");
        var serializer = new JsonSerializer();
        serializer.Serialize(file, productModel);
    }
}