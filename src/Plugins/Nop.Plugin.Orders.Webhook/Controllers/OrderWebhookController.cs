using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.WebhookSettings;
using Nop.Plugin.Orders.Webhook.Factory;
using Nop.Plugin.Orders.Webhook.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Orders.Webhook.Controllers;

public class OrderWebhookController : BaseController
{
    private readonly PermissionService _permissionService;
    private readonly IWebhookFactory _webhookFactory;
    private readonly IStoreContext _storeContext;
    private readonly ISettingService _settingService;
    private readonly ICustomerActivityService _customerActivityService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;

    public OrderWebhookController(PermissionService permissionService,
        IStoreContext storeContext, ISettingService settingService, ICustomerActivityService customerActivityService,
        ILocalizationService localizationService, INotificationService notificationService,
        IWebhookFactory webhookFactory)
    {
        _permissionService = permissionService;
        _storeContext = storeContext;
        _settingService = settingService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _webhookFactory = webhookFactory;
    }

    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public virtual async Task<IActionResult> WebhookConfigure()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        //prepare model
        var model = await _webhookFactory.PrepareWebHookSettingsModelAsync();

        return View("_Page_Views_OrderWebhook_cshtml/_Webhook.Common", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> WebhookConfigure(WebhookSettingModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        if (!ModelState.IsValid) return await WebhookConfigure();

        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var webhookSettings = await _settingService.LoadSettingAsync<WebhookSettings>(storeScope);
        webhookSettings = model.ToSettings(webhookSettings);


        //and loaded from database after each update
        await _settingService.SaveSettingOverridablePerStoreAsync(webhookSettings, x => x.ConfigurationEnabled,
            model.ConfigurationEnabled_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(webhookSettings, x => x.PlaceOrderEndpointUrl,
            model.PlaceOrderEndpointUrl_OverrideForStore, storeScope, false);

        //now clear settings cache
        await _settingService.ClearCacheAsync();

        //activity log
        await _customerActivityService.InsertActivityAsync("EditSettings",
            await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

        //if we got this far, something failed, redisplay form
        return await WebhookConfigure();
    }
}