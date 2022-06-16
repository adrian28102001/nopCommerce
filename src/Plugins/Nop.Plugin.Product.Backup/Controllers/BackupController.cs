using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Product.Backup.Factory.Settings;
using Nop.Plugin.Product.Backup.Models.Settings;
using Nop.Plugin.Product.Backup.Services.Import;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using ProductBackupSettings = Nop.Plugin.Product.Backup.Models.Settings.ProductBackupSettings;

namespace Nop.Plugin.Product.Backup.Controllers;

[Area(AreaNames.Admin)]
[AutoValidateAntiforgeryToken]
[ValidateIpAddress]
[AuthorizeAdmin]
public class BackupController : BasePluginController
{
    private readonly IPermissionService _permissionService;
    private readonly IProductBackupConfigSettings _productBackupConfigFactory;
    private readonly IStoreContext _storeContext;
    private readonly ISettingService _settingService;
    private readonly ICustomerActivityService _customerActivityService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly IWorkContext _workContext;
    private readonly IImportManufacturesFromZip _importManufacturesFromZip;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BackupController(IPermissionService permissionService, IStoreContext storeContext,
        ISettingService settingService,
        ICustomerActivityService customerActivityService, ILocalizationService localizationService,
        INotificationService notificationService, IProductBackupConfigSettings productBackupConfigFactory,
        IWorkContext workContext, IImportManufacturesFromZip importManufacturesFromZip,
        IWebHostEnvironment webHostEnvironment)
    {
        _permissionService = permissionService;
        _storeContext = storeContext;
        _settingService = settingService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _productBackupConfigFactory = productBackupConfigFactory;
        _workContext = workContext;
        _importManufacturesFromZip = importManufacturesFromZip;
        _webHostEnvironment = webHostEnvironment;
    }

    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public virtual async Task<IActionResult> Configure()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        var model = await _productBackupConfigFactory.PrepareProductBackupSettingsModel();

        return View("~/Plugins/Product.Backup/Views/Configure.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Configure(ProductBackupSettingsModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        if (!ModelState.IsValid) return await Configure();

        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var productBackupSettings = await _settingService.LoadSettingAsync<ProductBackupSettings>(storeScope);
        productBackupSettings = model.ToSettings(productBackupSettings);

        //and loaded from database after each update
        await _settingService.SaveSettingOverridablePerStoreAsync(productBackupSettings,
            x => x.BackupConfigurationEnabled,
            model.BackupConfigurationEnabled_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(productBackupSettings,
            x => x.ProcessingProductsNumber,
            model.ProcessingProductsNumber_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(productBackupSettings, x => x.ProductBackupTimer,
            model.ProductBackupTimer_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(productBackupSettings,
            x => x.ProductBackupStoragePath,
            model.ProductBackupStoragePath_OverrideForStore, storeScope, false);

        //now clear settings cache
        await _settingService.ClearCacheAsync();

        //activity log
        await _customerActivityService.InsertActivityAsync("EditSettings",
            await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

        //if we got this far, something failed, redisplay form
        return await Configure();
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public virtual async Task<IActionResult> ImportFromZip(IEnumerable<IFormFile> importZipFiles)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories))
            return AccessDeniedView();

        //a vendor cannot import categories
        if (await _workContext.GetCurrentVendorAsync() != null)
            return AccessDeniedView();

        try
        {
            var importZipFile = importZipFiles.FirstOrDefault();
            if (importZipFile != null && importZipFile.Length > 0)
            {
                await _importManufacturesFromZip.ImportProductsFromZip(importZipFile.OpenReadStream());
            }
            else
            {
                _notificationService.ErrorNotification(
                    await _localizationService.GetResourceAsync("Admin.Common.UploadFile"));
                return RedirectToAction("Configure");
            }

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Admin.Catalog.Categories.Imported"));

            return RedirectToAction("Configure");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Configure");
        }
    }
}