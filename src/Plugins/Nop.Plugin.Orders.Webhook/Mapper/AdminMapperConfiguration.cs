using AutoMapper;
using Nop.Core.Domain.WebhookSettings;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Orders.Webhook.Models;

namespace Nop.Plugin.Orders.Webhook.Mapper;

public class AdminMapperConfiguration: Profile, IOrderedMapperProfile
{
    public AdminMapperConfiguration()
    {
        CreateWebhookMaps();
    }
    
    protected virtual void CreateWebhookMaps() 
    {
        CreateMap<WebhookSettings, WebhookSettingModel>()
            .ForMember(model => model.ConfigurationEnabled_OverrideForStore,options => options.Ignore())
            .ForMember(model => model.PlaceOrderEndpointUrl_OverrideForStore, options => options.Ignore());
        CreateMap<WebhookSettingModel, WebhookSettings>();
    }

    public int Order { get; }
}