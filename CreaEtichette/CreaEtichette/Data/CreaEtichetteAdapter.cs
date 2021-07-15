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

        public void FillMAGAZZSQL(EtichetteDS ds, string IDMAGAZZ)
        {
            string select = @"SELECT * FROM CROSSREFERENCE 
                            where [Cross-Reference Type]=1
                            and [Cross-Reference Type No_]in('C00197','C00422','C00443','C00463','C00481')
                            AND [Cross-Reference No_] like '$P<IDMAGAZZ>'
                               ";
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

        public void TrovaArticoloSQL(EtichetteDS ds, string Modello)
        {
            string select = @"SELECT [Item No_] IDMAGAZZ,[Cross-Reference No_] MODELLO,[DESCRIPTION]DESMAGAZZ  FROM CROSSREFERENCE 
                            where [Cross-Reference Type]=1
                            and [Cross-Reference Type No_]in('C00197','C00422','C00443','C00463','C00481') ";
//                            AND [Cross-Reference No_] like '$P<MODELLO>";


            ParamSet ps = new ParamSet();
            AddConditionAndParam(ref select, "[Cross-Reference No_]", "MODELLO", Modello, ps, true);

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
                da.Fill(ds.USR_IMPORT_MAGAZZ);
            }

            select = @"SELECT * FROM DITTA2.USR_IMPORT_MAGAZZ WHERE IDMAGAZZ= $P<IDMAGAZZ>";
            ps = new ParamSet();

            ps.AddParam("IDMAGAZZ", DbType.String, IDMAGAZZ);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_IMPORT_MAGAZZ);
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

        public void FillETI_ARTICOLISQL(EtichetteDS ds, string IDMAGAZZ)
        {
            string select = @"SELECT * FROM [ETICHETTELV] WHERE IDMAGAZZ= $P<IDMAGAZZ>";
            ParamSet ps = new ParamSet();

            ps.AddParam("IDMAGAZZ", DbType.String, IDMAGAZZ);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.ETI_ARTICOLI);
            }
        }

        public void FillTEMP_COMMESSA(EtichetteDS ds, string IDMAGAZZ)
        {
            string select = @"SELECT trim(CLI.RAGIONESOC) as RAGIONESOC,trim(DT.RIFERIMENTO) as RIFERIMENTO,trim(DD.NRRIGA) as NRRIGA
                                from DITTA1.USR_VENDITED DD 
                                INNER JOIN DITTA1.USR_VENDITET DT ON DT.IDVENDITET = DD.IDVENDITET
                                INNER JOIN GRUPPO.CLIFO CLI ON DT.CODICECLIFO = CLI.CODICE
                                WHERE DD.IDMAGAZZ= $P<IDMAGAZZ>
                                AND DT.IDTABTIPDOC='0000000022'
                                order by DT.RIFERIMENTO, dd.nrriga ";
            ParamSet ps = new ParamSet();

            ps.AddParam("IDMAGAZZ", DbType.String, IDMAGAZZ);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.TEMP_COMMESSA);
            }
        }
        public void UpdateTable(string tablename, EtichetteDS ds)
        {
            string query = "SELECT * FROM ETICHETTELV";

            using (DbDataAdapter a = BuildDataAdapter(query))
            {
                try
                {
                    a.ContinueUpdateOnError = false;
                    DataTable dt = ds.Tables["ETI_ARTICOLI"];
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
