using System;
using VandelayIndustries.MassTransitTests.Contracts.DataModels;
using VandelayIndustries.MassTransitTests.Contracts.Events;
using Microsoft.AspNetCore.Mvc;
using MassTransit;

namespace VandelayIndustries.MassTransitTests.Api.Controllers
{
    [Route("api/MassTransitTests")]
    [ApiController]
    public class MassTransitTestsController : ControllerBase
    {
        [HttpPost()]
        public Guid CreateLegoMan([FromBody] CreateLegoMan legoMan)
        {
            Console.WriteLine($"Attempting to create lego man with name of {legoMan.Name}");
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            busControl.Start();

            var legoManCreated = new LegoManCreated
            {
                Id = Guid.NewGuid(),
                HatType = legoMan.HatType,
                Name = legoMan.Name,
                PantsColour = legoMan.PantsColour,
                Action = legoMan.Action
            };

            busControl.Publish<LegoManCreated>(legoManCreated);
            Console.WriteLine("Lego man created");
            return legoManCreated.Id;
        }
    }
}
