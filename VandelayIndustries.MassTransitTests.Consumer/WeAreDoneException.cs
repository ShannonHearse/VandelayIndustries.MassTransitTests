using System;

namespace VandelayIndustries.MassTransitTests.Consumer
{
    public class WeAreDoneException : Exception
    {
        public WeAreDoneException(string message) : base(message)
        {
        }
    }
}