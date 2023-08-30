using Fanex.Data.Repository;

namespace Fanex.CompanyMaxPayout.Business.Criteria;

public class MaxPayoutMemSelCriteria : CriteriaBase
{
    public int ProductId { get; set; }

    public string Username { get; set; }

    public override string GetSettingKey()
    {
        return "NAP_MaxPayout_Notification_MemSel";
    }

    public override bool IsValid()
    {
        return true;
    }
}