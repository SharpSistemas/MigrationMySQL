using System;

namespace Sharp.MySQL.Migrations.Exceptions
{
    public class NullAttributeException : Exception
    {
        public NullAttributeException(string message) : base(message)
        {
        }
    }
}
