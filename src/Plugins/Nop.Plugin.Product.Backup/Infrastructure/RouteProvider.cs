using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Product.Backup.Infrastructure;

public class RouteProvider: IRouteProvider
{
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute("Plugin.Product.Backup", "Plugins/Backup/Configure",
            new { controller = "Backup", action = "Configure" });
    }
    public int Priority { get; }
}