using LoadBalancer.Balancer;
using LoadBalancer.Balancer.Extension;
using LoadBalancer.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ServerOptions>(builder.Configuration.GetSection(nameof(ServerOptions)));
builder.Services.AddSingleton<BalancerContext>();
builder.Services.AddHttpClient();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
BalancerBuilderExtension.UseLoadBalancing(app,AlgorithmType.Equally);
app.Run();