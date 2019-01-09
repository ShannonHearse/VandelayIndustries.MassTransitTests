using System;

namespace VandelayIndustries.MassTransitTests.Consumer
{
    /// <summary>
    /// This message is a placeholder, and represents an unexpected error (eg, network down, database deadlock, etc, etc) occurs.
    /// </summary>
    public class TransientException : Exception
    {
        public TransientException(string message) : base(message)
        {
        }
    }
}