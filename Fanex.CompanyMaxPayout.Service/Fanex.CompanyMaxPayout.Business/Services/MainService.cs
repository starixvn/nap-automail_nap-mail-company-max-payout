using Newtonsoft.Json;
using System;

namespace Fanex.CompanyMaxPayout.Business.Services;

public interface MainService
{
    void Execute();
}

public class MainServiceImp(
    MessageService messageService,
    Log log,
    DataService dataService,
    TransformService transformService)
    : MainService
{
    public void Execute()
    {
        try
        {
            var rawData = dataService.QueryMembers();
            var members = transformService.Execute(rawData);

            members.ForEach(member =>
            {
                var result = messageService.Send(member);
                if (result)
                {
                    dataService.UpdateNotifyMember(member);
                }
                else
                {
                    var msg =
                        $"Fanex.CompanyMaxPayout.Service FAILED {JsonConvert.SerializeObject(member, Formatting.Indented)}";
                    log.Log(new Exception(msg));
                }
            });
        }
        catch (Exception exception)
        {
            log.Log(exception);
        }
    }
}