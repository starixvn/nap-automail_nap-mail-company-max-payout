using Fanex.CompanyMaxPayout.Business.Models;
using Fanex.Data.Repository;
using System.Globalization;
using System.Xml;

namespace Fanex.CompanyMaxPayout.Business.Criteria;

public class MaxPayoutMemUpdCommand : NonQueryCommand
{
    public MaxPayoutMemUpdCommand(MemberModel member)
    {
        var doc = new XmlDocument();
        var root = doc.CreateElement("notify");

        var row = doc.CreateElement("row");
        row.SetAttribute("custid", member.CustId.ToString());
        row.SetAttribute("productid", member.ProductId.ToString());
        row.SetAttribute("transactiontime", member.TransactionTime.ToString(CultureInfo.InvariantCulture));
        row.SetAttribute("groupid", member.GroupId.ToString(CultureInfo.InvariantCulture));
        row.SetAttribute("levelid", member.LevelId.ToString(CultureInfo.InvariantCulture));
        row.SetAttribute("ticketrefno", member.TicketRefNo);
        row.SetAttribute("logid", member.LogId);
        root.AppendChild(row);

        doc.AppendChild(root);

        MemNotify = doc.InnerXml;
    }

    public string MemNotify { get; set; }

    public override string GetSettingKey()
    {
        return "NAP_MaxPayout_Notification_MemUpd";
    }

    public override bool IsValid()
    {
        return true;
    }
}