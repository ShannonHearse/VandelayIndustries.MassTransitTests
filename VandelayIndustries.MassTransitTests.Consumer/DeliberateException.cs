using System;

namespace VandelayIndustries.MassTransitTests.Consumer
{
    public class DeliberateException : Exception
    {
        public DeliberateException(string message) : base(message)
        {
        }
    }
}
