using Fanex.CompanyMaxPayout.Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.CompanyMaxPayout.Business.Services;

public interface TransformService
{
    List<MemberModel> Execute(List<MemberModel> members);
}

public class TransformServiceImp(CacheRefService cacheRefService) : TransformService
{
    private readonly List<string> happy5Bettypes = ["8101", "8102", "8103", "8104", "8105"];
    private const string Happy5Sport = "(Happy 5)";

    public List<MemberModel> Execute(List<MemberModel> members)
    {
        var testCurrencyList = new List<string> { "uus", "yng" };
        var data = AppConfigService.Config.IsRealCurrency
            ? members.Where(s => !testCurrencyList.Contains(s.Currency.ToLowerInvariant())).ToList()
            : members.Where(s => testCurrencyList.Contains(s.Currency.ToLowerInvariant())).ToList();

        data.ForEach(member =>
        {
            member.FullGameTypes = ParseGameTypeList(member.BettypeIdList);
            member.ShortGameTypes = ParseShortGameTypeList(member.BettypeIdList);
        });

        return data;
    }

    private List<string> ParseGameTypeList(string bettypeIdList)
    {
        var gameTypes = new List<string>();
        if (string.IsNullOrEmpty(bettypeIdList))
        {
            return [];
        }

        var ids = new List<string>(bettypeIdList.Split(','));
        ids.ForEach(id =>
        {
            var sportName = ParseHappy5BettypeSporttype(id);
            gameTypes.Add(cacheRefService.GetBettypeName(id) + " " + sportName);
        });

        return gameTypes;
    }

    private List<string> ParseShortGameTypeList(string bettypeIdList)
    {
        const int maxIndex = 3;
        var gameTypes = new List<string>();
        if (string.IsNullOrEmpty(bettypeIdList))
        {
            return [];
        }

        var ids = new List<string>(bettypeIdList.Split(','));
        foreach (var id in ids)
        {
            if (gameTypes.Count > maxIndex)
            {
                if (!gameTypes.Contains("and other games"))
                {
                    gameTypes.Add("and other games...");
                }
            }
            else
            {
                var sportName = ParseHappy5BettypeSporttype(id);
                gameTypes.Add(cacheRefService.GetBettypeName(id) + " " + sportName);
            }
        }

        return gameTypes;
    }

    private string ParseHappy5BettypeSporttype(string id)
    {
        return happy5Bettypes.Contains(id) ? $"{Happy5Sport}" : string.Empty;
    }
}