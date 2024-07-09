using LoadBalancer.Balancer.Domain;
using LoadBalancer.Balancer.Extension;
using LoadBalancer.Balancer.WorkingAlgorithm;
using LoadBalancer.Models;;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<ServerOptions>(builder.Configuration.GetSection(nameof(ServerOptions)));
builder.Services.AddSingleton<BalancerContext>();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
BalancerBuilderExtension.UseLoadBalancing(app,AlgorithmType.EmphasisOnTheFirst);
app.Run();