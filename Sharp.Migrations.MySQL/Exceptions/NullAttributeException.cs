using System;

namespace Sharp.Migrations.MySQL.Exceptions
{
    public class NullAttributeException : Exception
    {
        public NullAttributeException(string message) : base(message)
        {
        }
    }
}
