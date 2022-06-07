using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Plugins;

namespace Nop.Plugin.Product.Backup.Provider;

public class BackupProvider : BasePlugin, IMiscPlugin
{
    private readonly IWebHelper _webHelper;

    public BackupProvider(IWebHelper webHelper)
    {
        _webHelper = webHelper;
    }

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/Backup/Configure";
    }
}