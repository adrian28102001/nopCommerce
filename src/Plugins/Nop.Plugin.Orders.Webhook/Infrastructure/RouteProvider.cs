using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Orders.Webhook.Infrastructure;

public class RouteProvider: IRouteProvider
{
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute("Plugin.Payments.Orders.Webhook", "Plugins/OrderWebhook/WConfigure",
            new { controller = "OrderWebhook", action = "Configure" });

    }
    public int Priority { get; }
}