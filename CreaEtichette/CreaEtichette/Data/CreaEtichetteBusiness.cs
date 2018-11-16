using CreaEtichette.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreaEtichette.Data.Core;

namespace CreaEtichette.Data
{
    public class CreaEtichetteBusiness : CreaEtichetteBusinessBase
    {
        public CreaEtichetteBusiness() : base() { }

        [DataContext]
        public void FillMAGAZZ(EtichetteDS ds, string IDMAGAZZ)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZ(ds, IDMAGAZZ);
        }

        [DataContext]
        public void TrovaArticolo(EtichetteDS ds, string Modello)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.TrovaArticolo(ds, Modello);
        }

        [DataContext]
        public void FillUSR_IMPORT_MAGAZZ(EtichetteDS ds, string IDMAGAZZ)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.FillUSR_IMPORT_MAGAZZ(ds, IDMAGAZZ);
        }

        [DataContext]
        public void FillETI_ARTICOLI(EtichetteDS ds, string IDMAGAZZ)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.FillETI_ARTICOLI(ds, IDMAGAZZ);
        }

        [DataContext(true)]
        public void UpdateETI_ARTICOLI(EtichetteDS ds)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.UpdateTable(ds.ETI_ARTICOLI.TableName, ds);
        }
    }
}
