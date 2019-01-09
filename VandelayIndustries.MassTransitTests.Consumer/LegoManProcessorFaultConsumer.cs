using System;
using System.Threading.Tasks;
using MassTransit;
using VandelayIndustries.MassTransitTests.Contracts.Events;

namespace VandelayIndustries.MassTransitTests.Consumer
{
    public class LegoManProcessorFaultConsumer : IConsumer<Fault<LegoManCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<LegoManCreated>> context)
        {
            var originalMessage = context.Message?.Message;
            var exceptions = context.Message?.Exceptions;
            await Console.Out.WriteLineAsync($"Failed to finish creation process for lego man with name of {context.Message?.Message?.Name}");
        }
    }
}