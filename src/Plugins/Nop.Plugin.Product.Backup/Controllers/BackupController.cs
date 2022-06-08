using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Product.Backup.Controllers;

[Area(AreaNames.Admin)]
[AutoValidateAntiforgeryToken]
[ValidateIpAddress]
[AuthorizeAdmin]
public class BackupController: BasePluginController
{
    private readonly IPermissionService _permissionService;
    private readonly IProductService _productService;

    public BackupController(IPermissionService permissionService, IProductService productService)
    {
        _permissionService = permissionService;
        _productService = productService;
    }

    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public virtual async Task<IActionResult> Configure()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        //prepare model
        var model = _productService.GetFiveUnexportedProductsAsync();
        return View("~/Plugins/Product.Backup/Views/Configure.cshtml", model);
    }
}