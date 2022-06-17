using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Plugin.Product.Backup.Models;
using Nop.Plugin.Product.Backup.Models.Settings;
using Nop.Plugin.Product.Backup.Services.Picture;
using Nop.Plugin.Product.Backup.Services.Product;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace Nop.Plugin.Product.Backup.Factory;

public class ProductBackupFactory : IProductBackupFactory
{
    private readonly ProductBackupSettings _productBackupSettings;
    private readonly IProductService _productService;
    private readonly Nop.Services.Catalog.IProductService _service;
    private readonly IPictureService _pictureService;
    private readonly IBackupPictureService _backupPictureService;

    public ProductBackupFactory(ProductBackupSettings productBackupSettings, IProductService productService,
        IPictureService pictureService,
        IBackupPictureService backupPictureService, Nop.Services.Catalog.IProductService service)
    {
        _productBackupSettings = productBackupSettings;
        _productService = productService;
        _pictureService = pictureService;
        _backupPictureService = backupPictureService;
        _service = service;
    }

    public async Task<List<ProductModel>> PrepareProductBackupModel()
    {
        if (!_productBackupSettings.BackupConfigurationEnabled) new List<ProductModel>();

        var models = await _productService.GetNextProductsToExport();
        var productModelList = new List<ProductModel>();

        foreach (var model in models)
        {
            var pictureModelList = await PrepareImageModel(model.Id);

            var mappedModel = new ProductModel
            {
                Id = model.Id,
                ProductTypeId = model.ProductTypeId,
                ParentGroupedProductId = model.ParentGroupedProductId,
                VisibleIndividually = model.VisibleIndividually,
                Name = model.Name,
                ShortDescription = model.ShortDescription,
                FullDescription = model.FullDescription,
                AdminComment = model.AdminComment,
                ProductTemplateId = model.ProductTemplateId,
                VendorId = model.VendorId,
                ShowOnHomepage = model.ShowOnHomepage,
                MetaKeywords = model.MetaKeywords,
                MetaDescription = model.MetaDescription,
                MetaTitle = model.MetaTitle,
                AllowCustomerReviews = model.AllowCustomerReviews,
                ApprovedRatingSum = model.ApprovedRatingSum,
                NotApprovedRatingSum = model.NotApprovedRatingSum,
                ApprovedTotalReviews = model.ApprovedTotalReviews,
                NotApprovedTotalReviews = model.NotApprovedTotalReviews,
                SubjectToAcl = model.SubjectToAcl,
                LimitedToStores = model.LimitedToStores,
                Sku = model.Sku,
                ManufacturerPartNumber = model.ManufacturerPartNumber,
                Gtin = model.Gtin,
                IsGiftCard = model.IsGiftCard,
                GiftCardTypeId = model.GiftCardTypeId,
                OverriddenGiftCardAmount = model.OverriddenGiftCardAmount,
                RequireOtherProducts = model.RequireOtherProducts,
                RequiredProductIds = model.RequiredProductIds,
                AutomaticallyAddRequiredProducts = model.AutomaticallyAddRequiredProducts,
                IsDownload = model.IsDownload,
                DownloadId = model.DownloadId,
                UnlimitedDownloads = model.UnlimitedDownloads,
                MaxNumberOfDownloads = model.MaxNumberOfDownloads,
                DownloadExpirationDays = model.DownloadExpirationDays,
                DownloadActivationTypeId = model.DownloadActivationTypeId,
                HasSampleDownload = model.HasSampleDownload,
                SampleDownloadId = model.SampleDownloadId,
                HasUserAgreement = model.HasUserAgreement,
                UserAgreementText = model.UserAgreementText,
                IsRecurring = model.IsRecurring,
                RecurringCycleLength = model.RecurringCycleLength,
                RecurringCyclePeriodId = model.RecurringCyclePeriodId,
                RecurringTotalCycles = model.RecurringTotalCycles,
                IsRental = model.IsRental,
                RentalPriceLength = model.RentalPriceLength,
                RentalPricePeriodId = model.RentalPricePeriodId,
                IsShipEnabled = model.IsShipEnabled,
                IsFreeShipping = model.IsFreeShipping,
                ShipSeparately = model.ShipSeparately,
                AdditionalShippingCharge = model.AdditionalShippingCharge,
                DeliveryDateId = model.DeliveryDateId,
                IsTaxExempt = model.IsTaxExempt,
                TaxCategoryId = model.TaxCategoryId,
                IsTelecommunicationsOrBroadcastingOrElectronicServices =
                    model.IsTelecommunicationsOrBroadcastingOrElectronicServices,
                ManageInventoryMethodId = model.ManageInventoryMethodId,
                ProductAvailabilityRangeId = model.ProductAvailabilityRangeId,
                UseMultipleWarehouses = model.UseMultipleWarehouses,
                WarehouseId = model.WarehouseId,
                StockQuantity = model.StockQuantity,
                DisplayStockAvailability = model.DisplayStockAvailability,
                DisplayStockQuantity = model.DisplayStockQuantity,
                MinStockQuantity = model.MinStockQuantity,
                LowStockActivityId = model.LowStockActivityId,
                NotifyAdminForQuantityBelow = model.NotifyAdminForQuantityBelow,
                BackorderModeId = model.BackorderModeId,
                AllowBackInStockSubscriptions = model.AllowBackInStockSubscriptions,
                OrderMinimumQuantity = model.OrderMinimumQuantity,
                OrderMaximumQuantity = model.OrderMaximumQuantity,
                AllowedQuantities = model.AllowedQuantities,
                AllowAddingOnlyExistingAttributeCombinations = model.AllowAddingOnlyExistingAttributeCombinations,
                NotReturnable = model.NotReturnable,
                DisableBuyButton = model.DisableBuyButton,
                DisableWishlistButton = model.DisableWishlistButton,
                AvailableForPreOrder = model.AvailableForPreOrder,
                PreOrderAvailabilityStartDateTimeUtc = model.PreOrderAvailabilityStartDateTimeUtc,
                CallForPrice = model.CallForPrice,
                Price = model.Price,
                OldPrice = model.OldPrice,
                ProductCost = model.ProductCost,
                CustomerEntersPrice = model.CustomerEntersPrice,
                MinimumCustomerEnteredPrice = model.MinimumCustomerEnteredPrice,
                MaximumCustomerEnteredPrice = model.MaximumCustomerEnteredPrice,
                BasepriceEnabled = model.BasepriceEnabled,
                BasepriceAmount = model.BasepriceAmount,
                BasepriceUnitId = model.BasepriceUnitId,
                BasepriceBaseAmount = model.BasepriceBaseAmount,
                BasepriceBaseUnitId = model.BasepriceBaseUnitId,
                MarkAsNew = model.MarkAsNew,
                MarkAsNewStartDateTimeUtc = model.MarkAsNewStartDateTimeUtc,
                MarkAsNewEndDateTimeUtc = model.MarkAsNewEndDateTimeUtc,
                HasTierPrices = model.HasTierPrices,
                HasDiscountsApplied = model.HasDiscountsApplied,
                Weight = model.Weight,
                Length = model.Length,
                Width = model.Width,
                Height = model.Height,
                AvailableStartDateTimeUtc = model.AvailableStartDateTimeUtc,
                AvailableEndDateTimeUtc = model.AvailableEndDateTimeUtc,
                DisplayOrder = model.DisplayOrder,
                Published = model.Published,
                Deleted = model.Deleted,
                CreatedOnUtc = model.CreatedOnUtc,
                UpdatedOnUtc = model.UpdatedOnUtc,
                Exported = model.Exported,
                PictureModelList = pictureModelList
            };
            model.Exported = false;

            await _service.UpdateProductAsync(model);
            productModelList.Add(mappedModel);
        }

        return productModelList;
    }

    private async Task<List<PictureModel>> PrepareImageModel(int imageId)
    {
        var pictureModelList = new List<PictureModel>();

        var pictures = await _pictureService.GetPicturesByProductIdAsync(imageId);

        foreach (var picture in pictures)
        {
            var pictureUrl = await _backupPictureService.GetPictureUrl(picture);

            var pictureModel = new PictureModel()
            {
                Id = picture.Id,
                AltAttribute = picture.AltAttribute,
                IsNew = picture.IsNew,
                MimeType = picture.MimeType,
                SeoFilename = picture.SeoFilename,
                TitleAttribute = picture.TitleAttribute,
                VirtualPath = picture.VirtualPath,
                UrlString = pictureUrl
            };
            pictureModelList.Add(pictureModel);
        }

        return pictureModelList;
    }
}