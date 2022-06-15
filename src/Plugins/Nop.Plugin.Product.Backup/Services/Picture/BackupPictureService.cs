using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Seo;

namespace Nop.Plugin.Product.Backup.Services.Picture;

public class BackupPictureService : PictureService, IBackupPictureService
{
    public BackupPictureService(IDownloadService downloadService, IHttpContextAccessor httpContextAccessor,
        ILogger logger, INopFileProvider fileProvider, IProductAttributeParser productAttributeParser,
        IRepository<Core.Domain.Media.Picture> pictureRepository, IRepository<PictureBinary> pictureBinaryRepository,
        IRepository<ProductPicture> productPictureRepository, ISettingService settingService,
        IUrlRecordService urlRecordService, IWebHelper webHelper, MediaSettings mediaSettings) : base(downloadService,
        httpContextAccessor, logger, fileProvider, productAttributeParser, pictureRepository, pictureBinaryRepository,
        productPictureRepository, settingService, urlRecordService, webHelper, mediaSettings)
    {
    }

    public async Task<string> GetPictureUrl(Core.Domain.Media.Picture picture)
    {
        var lastPart = await GetFileExtensionFromMimeTypeAsync(picture.MimeType);

        var thumbFileName = !string.IsNullOrEmpty(picture.SeoFilename)
            ? $"{picture.Id:0000000}_{picture.SeoFilename}.{lastPart}"
            : $"{picture.Id:0000000}.{lastPart}";

        var path = await GetThumbLocalPathAsync(thumbFileName);
        return path;
    }
}