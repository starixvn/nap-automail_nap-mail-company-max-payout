using Fanex.CompanyMaxPayout.Business.Models;
using Fanex.CompanyMaxPayout.Business.Services;
using Fanex.Data.Repository;
using Fanex.LetstalkService.Client;
using Fanex.RemotingRef;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.IO;
using AppConfigService = Fanex.CompanyMaxPayout.Business.Services.AppConfigService;
using LetstalkConfig = Fanex.LetstalkService.Client.AppConfigService;

namespace Fanex.CompanyMaxPayout.Service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Services { get; set; }

    public static void AddConfiguration(this IServiceCollection services)
    {
        Services = services;

        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        var text = File.ReadAllText("appsettings.json");
        AppConfigService.Config = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
        LetstalkConfig.Config.LetstalkServiceApi = AppConfigService.Config!.LetstalkServiceApi;
    }

    public static void AddRemotingRef(this IServiceCollection services)
    {
        var fanexRemotingRefServiceUrl = new Uri(AppConfigService.Config.RemotingRefUrl);
        RefDataTypeContainer.ConnectTo(fanexRemotingRefServiceUrl).FromApplication("Fanex.CompanyMaxPayout.Service");
    }

    public static void AddInternalService(this IServiceCollection services)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        services.AddSingleton<Log, LogImp>();
        services.AddSingleton<MessageService, MessageServiceImp>();
        services.AddSingleton<MainService, MainServiceImp>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<LetstalkClientService, LetstalkClientServiceImp>();
        services.AddSingleton<IRemotingRefService, RemotingRefService>();
        services.AddSingleton<CacheRefService, CacheRefServiceImp>();
        services.AddSingleton<DataService, DataServiceImp>();
        services.AddSingleton<TransformService, TransformServiceImp>();
    }
}