using System;
using VandelayIndustries.MassTransitTests.Contracts.Events;
using GreenPipes;
using MassTransit;

namespace VandelayIndustries.MassTransitTests.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mass Transit Tests Consumer!");
            Console.WriteLine("Press any key to terminate!");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.UseMessageScheduler(new Uri("rabbitmq://localhost/quartz"));

                cfg.ReceiveEndpoint(host, "lego_man_queue", endpointConfigurator =>
                {
                    // The 'UseScheduledRedelivery seems to override this.  Also, seems to duplicate when you call 'Schedule redelivery'.
                    
                    
                    endpointConfigurator.Consumer<LegoManProcessor>(consumerConfigurator =>
                    {
                        consumerConfigurator.Message<LegoManCreated>(consumerConfig =>
                        {
                            // Note: Order of config seems to be important, move the useRetry to above UseScheduledDelivery, and the immediate retries seems to disappear.
                            consumerConfig.UseScheduledRedelivery(Retry.Incremental(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3)));
                            consumerConfig.UseRetry(retryConfig =>
                            {
                                retryConfig.Immediate(5);
                                retryConfig.Ignore<WeAreDoneException>();
                            });
                        });
                    });
                    
                    // e.Consumer<LegoManProcessor>();
                    endpointConfigurator.Consumer<LegoManProcessorFaultConsumer>();
                });
                // The below is for context.Defer, which also needs a specific plugin.
                // cfg.UseDelayedExchangeMessageScheduler();
            });

            busControl.Start();

            Console.ReadKey();

            busControl.Stop();
        }
    }
}
