using Fanex.CompanyMaxPayout.Business.Criteria;
using Fanex.CompanyMaxPayout.Business.Models;
using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.CompanyMaxPayout.Business.Services;

public interface DataService
{
    List<MemberModel> QueryMembers();

    void UpdateNotifyMember(MemberModel member);
}

public class DataServiceImp : DataService
{
    private readonly IDynamicRepository repository;

    public DataServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public List<MemberModel> QueryMembers()
    {
        var criteria = new MaxPayoutMemSelCriteria
        {
            ProductId = AppConfigService.Config.ProductId,
            Username = AppConfigService.Config.UserName
        };

        var result = repository.Fetch<MemberModel>(criteria).ToList();
        return result;
    }

    public void UpdateNotifyMember(MemberModel member)
    {
        var command = new MaxPayoutMemUpdCommand(member);
        repository.Execute(command);
    }
}