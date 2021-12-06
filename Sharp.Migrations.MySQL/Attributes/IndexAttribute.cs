using System;

namespace Sharp.Migrations.MySQL.Attributes
{
    [Obsolete("Não há suporte para criação de index no momento.", true)]
    public class IndexAttribute : Attribute
    {
        public string IndexName { get; set; }
        public IndexAttribute(string indexName)
        {
            IndexName = indexName;
        }
    }
}
