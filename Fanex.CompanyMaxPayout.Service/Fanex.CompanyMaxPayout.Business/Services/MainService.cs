using Newtonsoft.Json;
using System;

namespace Fanex.CompanyMaxPayout.Business.Services;

public interface MainService
{
    void Execute();
}

public class MainServiceImp : MainService
{
    private readonly MessageService messageService;
    private readonly Log log;
    private readonly DataService dataService;
    private readonly TransformService transformService;

    public MainServiceImp(
        MessageService messageService,
        Log log,
        DataService dataService,
        TransformService transformService)
    {
        this.messageService = messageService;
        this.log = log;
        this.dataService = dataService;
        this.transformService = transformService;
    }

    public void Execute()
    {
        try
        {
            var rawData = dataService.QueryMembers();
            var members = transformService.Execute(rawData);
            log.Log($"Fanex.CompanyMaxPayout.Service [{members.Count}]");
            members.ForEach(member =>
            {
                var result = messageService.Send(member);
                if (result)
                {
                    dataService.UpdateNotifyMember(member);
                }
                else
                {
                    var msg = $"Fanex.CompanyMaxPayout.Service FAILED {JsonConvert.SerializeObject(member, Formatting.Indented)}";
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