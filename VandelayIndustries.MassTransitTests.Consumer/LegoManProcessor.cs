using System;
using System.Threading.Tasks;
using VandelayIndustries.MassTransitTests.Contracts.Events;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace VandelayIndustries.MassTransitTests.Consumer
{
    public class LegoManProcessor : IConsumer<LegoManCreated>
    {
        public async Task Consume(ConsumeContext<LegoManCreated> context)
        {
            await Console.Out.WriteLineAsync($"Executing post creation process for lego name with name of {context.Message.Name}");
            var redeliveryCount = int.Parse(context.Headers.Get("MT-Redelivery-Count", "0"));
            await Console.Out.WriteLineAsync($"Redelivery Count = {redeliveryCount}");

            if (context.Message.Action.ToLower().Contains("InvalidInput".ToLower()))
                throw new InvalidInputException("Input was Invalid.  Straight to error queue with you.");
            if (context.Message.Action.ToLower().Contains("PrerequisiteNotYetCreated".ToLower()))
                throw new PrerequisiteNotYetCreatedException("Relevant Prerequisite Not Yet Created. Redeliver later.  Give the Prereq a chance to happen.  Eventually get to the error queue.");
            if (context.Message.Action.ToLower().Contains("TransientException".ToLower()))
                throw new TransientException("Transient Error.  Immediate retries for you.  If they don't work, Redeliver and Immediate retries.  Eventually get to the error queue.");

            await Console.Out.WriteLineAsync("Message successfully processed");
        }
    }
}
