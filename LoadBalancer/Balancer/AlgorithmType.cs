namespace LoadBalancer.Balancer;

public enum AlgorithmType
{
    /// <summary>
    /// This value of the algorithm means equal distribution among recipients
    /// </summary>
    Equally=0,
    /// <summary>
    /// Most of the load distribution to the first receiver
    /// </summary>
    EmphasisOnTheFirst =1,
    /// <summary>
    /// Most of the load distribution to the second receiver
    /// </summary>
    EmphasisOnTheSecond =2,
    /// <summary>
    /// Distribution will be done on a random basis among the recipients
    /// </summary>
    Default =3,
}