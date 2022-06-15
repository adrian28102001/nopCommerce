using System.Threading.Tasks;

namespace Nop.Plugin.Product.Backup.Services.Export;

public interface IExportService
{
    Task Export();
}