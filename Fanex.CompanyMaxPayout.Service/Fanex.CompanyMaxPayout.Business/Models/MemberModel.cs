using System;
using System.Collections.Generic;

namespace Fanex.CompanyMaxPayout.Business.Models;

public class MemberModel
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int CustId { get; set; }

    public string UserName { get; set; }

    public string Currency { get; set; }

    public string BettypeIdList { get; set; }

    public string Site { get; set; }

    public string TicketRefNo { get; set; }

    public DateTime TransactionTime { get; set; }

    public decimal ActualPayout { get; set; }

    public decimal CompanyMaxPayout { get; set; }

    public decimal LevelId { get; set; }

    public decimal GroupId { get; set; }

    public string LogId { get; set; }

    public List<string> FullGameTypes { get; set; }

    public List<string> ShortGameTypes { get; set; }
}