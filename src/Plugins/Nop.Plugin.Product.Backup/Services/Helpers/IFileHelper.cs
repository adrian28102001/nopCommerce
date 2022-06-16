using System.IO;
using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;

namespace Nop.Plugin.Product.Backup.Services.Helpers;

public interface IFileHelper
{
    public void ExportFile(int productId, int pictureId, string source);
    public Task WriteToFile(ProductModel productModel);
    public Task Decompress(DirectoryInfo directoryPath);
}