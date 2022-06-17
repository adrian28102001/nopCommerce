using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;

namespace Nop.Plugin.Product.Backup.Mapper;

public interface IMapping
{
    Task<Core.Domain.Catalog.Product> MapProducts(ProductModel productModel);
    Task<Core.Domain.Media.Picture> MapPictures(PictureModel pictureModel);
}