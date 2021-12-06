using System;

namespace Sharp.MySQL.Migrations.Exceptions
{
    /// <summary>
    /// Exception when is detect an invalid use of a property
    /// </summary>
    public class InvalidAttributeException : Exception
    {
        /// <summary>
        /// Constructor of the exception
        /// </summary>
        /// <param name="message">Message to show when the exception is thrown</param>
        public InvalidAttributeException(string message) : base(message)
        {
            
        }
    }
}
