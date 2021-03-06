﻿using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Gateway.RemoteSite";
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Gateway.RemoteSite");
        configure.DefaultBuilder();
        configure.RunGatewayWithInMemoryPersistence();
        configure.UseInMemoryGatewayDeduplication();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}