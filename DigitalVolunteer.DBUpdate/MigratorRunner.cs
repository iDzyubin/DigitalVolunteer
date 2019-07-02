using System.Reflection;
using Npgsql;
using ThinkingHome.Migrator;

namespace DigitalVolunteer.DBUpdate
{
    public class MigratorRunner
    {
        private NpgsqlConnection DbConnection;
        private string FactoryType = "postgres";
        private Assembly Asm;

        public MigratorRunner( string cnnString )
        {
            this.Asm = GetType().Assembly;
            this.DbConnection = new NpgsqlConnection( cnnString );
        }

        public void Run()
        {
            var migrator = new Migrator(FactoryType, DbConnection, Asm, null);
            migrator.Migrate();
        }
    }
}
