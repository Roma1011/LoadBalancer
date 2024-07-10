namespace LoadBalancer.Balancer.Health;

public sealed class WorkerManager(IEnumerable<IWorker> backgroundServices) : BackgroundService
{
    private static readonly TimeSpan IntervalMilliSeconds = new(0, 0, 0, 2, 0);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var services = backgroundServices.Select(service =>
            LoopService(service, cancellationToken));
        await Task.WhenAll(services);
    }

    private static async Task LoopService(IWorker worker, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await worker.StartAsync(cancellationToken);

            await Task.Delay(IntervalMilliSeconds, cancellationToken);
        }
    }
}