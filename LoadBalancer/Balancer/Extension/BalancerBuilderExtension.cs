namespace LoadBalancer.Balancer.Extension;

public static class BalancerBuilderExtension
{
    public static IApplicationBuilder UseLoadBalancing(this IApplicationBuilder builder,AlgorithmType algType)
    {
        builder.UseMiddleware<Core>(algType);
        return builder;
    }
}