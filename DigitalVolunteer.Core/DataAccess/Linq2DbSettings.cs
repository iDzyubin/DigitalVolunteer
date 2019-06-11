using System;
using System.Collections.Generic;
using LinqToDB.Configuration;

namespace DigitalVolunteer.Core.DataAccess
{
    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }

    public class Linq2DbSettings : ILinqToDBSettings
    {
        string _connStr;
        public Linq2DbSettings( string connStr )
        {
            _connStr = connStr ?? throw new ArgumentNullException( nameof( connStr ) );
        }

        public IEnumerable<IDataProviderSettings> DataProviders
        {
            get { yield break; }
        }

        public string DefaultConfiguration => "PostgreSQL";
        public string DefaultDataProvider => "PostgreSQL";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = "PostgreSQL",
                        ProviderName = "PostgreSQL",
                        ConnectionString = _connStr
                    };
            }
        }
    }
}
