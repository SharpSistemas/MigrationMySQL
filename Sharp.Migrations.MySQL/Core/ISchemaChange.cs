using System;
using System.Collections.Generic;
using System.Text;

namespace Sharp.MySQL.Migrations.Core
{
    public interface ISchemaChange
    {
        int SchemaVersion { get; }

        void Initialize(ConnectionFactory factory);
        bool CanRun();
        void Run();
    }
}
