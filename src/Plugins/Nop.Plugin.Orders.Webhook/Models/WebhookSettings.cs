using Nop.Core.Configuration;

namespace Nop.Plugin.Orders.Webhook.Models;

public class WebhookSettings : ISettings
{
    public bool ConfigurationEnabled { get; set; }
    public bool ConfigurationEnabled_OverrideForStore { get; set; }

    public string PlaceOrderEndpointUrl { get; set; }
    public bool PlaceOrderEndpointUrl_OverrideForStore { get; set; }
}