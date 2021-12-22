using System;
using System.Collections.Generic;
using System.Text;
using Sharp.MySQL;
using Sharp.MySQL.Migrations.Core;

namespace RunStuff.SchemaChanges
{
    public class Change_01_20211220 : ISchemaChange
    {
        private ConnectionFactory factory;

        public int SchemaVersion => 1;

        public void Initialize(ConnectionFactory factory)
        {
            this.factory = factory;
        }

        public bool CanRun()
        {
            return true;
        }

        public void Run()
        {
            // Excute my stuff
        }
    }
}
