using LoadBalancer.Balancer;
using LoadBalancer.Balancer.Extension;
using LoadBalancer.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<ServerOptions>(builder.Configuration.GetSection(nameof(ServerOptions)));
builder.Services.AddSingleton<BalancerContext>();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
BalancerBuilderExtension.UseLoadBalancing(app,AlgorithmType.EmphasisOnTheSecond);
app.Run();