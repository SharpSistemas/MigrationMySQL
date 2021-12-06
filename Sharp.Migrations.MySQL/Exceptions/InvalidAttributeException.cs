using System;

namespace Sharp.MySQL.Migrations.Exceptions
{
    public class InvalidAttributeException : Exception
    {
        public InvalidAttributeException(string message) : base(message)
        {
        }
    }
}
