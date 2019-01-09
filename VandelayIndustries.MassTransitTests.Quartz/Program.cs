using System;
using MassTransit;

namespace VandelayIndustries.MassTransitTests.Quartz
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MassTransit Quartz Scheduler Service!");
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.UseInMemoryScheduler();
            });

            busControl.Start();

            Console.WriteLine("Press any key to terminate!");
            Console.ReadKey();

            busControl.Stop();
        }
    }
}
