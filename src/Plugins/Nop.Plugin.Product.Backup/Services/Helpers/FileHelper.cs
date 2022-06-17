﻿using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
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

    public void ExportFile(int productId, int pictureId, string source)
    {
        var rootUrl = _productBackupConfigSettings.ProductBackupStoragePath;
        var destination = $"{rootUrl}/" + productId + "_" + pictureId + ".jpg";
        if (!string.IsNullOrEmpty(destination))
            File.Copy(source, destination);
    }

    public async Task WriteToFile(ProductModel productModel)
    {
        var rootUrl = _productBackupConfigSettings.ProductBackupStoragePath;
        await File.WriteAllTextAsync($"{rootUrl}/" + productModel.Id + ".json",
            JsonConvert.SerializeObject(productModel));
        await using var file = File.CreateText($"{rootUrl}/" + productModel.Id + ".json");
        var serializer = new JsonSerializer();
        serializer.Serialize(file, productModel);
    }

    public Task Decompress(DirectoryInfo directoryPath)
    {
        //get all files
        foreach (var file in directoryPath.GetFiles())
        {
            var path = directoryPath.FullName;
            //get zip file, if exists
            var zipPath = path + file.Name;

            var extractPath = Regex.Replace(path + file.Name, ".zip", "");

            //if path exists, delete it to generate new extracted files
            if (Directory.Exists(extractPath))
                Directory.Delete(extractPath, true);

            //if zip exists extract it
            if (File.Exists(zipPath))
                ZipFile.ExtractToDirectory(zipPath, extractPath);
        }
        return Task.CompletedTask;
    }
}