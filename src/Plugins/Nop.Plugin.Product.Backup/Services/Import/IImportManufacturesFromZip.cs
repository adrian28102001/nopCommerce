using System.IO;
using System.Threading.Tasks;

namespace Nop.Plugin.Product.Backup.Services.Import;

public interface IImportManufacturesFromZip
{
    Task ImportProductsFromZip(Stream stream);
}