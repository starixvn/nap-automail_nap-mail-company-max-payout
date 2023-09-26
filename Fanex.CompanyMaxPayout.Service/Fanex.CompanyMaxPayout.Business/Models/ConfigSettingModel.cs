using ONEbook.UM.Configurations;

namespace Fanex.CompanyMaxPayout.Business.Models;

public class ConfigSettingModel
{
    public string SentryClientKey { get; set; }

    public string CategorySentryLog { get; set; }

    public string LetstalkServiceApi { get; set; }

    public string TargetName { get; set; }

    public string LetstalkGroup { get; set; }

    public string RemotingRefUrl { get; set; }

    public UMClientConfiguration UmConfig { get; set; }

    public string CronSchedule { get; set; } = "0/10 * * * * ?";

    public bool IsRealCurrency { get; set; }

    public int ProductId { get; set; }

    public string UserName { get; set; }

    public string Environment { get; set; }
}