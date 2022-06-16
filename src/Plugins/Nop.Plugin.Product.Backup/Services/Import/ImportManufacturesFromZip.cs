using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Nop.Plugin.Product.Backup.Models.Settings;
using Nop.Plugin.Product.Backup.Services.Helpers;

namespace Nop.Plugin.Product.Backup.Services.Import;

public class ImportManufacturesFromZip : IImportManufacturesFromZip
{
    private IFileHelper FileHelper { get; set; }
    private ProductBackupSettings ProductBackupSettings { get; set; }


    public ImportManufacturesFromZip(IFileHelper fileHelper, ProductBackupSettings productBackupSettings)
    {
        FileHelper = fileHelper;
        ProductBackupSettings = productBackupSettings;
    }

    public Task DecompressFile(IFormFile importZipFiles)
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
                importZipFiles.CopyTo(fileStream);
            }
            
            ZipFile.ExtractToDirectory(filePath, directory);
        }

        return Task.CompletedTask;

        // using (var archive = ZipFile.OpenRead(zipPath))
        // {
        //     foreach (var entry in archive.Entries)
        //     {
        //         entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
        //         using var r = new StreamReader($"{entry}");
        //         var json = r.ReadToEnd();
        //         var products = JsonConvert.DeserializeObject<List<ProductModel>>(json);
        //     }
        // }

        return Task.CompletedTask;
    }
}