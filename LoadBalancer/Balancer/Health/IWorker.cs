namespace LoadBalancer.Balancer.Health;

public interface IWorker
{
    Task StartAsync(CancellationToken cancellationToken);
}