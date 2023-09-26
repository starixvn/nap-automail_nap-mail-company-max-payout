using Fanex.RemotingRef;

namespace Fanex.CompanyMaxPayout.Business.Services;

public interface CacheRefService
{
    string GetBettypeName(string bettypeId);
}

public class CacheRefServiceImp : CacheRefService
{
    private const string LangEn = "en";
    private readonly IRemotingRefService remotingRefService;

    public CacheRefServiceImp(IRemotingRefService remotingRefService)
    {
        this.remotingRefService = remotingRefService;
    }

    public string GetBettypeName(string bettypeId)
    {
        return remotingRefService.GetBetTypeName(bettypeId, LangEn);
    }
}