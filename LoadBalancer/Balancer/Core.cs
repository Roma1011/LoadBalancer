using System.Text;

namespace LoadBalancer.Balancer;

public sealed class Core(IHttpClientFactory httpClientFactory, RequestDelegate next)
{
    public async Task<HttpResponseMessage> Invoke(HttpContext context)
    {
        HttpClient httpClient = httpClientFactory.CreateClient();
        var response=await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(context.Request.Method.ToUpper()), 
            new Uri( new Uri("http://localhost:5165"),context.Request.Path.Value))
        {
            Content = new StringContent(await GetContentValueAsync(context.Request), Encoding.UTF8,await GetContentType(context.Request))
        });
        await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
        return response;
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