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
        public void LeggiUtente(EtichetteDS ds, string User)
        {
            CreaEtichetteAdapter a = new CreaEtichetteAdapter(DbConnection, DbTransaction);
            a.LeggiUtente(ds, User);
        }

     
    }
}
