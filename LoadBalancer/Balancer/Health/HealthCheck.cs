using System.Net;
using LoadBalancer.Balancer.Domain;
using LoadBalancer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LoadBalancer.Balancer.Health;

public class HealthCheck([FromServices]BalancerContext balancerContext,IOptions<ServerOptions> options, IHttpClientFactory httpClientFactory):IWorker
{
    private readonly ServerOptions _server = options.Value;
    private const string HealthCheckEndpoint = "/HealthCheck/HealthCheck";
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var url in _server.Receivers)
        {
            var httpClient = httpClientFactory.CreateClient();

            Uri endpoint = new(new Uri(url), HealthCheckEndpoint);

            try
            {
                var response = await httpClient.GetAsync(endpoint, cancellationToken);

                
                if(response.StatusCode!=HttpStatusCode.OK)
                    balancerContext.SetStatus(url,false);
                else
                    balancerContext.SetStatus(url,true);
            }
            catch (Exception e)
            {
                balancerContext.SetStatus(url,false);
            }
        }
    }
}