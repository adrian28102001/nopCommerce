using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Nop.Plugin.Product.Backup.Services.Import;

public interface IImportManufacturesFromZip
{
    Task<Task> DecompressFile(IFormFile importZipFiles);
}