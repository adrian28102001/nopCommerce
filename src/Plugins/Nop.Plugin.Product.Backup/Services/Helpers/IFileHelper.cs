using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;

namespace Nop.Plugin.Product.Backup.Services.Helpers;

public interface IFileHelper
{
    public void DownloadFile(int productId,int pictureId, string source);
    public Task SerializeToJson(ProductModel productModel);
}