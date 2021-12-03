using System;

namespace MigrationTests.Exceptions
{
    public class NullAttributeException : Exception
    {
        public NullAttributeException(string message) : base(message)
        {
        }
    }
}
