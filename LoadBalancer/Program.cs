using LoadBalancer.Balancer;
using LoadBalancer.Balancer.Extension;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Balancer>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
BalancerBuilderExtension.UseLoadBalancing(app);
app.Run();