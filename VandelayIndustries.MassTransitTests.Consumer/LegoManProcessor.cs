using System;
using System.Threading.Tasks;
using VandelayIndustries.MassTransitTests.Contracts.Events;
using MassTransit;

namespace VandelayIndustries.MassTransitTests.Consumer
{
    public class LegoManProcessor : IConsumer<LegoManCreated>
    {
        public async Task Consume(ConsumeContext<LegoManCreated> context)
        {
            await Console.Out.WriteLineAsync($"Executing post creation process for lego name with name of {context.Message.Name}");
            var redeliveryCount = int.Parse(context.Headers.Get("MT-Redelivery-Count", "0"));
            await Console.Out.WriteLineAsync($"Redelivery Count = {redeliveryCount}");

            if (context.Message.Action != null)
            {
                if (context.Message.Action.ToLower().Contains("exception"))
                {
                    await Console.Out.WriteLineAsync("Throwing Exception");
                    throw new DeliberateException("Lego Man is an exception");
                }

                if (context.Message.Action.ToLower().Contains("defer"))
                {
                    await Console.Out.WriteLineAsync("Deferring");
                    // Note: Defer does work unless you have the rabbitmq_delayed_message_exchange on the relevant rabbit mq server.
                    await context.Defer(TimeSpan.FromSeconds(10));
                    await Console.Out.WriteLineAsync("Deferred");
                    return;
                }

                if (context.Message.Action.ToLower().Contains("redeliver"))
                {
                    await Console.Out.WriteLineAsync("Redelivering");
                    if (redeliveryCount > 5)
                    {
                        // We throw this exception, On ConsumerConfiguration, we have said to ignore this exception, therefor it goes straight to the relevant error queue.
                        // Is there a better way to to this?  Can we go context.DeliverToErrorQueue() or similar?
                        throw new WeAreDoneException("We are done peoples");
                    }
                    await context.Redeliver(TimeSpan.FromSeconds(5));
                    await Console.Out.WriteLineAsync("Redelivered!");
                    return;
                }
            }

            await Console.Out.WriteLineAsync($"Post Creation Process for Lego man with name of {context.Message.Name} was executed.");
        }
    }
}
