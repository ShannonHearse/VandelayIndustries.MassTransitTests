using System;

namespace VandelayIndustries.MassTransitTests.Consumer
{
    /// <summary>
    ///  This exception (or a more specific derivative) should be thrown upon experiencing out of order message issues.  Ones that will be solved once another message is processed.
    /// </summary>
    public class PrerequisiteNotYetCreatedException : Exception
    {
        public PrerequisiteNotYetCreatedException(string message) : base(message)
        {
        }
    }
}
