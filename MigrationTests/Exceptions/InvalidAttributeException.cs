using System;

namespace MigrationTests.Exceptions
{
    public class InvalidAttributeException : Exception
    {
        public InvalidAttributeException(string message) : base(message)
        {
        }
    }
}
