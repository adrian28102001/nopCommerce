using System.Threading.Tasks;

namespace Nop.Plugin.Product.Backup.Services.Picture;

public interface IBackupPictureService
{
    public Task<string> GetPictureUrl(Core.Domain.Media.Picture picture);
}