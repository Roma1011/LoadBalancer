using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoadBalancer.Models;
using Microsoft.Extensions.Options;

namespace LoadBalancer.Balancer;

public class BalancerContext
{
    private readonly ServerStatus[] _serverInfo;
    private readonly List<RequestHistory> _requestHistory=[];
    public BalancerContext(IOptions<ServerOptions> servOptions)
    {
        Array.Resize(ref _serverInfo,servOptions.Value.Receivers.Length);
        for (int i = 0; i < servOptions.Value.Receivers.Length; i++)
        {
            _serverInfo.SetValue(new ServerStatus(){ Uri = servOptions.Value.Receivers[i], IsActive = false },i);
        }
    }

    public async Task<string> BalanceIt(AlgorithmType algType)
    {
        return await BurdenAlgorithms.CallIt(_serverInfo, algType);
    }
    
    public void SaveHistory(RequestHistory history)
    {
        _requestHistory.Add(history);
    }
}