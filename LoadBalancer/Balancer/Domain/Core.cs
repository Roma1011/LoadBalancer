using System.Net;
using System.Text;
using LoadBalancer.Balancer.WorkingAlgorithm;
using LoadBalancer.Models;

namespace LoadBalancer.Balancer.Domain;

internal sealed class Core(BalancerContext balancercontext,AlgorithmType algorithmType,IHttpClientFactory httpClientFactory,RequestDelegate next)
{
    public async Task<HttpResponseMessage> Invoke(HttpContext context)
    {
        string uri= await balancercontext.BalanceIt(algorithmType);
        if (uri == string.Empty)
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        
        HttpClient httpClient = httpClientFactory.CreateClient();
        HttpResponseMessage message=new HttpResponseMessage();
        
        var responseHealth=await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(context.Request.Method.ToUpper()), 
            new Uri(new Uri(uri),"/HealthCheck/HealthCheck"))
        {
            Content = new StringContent(await GetContentValueAsync(context.Request), Encoding.UTF8,await GetContentType(context.Request))
        });
        
        if (responseHealth.StatusCode == HttpStatusCode.OK)
        {
            message=await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(context.Request.Method.ToUpper()), 
                new Uri( new Uri(uri),context.Request.Path.Value))
            {
                Content = new StringContent(await GetContentValueAsync(context.Request), Encoding.UTF8,await GetContentType(context.Request))
            });
            await context.Response.WriteAsync(await message.Content.ReadAsStringAsync());
        
            balancercontext.SaveHistory(new RequestHistory(uri,DateTime.Now));
        }
        else
        {
            balancercontext.StatusInactive(uri);
        }
        return message;
    }

    private static async Task<string> GetContentValueAsync(HttpRequest request)
    {
        using var reader =new StreamReader(request.Body, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }

    private static Task<string> GetContentType(HttpRequest request)
    {
        if (string.IsNullOrEmpty(request.ContentType))
            return Task.FromResult("application/octet-stream");

        return Task.FromResult(request.ContentType);
    }
}