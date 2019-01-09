using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.QuartzIntegration;

// using Quartz;
// using Quartz.Impl;

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
        /*
        static void Main(string[] args)
        {
            RunProgram().GetAwaiter().GetResult();
        }

        private static async Task RunProgram()
        {
            try
            {
                await Console.Out.WriteLineAsync("Starting Quartz Scheduler");
                NameValueCollection props = new NameValueCollection
                {
                    {"quartz.serializer.type", "binary" },
                    {"quartz.scheduler.instanceName", "MyScheduler" },
                    {"quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                    {"quartz.threadPool.threadCount", "1" },
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();

                await scheduler.Start();
                await Console.Out.WriteLineAsync("Scheduler Running.  Press any key to terminate");
                Console.Read();
                await scheduler.Shutdown();

            }
            catch (Exception e)
            {
                await Console.Error.WriteLineAsync(e.ToString());
            }
        }
        */
    }
}
