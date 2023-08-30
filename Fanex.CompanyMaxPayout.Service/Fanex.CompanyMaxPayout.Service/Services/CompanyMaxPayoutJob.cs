using Fanex.CompanyMaxPayout.Business.Services;
using ONEbook.UM.Services;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Fanex.CompanyMaxPayout.Service.Services;

public class CompanyMaxPayoutJob : IJob
{
    private readonly IMaintenanceService um;
    private readonly Log log;
    private readonly MainService mainService;

    public CompanyMaxPayoutJob(
        IMaintenanceService um,
        Log log,
        MainService mainService)
    {
        this.um = um;
        this.log = log;
        this.mainService = mainService;
    }

    public Task Execute(IJobExecutionContext context)
    {
        try
        {
            if (um.IsUnderMaintenance())
            {
                return Task.CompletedTask;
            }
            if (!AppConfigService.IsProcessing)
            {
                AppConfigService.IsProcessing = true;
                SendMessage();
                AppConfigService.IsProcessing = false;
            }
        }
        catch (Exception ex)
        {
            log.Log(ex);
            AppConfigService.IsProcessing = false;
        }

        return Task.CompletedTask;
    }

    private void SendMessage()
    {
        mainService.Execute();
    }
}