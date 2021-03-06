﻿using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.MultipleDeserializers.MergedNewtonsoftEndpoint";
        #region configMergedNewtonsoft
        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.MergedNewtonsoftEndpoint");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.RegisterOutgoingMessageLogger();
        #endregion
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            var message = MesasgeBuilder.BuildMessage();
            await endpointInstance.Send("Samples.MultipleDeserializers.ReceivingEndpoint", message)
                .ConfigureAwait(false);
            Console.WriteLine("Order Sent");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}