using System;

namespace VandelayIndustries.MassTransitTests.Consumer
{
    public class DeliberateException : Exception
    {
        public DeliberateException(string message) : base(message)
        {
        }
    }

    public class WeAreDoneException : Exception
    {
        public WeAreDoneException(string message) : base(message)
        {
        }
    }
}
