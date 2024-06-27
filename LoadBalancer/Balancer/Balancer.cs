using System.Net;
using System.Text;

namespace LoadBalancer.Balancer;

public class Balancer(IHttpClientFactory clientFactory)
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        
        HttpClient httpClient = clientFactory.CreateClient();
        httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(context.Request.Method.ToUpper()), "http://example.com")
        {
            Content = new StringContent(, Encoding.UTF8, "application/json ")
        });
    }
    
}