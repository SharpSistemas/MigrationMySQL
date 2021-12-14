[![NuGet](https://buildstats.info/nuget/Sharp.MySQL.Migrations)](https://www.nuget.org/packages/Sharp.MySQL.Migrations)


# Migration - MySQL

Biblioteca simplificada para Migration em bancos de dados MySql/MariaDB

[Documentação em Progresso]


Objetivo: Criar tabelas e colunas à partir de uma Model

1. Crie uma model para representar a tabela
~~~ C#
public class Pessoas
{
    [PrimaryKey]
    [AutoIncrement]
    [TypeFieldBD(TypeField.INT, NotNull = true)]
    public int Codigo { get; set; } //INT AI PK NN

    [TypeFieldBD(TypeField.NVARCHAR, 100, NotNull = true)]
    public string Nome { get; set; } //NVARCHAR(100) NN
}
~~~

2. Inicialize nossa Factory com sua connection String
~~~ C#
var mySQLFactory = new Sharp.MySQL.ConnectionFactory(connstring);
~~~

3. Crie um Migration com a Factory, adicione models e execute a migração
~~~ C#
var migration = new Sharp.MySQL.Migration(mySQLFactory);
// Add or change tables
var result = migration.Add<Pessoas>()
                      .Migrate();
~~~

* A tabela será criada no banco
* Caso faltem colunas, as novas colunas são criadas
* Utlize o Result para verificar se houveram modificações no banco


Veja o projeto de exemplo em: [MigrationMySQL/RunStuff](https://github.com/SharpSistemas/MigrationMySQL/tree/main/RunStuff)
