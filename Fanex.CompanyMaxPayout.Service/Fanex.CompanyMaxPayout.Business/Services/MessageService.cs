using Fanex.CompanyMaxPayout.Business.Models;
using Fanex.LetstalkService.Client;
using Fanex.LetstalkService.Client.Models;
using System;
using System.Collections.Generic;

namespace Fanex.CompanyMaxPayout.Business.Services;

public interface MessageService
{
    bool Send(MemberModel member);
}

public class MessageServiceImp(LetstalkClientService letstalkService) : MessageService
{
    public bool Send(MemberModel member)
    {
        var letstalkModel = new LetstalkModel
        {
            TargetName = $"{AppConfigService.Config.TargetName}",
            Subject = $"{AppConfigService.Config.Environment}Company Max Payout Alert",
            Content = TransformMessage(member),
            Group = AppConfigService.Config.LetstalkGroup
        };

        return letstalkService.SendMessage(letstalkModel);
    }

    private static string TransformMessage(MemberModel member)
    {
        var content = new List<string>
        {
            $"Product: {member.ProductName}",
            $"Game Type: {string.Join(", ", member.ShortGameTypes)}",
            $"Username: {member.UserName}",
            $"Site: {member.Site}",
            $"Ticket Ref No: {member.TicketRefNo}",
            $"Transaction Time: {member.TransactionTime:MM/dd/yyyy HH:mm:ss}",
            $"Actual Payout: {member.ActualPayout:#,###.00} ({member.Currency})",
            $"Company Max Payout: {member.CompanyMaxPayout:#,###.00} ({member.Currency})",
            "Please check and carry out timely actions accordingly."
        };

        return string.Join(Environment.NewLine, content);
    }
}