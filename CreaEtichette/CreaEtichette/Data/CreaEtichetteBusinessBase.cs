using Oracle.ManagedDataAccess.Client;
using CreaEtichette.Data.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreaEtichette.Data
{
    public class CreaEtichetteBusinessBase : BusinessBase
    {
        protected static string ConnectionName
        {
            get
            {
                return "RVL";
            }
        }

        protected static string ConnectionString
        {
            get
            {
                
                ConnectionStringSettings c = ConfigurationManager.ConnectionStrings[ConnectionName];
                return c.ConnectionString;
            }
        }
        protected string _connectionString;
        public CreaEtichetteBusinessBase()
        {
            _connectionString = ConnectionString;
        }

        protected override IDbConnection OpenConnection(string contextName)
        {
            //  SqlConnection con = new SqlConnection(_connectionString);
            OracleConnection con = new OracleConnection(_connectionString);
            con.Open();
            return con;
        }

        public void Rollback()
        {
            SetAbort();
        }

        [DataContext]
        public long GetID()
        {
            CreaEtichetteAdapterBase a = new CreaEtichetteAdapterBase(DbConnection, DbTransaction);
            return a.GetID();
        }
    }
}
