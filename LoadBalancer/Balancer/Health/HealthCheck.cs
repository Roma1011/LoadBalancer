﻿using System.Net;
using LoadBalancer.Balancer.Domain;
using LoadBalancer.Models;
using Microsoft.Extensions.Options;

namespace LoadBalancer.Balancer.Health;

public class HealthCheck(IOptions<ServerOptions> options, IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider):IWorker
{
    private readonly ServerOptions _server = options.Value;
    private const string HealthCheckEndpoint = "/HealthCheck/HealthCheck";
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var loadBalancerManager = scope.ServiceProvider.GetRequiredService<BalancerContext>();
        
        foreach (var url in _server.Receivers)
        {
            var httpClient = httpClientFactory.CreateClient();

            Uri endpoint = new(new Uri(url), HealthCheckEndpoint);

            try
            {
                var response = await httpClient.GetAsync(endpoint, cancellationToken);

                
                if(response.StatusCode!=HttpStatusCode.OK)
                    loadBalancerManager.SetStatus(url,false);
                else
                    loadBalancerManager.SetStatus(url,true);
            }
            catch (Exception e)
            {
                loadBalancerManager.SetStatus(url,false);
            }
        }
    }
}