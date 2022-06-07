using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Product.Backup.Controllers;

public class BackupController: BasePluginController
{
    
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public virtual  Task<IActionResult> Configure()
    {
        return Task.FromResult<IActionResult>(View());
    }
}