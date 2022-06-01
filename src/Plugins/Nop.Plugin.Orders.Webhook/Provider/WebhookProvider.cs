using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Plugins;

namespace Nop.Plugin.Orders.Webhook.Provider;

public class WebhookProvider : BasePlugin, IWebhookProvider, IMiscPlugin
{
    private readonly IWebHelper _webHelper;

    public WebhookProvider(IWebHelper webHelper)
    {
        _webHelper = webHelper;
    }

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/OrderWebhook/Configure";
    }
}