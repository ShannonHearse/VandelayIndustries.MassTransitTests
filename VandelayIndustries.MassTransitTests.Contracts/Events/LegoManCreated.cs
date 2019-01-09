using System;

namespace VandelayIndustries.MassTransitTests.Contracts.Events
{
    public class LegoManCreated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PantsColour { get; set; }
        public string HatType { get; set; }
        public string Action { get; set; }
    }
}
