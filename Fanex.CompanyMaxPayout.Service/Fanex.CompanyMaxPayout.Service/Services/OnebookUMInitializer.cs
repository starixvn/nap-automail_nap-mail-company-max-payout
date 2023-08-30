using Fanex.Caching;
using Fanex.CompanyMaxPayout.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using ONEbook.UM;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;

namespace Fanex.CompanyMaxPayout.Service.Services;

public static class OnebookUmInitializer
{
    public static void InitializeUmClientManager()
    {
        var umConfig = AppConfigService.Config.UmConfig;

        UMClientManager
            .UseConfig(umConfig)
            .UseCacheService(new UMCacheService(new CacheService()))
            .UseRestClient(new UMRestClient(umConfig));
    }

    public static void RegisterSimpleInjector(IServiceCollection services)
    {
        var umConfig = AppConfigService.Config.UmConfig;
        services.AddSingleton(umConfig);
        services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
        services.AddSingleton<ICacheService>(new CacheService());
        services.AddSingleton<IUMCacheService, UMCacheService>();
        services.AddSingleton<IPHelper, IPHelperImpl>();
        services.AddSingleton<IUMRestClient, UMRestClient>();
        services.AddSingleton<IMaintenanceService, MaintenanceService>();
    }
}

public class HttpContextHelper : IHttpContextHelper
{
    public string GetSession(string key)
    {
        return string.Empty;
    }

    public string GetValueFromQueryString(string queryKey)
    {
        return string.Empty;
    }

    public void SetSession(string key, string value)
    {
    }
}