using Fanex.CompanyMaxPayout.Business.Models;
using Fanex.CompanyMaxPayout.Business.Services;
using Fanex.CompanyMaxPayout.Service.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.LetstalkService.Client;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.RemotingRef;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;
using Quartz;
using Quartz.Impl;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LetstalkConfig = Fanex.LetstalkService.Core.Services.AppConfigService;

namespace Fanex.CompanyMaxPayout.Service;

public static class ServiceCollectionExtensions
{
    public static void LoadConfiguration(this IServiceCollection services)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        var text = File.ReadAllText("appsettings.json");
        AppConfigService.Config = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
        LetstalkConfig.Config.LetstalkServiceApi = AppConfigService.Config!.LetstalkServiceApi;
    }

    public static void SentryRegister(this IServiceCollection services)
    {
        var sentry = new SentryLogging(new SentryEngineOptions
        {
            Dsn = new Dsn(AppConfigService.Config.SentryClientKey)
        });

        LogManager
            .SetDefaultLogCategory(AppConfigService.Config.CategorySentryLog)
            .Use(new List<ILogEngine>
            {
                sentry
            });
    }

    public static void FanexDataRegister(this IServiceCollection services, IConfiguration config)
    {
        var connections = config.GetSection("ConnectionStrings")
            .GetChildren()
            .ToDictionary(
                x => x.Key,
                y => new ConnectionConfiguration(y.Key, y.Value)
            );

        DbSettingProviderManager
            .StartNewSession()
            .Use(connections)
            .WithSqlServer()
            .UseDefaultDbSettingProvider(config.GetSection("StoredProcedurePath").Value)
            .Run();
    }

    public static void RemotingRefRegister(this IServiceCollection services)
    {
        var fanexRemotingRefServiceUrl = new Uri(AppConfigService.Config.RemotingRefUrl);
        RefDataTypeContainer.ConnectTo(fanexRemotingRefServiceUrl).FromApplication("Fanex.CompanyMaxPayout.Service");
    }

    public static void InternalServiceRegister(this IServiceCollection services)
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
        services.AddSingleton<CompanyMaxPayoutJob>();

        OnebookUmInitializer.InitializeUmClientManager();
        OnebookUmInitializer.RegisterSimpleInjector(services);
    }

    public static async Task StartScheduler(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        scheduler.JobFactory = new CompanyMaxPayoutJobFactory(serviceProvider);

        var trigger = TriggerBuilder.Create()
            .WithIdentity("Fanex.CompanyMaxPayout.Service", "Fanex.CompanyMaxPayout.Service")
            .StartNow()
            .WithCronSchedule(AppConfigService.Config.CronSchedule)
            .Build();

        var job = JobBuilder.Create<CompanyMaxPayoutJob>().Build();
        await scheduler.ScheduleJob(job, trigger);
        await scheduler.Start();
    }
}