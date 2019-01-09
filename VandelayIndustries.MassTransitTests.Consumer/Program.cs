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
                    endpointConfigurator.Consumer<LegoManProcessor>(consumerConfigurator =>
                    {
                        consumerConfigurator.Message<LegoManCreated>(consumerConfig =>
                        {
                            // For the below config, we want:
                            //    InvalidInputException to go straight to error queue
                            //    PrerequisiteNotYetCreatedException to go straight to Scheduled Redelivery (Second Level Retries)
                            //    All other exceptions (should be mainly Transient) to go through Retries and Scheduled Redelivery.

                            consumerConfig.UseScheduledRedelivery(scheduledRedeliveryConfiguration =>
                            {
                                scheduledRedeliveryConfiguration.Incremental(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3));
                                scheduledRedeliveryConfiguration.Ignore<InvalidInputException>();
                            });

                            consumerConfig.UseRetry(retryConfig =>
                            {
                                // Immediate Retries of 5 left in, as we want this to work for exceptions that re not 'WeAreDoneException'
                                retryConfig.Immediate(5);
                                // This is to ensure if the LegoManCreated Consumer throws a 'WeAreDone' exception, then it goes straight to the Error Queue.
                                retryConfig.Ignore<PrerequisiteNotYetCreatedException>();
                                retryConfig.Ignore<InvalidInputException>();
                            });
                        });
                    });
                });
            });

            busControl.Start();

            Console.ReadKey();

            busControl.Stop();
        }
    }
}
