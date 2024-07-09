using LoadBalancer.Balancer.Domain;
using LoadBalancer.Balancer.WorkingAlgorithm;

namespace LoadBalancer.Balancer.Extension;

public static class BalancerBuilderExtension
{
    public static IApplicationBuilder UseLoadBalancing(this IApplicationBuilder builder,AlgorithmType algType)
    {
        builder.UseMiddleware<Core>(algType);
        return builder;
    }
}