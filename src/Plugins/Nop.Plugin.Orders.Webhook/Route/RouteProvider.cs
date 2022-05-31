using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Orders.Webhook.Route;

public class RouteProvider: IRouteProvider
{
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute("Plugin.Payments.Orders.Webhook", "Plugins/OrderWebhook/WebhookConfigure",
            new { controller = "OrderWebhook", action = "WebhookConfigure" });
    }
    public int Priority { get; }
}