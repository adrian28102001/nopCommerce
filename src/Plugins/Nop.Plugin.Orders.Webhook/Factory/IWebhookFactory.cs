using System.Threading.Tasks;
using Nop.Plugin.Orders.Webhook.Models;

namespace Nop.Plugin.Orders.Webhook.Factory;

public interface IWebhookFactory
{
    /// <summary>
    /// Prepare webhook settings model
    /// </summary>
    /// <param name="model">Webhook settings model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the webhook settings model
    /// </returns>
    Task<WebhookSettingModel> PrepareWebHookSettingsModelAsync(WebhookSettingModel model = null);

}