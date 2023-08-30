using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace Fanex.CompanyMaxPayout.Service.Services;

public class CompanyMaxPayoutJobFactory : IJobFactory
{
    private readonly IServiceProvider serviceProvider;

    public CompanyMaxPayoutJobFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return serviceProvider.GetService<CompanyMaxPayoutJob>();
    }

    public void ReturnJob(IJob job)
    {
        // nothing todo ..
    }
}