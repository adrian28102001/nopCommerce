using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Factory;
using Nop.Plugin.Product.Backup.Services.Helpers;

namespace Nop.Plugin.Product.Backup.Services.Export;

public class ExportService : IExportService
{
    private readonly IProductBackupFactory _productBackupFactory;
    private readonly IFileHelper _fileHelper;

    public ExportService(IProductBackupFactory productBackupFactory, IFileHelper fileHelper)
    {
        _productBackupFactory = productBackupFactory;
        _fileHelper = fileHelper;
    }

    public async Task Export()
    {
        var productModels = await _productBackupFactory.PrepareProductBackupModel();

        foreach (var product in productModels)
        {
            foreach (var picture in product.PictureModelList)
                _fileHelper.ExportFile(product.Id, picture.Id, picture.UrlString);

            await _fileHelper.WriteToFile(product);
        }
    }
}