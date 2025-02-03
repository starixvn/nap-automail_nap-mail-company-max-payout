using Fanex.RemotingRef;

namespace Fanex.CompanyMaxPayout.Business.Services;

public interface CacheRefService
{
    string GetBettypeName(string bettypeId);
}

public class CacheRefServiceImp(IRemotingRefService remotingRefService) : CacheRefService
{
    private const string LangEn = "en";

    public string GetBettypeName(string bettypeId)
    {
        return remotingRefService.GetBetTypeName(bettypeId, LangEn);
    }
}