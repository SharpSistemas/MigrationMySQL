using System;

namespace Sharp.Migrations.MySQL.Exceptions
{
    public class InvalidAttributeException : Exception
    {
        public InvalidAttributeException(string message) : base(message)
        {
        }
    }
}
