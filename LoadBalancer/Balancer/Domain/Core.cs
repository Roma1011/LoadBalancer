using System.Net;
using System.Text;
using LoadBalancer.Balancer.WorkingAlgorithm;
using LoadBalancer.Models;

namespace LoadBalancer.Balancer.Domain;

internal sealed class Core(BalancerContext balancercontext,AlgorithmType algorithmType,IHttpClientFactory httpClientFactory,RequestDelegate next)
{
    public static int x = 1;
    public static int i = 0;
    public async Task Invoke(HttpContext context)
    {
        string uri= await balancercontext.BalanceIt(algorithmType);
        if (uri == string.Empty)
        {
            if (i > x)
            {
                context.Response.StatusCode = StatusCodes.Status502BadGateway;
                return;
            }

            i++;
            await this.Invoke(context);
        }
        
        HttpClient httpClient = httpClientFactory.CreateClient();
        
        var responseMessage=await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(context.Request.Method.ToUpper()), 
            new Uri( new Uri(uri),context.Request.Path.Value))
        {
            Content = new StringContent(await GetContentValueAsync(context.Request), Encoding.UTF8,await GetContentType(context.Request))
        });
        await context.Response.WriteAsync(await responseMessage.Content.ReadAsStringAsync());
    
        balancercontext.SaveHistory(new RequestHistory(uri,DateTime.Now));
        
        return ;
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