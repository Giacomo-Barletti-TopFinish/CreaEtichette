using CreaEtichette.Data;
using CreaEtichette.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreaEtichette
{
    public partial class TrovaCommessaFrm : Form
    {
        private string _commessa;
        public string Commessa { get { return _commessa; } }
        EtichetteDS ds = new EtichetteDS();
        private string _IDMAGAZZ;
        public string IDMAGAZZ { set { _IDMAGAZZ = value; } }

        public TrovaCommessaFrm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TrovaCommessaFrm_Load(object sender, EventArgs e)
        {
            using (CreaEtichetteBusiness bEtichetta = new CreaEtichetteBusiness())
            {
                bEtichetta.FillTEMP_COMMESSA(ds, _IDMAGAZZ);
                dgvCommessa.AutoGenerateColumns = true;
                dgvCommessa.DataSource = ds;
                dgvCommessa.DataMember = ds.TEMP_COMMESSA.TableName;

                dgvCommessa.Columns[0].Width = 400;
            }
        }

        private void dgvCommessa_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DataRow r = ds.TEMP_COMMESSA.Rows[e.RowIndex];
            string CLiente = (String)r[0];
            string oc = (String)r[1];
            string riga = (String)r[2];

            _commessa = string.Format("{0}-{1}", oc.Trim(), riga.Trim());
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
