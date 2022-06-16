using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
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

    public Task ImportProductsFromZip(Stream stream)
    {
        using var workFiles = new ZipArchive(stream);
        var workFile = workFiles.Entries.FirstOrDefault();
        if (workFile == null)
            throw new NopException("No zip found");

        return Task.CompletedTask;
    }
}