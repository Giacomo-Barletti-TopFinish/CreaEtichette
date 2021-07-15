using CreaEtichette.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreaEtichette.Data.Core;

namespace CreaEtichette.Data
{
    public class CreaEtichetteBusinesSQL : CreaEtichetteBusinessBaseSQL
    {
        public CreaEtichetteBusinesSQL() : base() { }

        [DataContext]
        public void FillMAGAZZ(EtichetteDS ds, string IDMAGAZZ)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZSQL(ds, IDMAGAZZ);
        }

        [DataContext]
        public void TrovaArticolo(EtichetteDS ds, string Modello)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.TrovaArticoloSQL(ds, Modello);
        }


        [DataContext]
        public void FillETI_ARTICOLI(EtichetteDS ds, string IDMAGAZZ)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.FillETI_ARTICOLISQL(ds, IDMAGAZZ);
        }

        [DataContext(true)]
        public void UpdateETI_ARTICOLI(EtichetteDS ds)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.UpdateTable("ETICHETTELV", ds);
        }

        [DataContext]
        public void FillTEMP_COMMESSA(EtichetteDS ds, string IDMAGAZZ)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.FillTEMP_COMMESSA(ds, IDMAGAZZ);
        }
    }
}
