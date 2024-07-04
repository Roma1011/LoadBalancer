using LoadBalancer.Balancer;
using LoadBalancer.Balancer.Domain;
using LoadBalancer.Balancer.Extension;
using LoadBalancer.Balancer.WorkingAlgorithm;
using LoadBalancer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


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