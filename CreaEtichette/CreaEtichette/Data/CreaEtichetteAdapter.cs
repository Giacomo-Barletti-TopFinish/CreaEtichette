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

        public void FillMAGAZZ(EtichetteDS ds, string IDMAGAZZ)
        {
            string select = @"SELECT * FROM GRUPPO.MAGAZZ WHERE IDMAGAZZ= $P<IDMAGAZZ>";
            ParamSet ps = new ParamSet();

            ps.AddParam("IDMAGAZZ", DbType.String, IDMAGAZZ);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.MAGAZZ);
            }
        }

        public void TrovaArticolo(EtichetteDS ds, string Modello)
        {
            string select = @"SELECT * FROM GRUPPO.MAGAZZ WHERE MODELLO like $P<MODELLO>";
            ParamSet ps = new ParamSet();
            string MODELLO = string.Format("%{0}%", Modello);
            ps.AddParam("MODELLO", DbType.String, MODELLO);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.MAGAZZ);
            }
        }
        public void FillUSR_IMPORT_MAGAZZ(EtichetteDS ds, string IDMAGAZZ)
        {
            string select = @"SELECT * FROM DITTA1.USR_IMPORT_MAGAZZ WHERE IDMAGAZZ= $P<IDMAGAZZ>";
            ParamSet ps = new ParamSet();

            ps.AddParam("IDMAGAZZ", DbType.String, IDMAGAZZ);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.MAGAZZ);
            }

            select = @"SELECT * FROM DITTA2.USR_IMPORT_MAGAZZ WHERE IDMAGAZZ= $P<IDMAGAZZ>";
            ps = new ParamSet();

            ps.AddParam("IDMAGAZZ", DbType.String, IDMAGAZZ);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.MAGAZZ);
            }
        }

        public void FillETI_ARTICOLI(EtichetteDS ds, string IDMAGAZZ)
        {
            string select = @"SELECT * FROM ETI_ARTICOLI WHERE IDMAGAZZ= $P<IDMAGAZZ>";
            ParamSet ps = new ParamSet();

            ps.AddParam("IDMAGAZZ", DbType.String, IDMAGAZZ);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.ETI_ARTICOLI);
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
