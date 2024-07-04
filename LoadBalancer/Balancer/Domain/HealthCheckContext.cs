using System;
using System.Net.Sockets;
using LoadBalancer.Models;

namespace LoadBalancer.Balancer.Domain;

public class HealthCheckContext
{
    public static void CheckHealth(ref ServerStatus[] serverStatusArray)
    {
        foreach (var item in serverStatusArray)
        {
            Uri uri = new Uri(item.Uri);
            string host = uri.Host;
            int port = uri.Port;
            
            using (TcpClient client = new TcpClient())
            {
                client.Connect(host, port);
                item.IsActive = client.Connected;
                Console.WriteLine($"Connected to {host}:{port}");
            }
        }
    }
}