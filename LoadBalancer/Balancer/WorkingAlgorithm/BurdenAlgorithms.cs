﻿using System;
using System.Threading.Tasks;
using LoadBalancer.Models;

namespace LoadBalancer.Balancer.WorkingAlgorithm;

public class BurdenAlgorithms
{
    private static int _index = 0;
    public static Task<string> CallIt(ServerStatus[] serverInfo, AlgorithmType type)
    {
        switch (type)
        {
            case AlgorithmType.Equally:
            {
                string uri=string.Empty;
                
                while (_index<serverInfo.Length)
                {
                    if (serverInfo[_index].IsActive == false)
                        goto CONTINUE;
                    
                    uri=serverInfo[_index].Uri;
                    
                    CONTINUE:
                    _index++;
                    break;
                }

                if (_index == serverInfo.Length)
                    _index = 0;
                
                return Task.FromResult(uri);
            } break;
            //------------------------------------------------------------------------------------------------------------            
            case AlgorithmType.EmphasisOnTheFirst:
            {
                if (_index > serverInfo.Length / 2)
                    _index = 0;
                
                if(serverInfo[_index].IsActive==false)
                    break;
                
                _index++;
                
                return Task.FromResult(serverInfo[Random.Shared.Next(0,serverInfo.Length / 2)].Uri);
            } break;
            //------------------------------------------------------------------------------------------------------------             
            case AlgorithmType.EmphasisOnTheSecond:
            {
                if (_index > serverInfo.Length)
                    _index = serverInfo.Length / 2;
                
                if(serverInfo[_index].IsActive==false)
                    break;
                
                _index++;
                return Task.FromResult(serverInfo[Random.Shared.Next(serverInfo.Length/2,serverInfo.Length)].Uri);
            } break;
            //------------------------------------------------------------------------------------------------------------             
            default:
                return Task.FromResult(serverInfo[Random.Shared.Next(0, serverInfo.Length)].Uri);
        }

        return Task.FromResult(string.Empty);
    }
}