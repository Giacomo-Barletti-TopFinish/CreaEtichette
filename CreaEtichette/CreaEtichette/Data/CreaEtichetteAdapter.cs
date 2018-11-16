using CreaEtichette.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreaEtichette.Data
{
    public class CreaEtichetteAdapter : CreaEtichetteAdapterBase
    {
        public CreaEtichetteAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void LeggiUtente(EtichetteDS ds, string User)
        {
            string select = @"SELECT * FROM GRUPPO.USR_USER WHERE UIDUSER= $P<UTENTE>";
            ParamSet ps = new ParamSet();

            ps.AddParam("UTENTE", DbType.String, User);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_USER);
            }
        }


        public void UpdateTable(string tablename, EtichetteDS ds)
        {
            string query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0}", tablename);

            using (DbDataAdapter a = BuildDataAdapter(query))
            {
                try
                {
                    a.ContinueUpdateOnError = false;
                    DataTable dt = ds.Tables[tablename];
                    DbCommandBuilder cmd = BuildCommandBuilder(a);
                    a.UpdateCommand = cmd.GetUpdateCommand();
                    a.DeleteCommand = cmd.GetDeleteCommand();
                    a.InsertCommand = cmd.GetInsertCommand();
                    a.Update(dt);
                }
                catch (DBConcurrencyException ex)
                {

                }
                catch
                {
                    throw;
                }
            }
        }

      
    }
}
