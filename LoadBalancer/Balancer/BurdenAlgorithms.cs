using LoadBalancer.Models;

namespace LoadBalancer.Balancer;

public class BurdenAlgorithms
{
    private static int _index = 0;
    public static Task<string> CallIt(List<RequestHistory> history,ServerStatus[] serverInfo, AlgorithmType type)
    {
        switch (type)
        {
            case AlgorithmType.Equally:
            {
                string uri=string.Empty;
                
                while (_index<serverInfo.Length)
                {
                    uri=serverInfo[_index].Uri;
                    _index++;
                    break;
                }

                if (_index == serverInfo.Length)
                    _index = 0;
                
                return Task.FromResult(uri);
            }
            case AlgorithmType.EmphasisOnTheFirst:
            {
                if (_index > serverInfo.Length / 2)
                    _index = 0;
                
                _index++;
                return Task.FromResult(serverInfo[Random.Shared.Next(0,serverInfo.Length / 2)].Uri);
            }
            case AlgorithmType.EmphasisOnTheSecond:
            {
                if (_index > serverInfo.Length)
                    _index = serverInfo.Length / 2;
                
                _index++;
                return Task.FromResult(serverInfo[Random.Shared.Next(serverInfo.Length/2,serverInfo.Length)].Uri);
            }
            default:
                return Task.FromResult(serverInfo[Random.Shared.Next(0, serverInfo.Length)].Uri);
        }
    }
}