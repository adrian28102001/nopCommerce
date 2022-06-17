using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;

namespace Nop.Plugin.Product.Backup.Mapper;

public class Mapping : IMapping
{
    public Task<Core.Domain.Catalog.Product> MapProducts(ProductModel productModel)
    {
        var product = new Core.Domain.Catalog.Product()
        {
            ProductTypeId = productModel.ProductTypeId,
            Name = productModel.Name,
            ShortDescription = productModel.ShortDescription,
            FullDescription = productModel.FullDescription,
            Sku = productModel.Sku,
            StockQuantity = productModel.StockQuantity,
            OldPrice = productModel.OldPrice,
            Price = productModel.Price,
            CreatedOnUtc = productModel.CreatedOnUtc,
            UpdatedOnUtc = productModel.UpdatedOnUtc,
        };

        return Task.FromResult(product);
    }

    public Task<Core.Domain.Media.Picture> MapPictures(PictureModel pictureModel)
    {
        var picture = new Core.Domain.Media.Picture()
        {
            Id = pictureModel.Id,
            AltAttribute = pictureModel.AltAttribute,
            IsNew = pictureModel.IsNew,
            MimeType = pictureModel.MimeType,
            SeoFilename = pictureModel.SeoFilename,
            TitleAttribute = pictureModel.TitleAttribute,
            VirtualPath = pictureModel.VirtualPath,
        };

        return Task.FromResult(picture);
    }
}