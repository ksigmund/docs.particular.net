﻿using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Store.Messages.RequestResponse;

public class ProvisionDownloadHandler : IHandleMessages<ProvisionDownloadRequest>
{
    static ILog log = LogManager.GetLogger<ProvisionDownloadHandler>();

    public Task Handle(ProvisionDownloadRequest message, IMessageHandlerContext context)
    {
        if (DebugFlagMutator.Debug)
        {
            Debugger.Break();
        }

        log.Info("Provision the products and make the Urls available to the Content management for download ...[{string.Join(", ", message.ProductIds)}] product(s) to provision");

        return context.Reply(new ProvisionDownloadResponse
            {
                OrderNumber = message.OrderNumber,
                ProductIds = message.ProductIds,
                ClientId = message.ClientId
            });
    }
}
