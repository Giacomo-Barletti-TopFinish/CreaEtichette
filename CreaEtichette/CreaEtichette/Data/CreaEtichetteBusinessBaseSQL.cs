using Oracle.ManagedDataAccess.Client;
using CreaEtichette.Data.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CreaEtichette.Data
{
    public class CreaEtichetteBusinessBaseSQL : BusinessBase
    {
        protected static string ConnectionName
        {
            get
            {
                return "MPI";
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
        public CreaEtichetteBusinessBaseSQL()
        {
            _connectionString = ConnectionString;
        }

        protected override IDbConnection OpenConnection(string contextName)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            //OracleConnection con = new OracleConnection(_connectionString);
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
