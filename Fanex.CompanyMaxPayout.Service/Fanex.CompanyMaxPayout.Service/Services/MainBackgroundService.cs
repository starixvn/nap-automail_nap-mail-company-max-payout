using Fanex.CompanyMaxPayout.Business.Services;
using Microsoft.Extensions.Hosting;
using ONEbook.UM.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.CompanyMaxPayout.Service.Services;

public class MainBackgroundService(MainService mainService, IMaintenanceService um) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int periodicityInMinute = 1;
        const int delay = periodicityInMinute * 60 * 1000;

        while (!stoppingToken.IsCancellationRequested)
        {
            if (await um.IsUnderMaintenanceAsync())
            {
                continue;
            }

            mainService.Execute();
            await Task.Delay(delay, stoppingToken);
        }
    }
}