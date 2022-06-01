using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Orders.Webhook.Models;
using Nop.Services.Configuration;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace Nop.Plugin.Orders.Webhook.Factory;

public class WebhookFactory
{
    private readonly IStoreContext _storeContext;
    private readonly ISettingService _settingService;

    public WebhookFactory(IStoreContext storeContext, ISettingService settingService)
    {
        _storeContext = storeContext;
        _settingService = settingService;
    }
    
    public async Task<WebhookSettingModel> PrepareWebHookSettingsModelAsync(WebhookSettingModel model = null)
    {
        //load settings for a chosen store scope
        var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var webhookSettings = await _settingService.LoadSettingAsync<WebhookSettings>(storeId);

        //fill in model values from the entity
        model ??= webhookSettings.ToSettingsModel<WebhookSettingModel>();

        //fill in additional values (not existing in the entity)
        model.ActiveStoreScopeConfiguration = storeId;
            
        if (storeId <= 0)
            return model;
            
        model.ConfigurationEnabled_OverrideForStore = await _settingService.SettingExistsAsync(webhookSettings, x => x.ConfigurationEnabled, storeId);
        model.PlaceOrderEndpointUrl_OverrideForStore = await _settingService.SettingExistsAsync(webhookSettings, x => x.PlaceOrderEndpointUrl, storeId);
            
        return model;
    }
}