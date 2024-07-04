﻿using LoadBalancer.Balancer.Domain;
using LoadBalancer.Balancer.WorkingAlgorithm;
using Microsoft.AspNetCore.Builder;

namespace LoadBalancer.Balancer.Extension;

public static class BalancerBuilderExtension
{
    public static IApplicationBuilder UseLoadBalancing(this IApplicationBuilder builder,AlgorithmType algType)
    {
        builder.UseMiddleware<Core>(algType);
        return builder;
    }
}