using System;

namespace Sharp.MySQL.Migrations.Exceptions
{    /// <summary>
     /// Exception when is detect an null property
     /// </summary>
    public class NullAttributeException : Exception
    {
        /// <summary>
        /// Constructor of the exception
        /// </summary>
        /// <param name="message">Message to show when the exception is thrown</param>
        public NullAttributeException(string message) : base(message)
        {
        }
    }
}
