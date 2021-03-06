using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        Start().GetAwaiter().GetResult();
    }

    static async Task Start()
    {
        Console.Title = "Samples.PipelineHandlerTimer";
        var endpointConfiguration = new EndpointConfiguration("Samples.PipelineHandlerTimer");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            await Run(endpointInstance)
                .ConfigureAwait(false);
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }

    static async Task Run(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'Enter' to send a Message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                await SendMessage(endpointInstance)
                    .ConfigureAwait(false);
                continue;
            }
            return;
        }
    }

    static Task SendMessage(IEndpointInstance endpointInstance)
    {
        Console.WriteLine();
        Console.WriteLine("Message sent");
        var message = new Message();
        return endpointInstance.SendLocal(message);
    }

}