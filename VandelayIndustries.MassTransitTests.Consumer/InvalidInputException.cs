using System;

namespace VandelayIndustries.MassTransitTests.Consumer
{
    /// <summary>
    /// This message should be thrown if the data contained in the message can never be processed due to validation or similar issues.
    /// </summary>
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message)
        {
        }
    }
}
