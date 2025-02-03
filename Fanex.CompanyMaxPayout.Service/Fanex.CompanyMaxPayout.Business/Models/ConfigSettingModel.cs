using System;

namespace Fanex.CompanyMaxPayout.Business.Models;

public class ConfigSettingModel
{
    private static readonly DateTimeOffset StartTime = DateTimeOffset.Now;

    public string LetstalkServiceApi { get; set; }

    public string TargetName { get; set; }

    public string LetstalkGroup { get; set; }

    public string RemotingRefUrl { get; set; }

    public bool IsRealCurrency { get; set; }

    public int ProductId { get; set; }

    public string UserName { get; set; }

    public string Environment { get; set; }

    private const int Twelve = 12;
    private const int TwentyOne = 21;
    private const int TwentyTwo = 22;

    public bool LifeCycleCompleted
    {
        get
        {
            var workedHours = (DateTimeOffset.Now - StartTime).TotalHours;
            return workedHours >= Twelve && DateTimeOffset.Now.Hour >= TwentyOne && DateTimeOffset.Now.Hour <= TwentyTwo;
        }
    }

    public bool SafeToRestart => LifeCycleCompleted;
}