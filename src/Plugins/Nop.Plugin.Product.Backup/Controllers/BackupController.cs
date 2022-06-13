using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Product.Backup.Factory;
using Nop.Plugin.Product.Backup.Models.Settings;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Product.Backup.Controllers;

[Area(AreaNames.Admin)]
[AutoValidateAntiforgeryToken]
[ValidateIpAddress]
[AuthorizeAdmin]
public class BackupController : BasePluginController
{
    private readonly IPermissionService _permissionService;
    private readonly IProductBackupFactory _productBackupFactory;
    private readonly IStoreContext _storeContext;
    private readonly ISettingService _settingService;
    private readonly ICustomerActivityService _customerActivityService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;

    public BackupController(IPermissionService permissionService,
        IProductBackupFactory productBackupFactory, IStoreContext storeContext, ISettingService settingService,
        ICustomerActivityService customerActivityService, ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _permissionService = permissionService;
        _productBackupFactory = productBackupFactory;
        _storeContext = storeContext;
        _settingService = settingService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public virtual async Task<IActionResult> Configure()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        var model = await _productBackupFactory.PrepareProductBackupSettingsModelAsync();
        
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
        await _settingService.SaveSettingOverridablePerStoreAsync(productBackupSettings, x => x.BackupConfigurationEnabled,
            model.BackupConfigurationEnabled_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(productBackupSettings, x => x.ProcessingProductsNumber,
            model.ProcessingProductsNumber_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(productBackupSettings, x => x.ProductBackupTimer,
            model.ProductBackupTimer_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(productBackupSettings, x => x.ProductBackupStoragePath,
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
}

