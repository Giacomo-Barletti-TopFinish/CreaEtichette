using BarcodeLib;
using CreaEtichette.Data;
using CreaEtichette.Entities;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using RawPrint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreaEtichette
{
    public partial class Form1 : Form
    {
        private EtichetteDS _ds = new EtichetteDS();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStampaEtichetta1_Click(object sender, EventArgs e)
        {
            lblMessaggio.Text = string.Empty;
            if (ddlStampanti.SelectedIndex == -1)
            {
                lblMessaggio.Text = "Selezionare la stampante";
                return;
            }

            if (string.IsNullOrEmpty(txtE1_SKU.Text) ||
                string.IsNullOrEmpty(txtE1_Parziale.Text) ||
                string.IsNullOrEmpty(txtE1_Quantita.Text) ||
                string.IsNullOrEmpty(txtE1_StampatoDa.Text))
            {
                lblMessaggio.Text = "Ci sono campi vuoti nell'etichetta. Impossibile stampare";
                return;
            }

            if (nE1.Value <= 0)
            {
                lblMessaggio.Text = "Indicare il numero di etichette";
                return;
            }
            //            string PrinterName = @"\\10.30.1.10\ZDesigner CONF-CHANEL";
            string PrinterName = ddlStampanti.SelectedItem.ToString();

            for (int i = 0; i < nE1.Value; i++)
            {
                ZebraHelper.EtichettaLuisVuitton(PrinterName, txtE1_SKU.Text, txtE1_Parziale.Text, txtE1_Quantita.Text, txtE1_StampatoDa.Text);
            }
        }

        private MemoryStream EtichettaLuisVitton(string SKU, string descrizione, string quantita, string stampatada)
        {
            Document doc = new Document();
            doc.DefaultPageSetup.PageWidth = Unit.FromCentimeter(10);
            doc.DefaultPageSetup.PageHeight = Unit.FromCentimeter(5);
            doc.DefaultPageSetup.BottomMargin = Unit.FromCentimeter(0.5);
            doc.DefaultPageSetup.TopMargin = Unit.FromCentimeter(0.5);
            doc.DefaultPageSetup.LeftMargin = Unit.FromCentimeter(0.1);
            doc.DefaultPageSetup.RightMargin = Unit.FromCentimeter(0.1);

            Section sec = doc.AddSection();
            Paragraph p = sec.AddParagraph(SKU);
            p.Format.Font.Bold = true;
            p = sec.AddParagraph(descrizione);
            p.Format.SpaceBefore = Unit.FromCentimeter(0.1);
            string qta = string.Format("Qty:                  {0} UN", quantita.ToString());
            p = sec.AddParagraph(qta);

            p.Format.Font.Bold = true;
            p.Format.SpaceBefore = Unit.FromCentimeter(0.4);
            p = sec.AddParagraph(string.Format("PRINTED BY - {0} -", stampatada));
            p.Format.SpaceBefore = Unit.FromCentimeter(0.1);

            Border newBorder = new Border { Style = MigraDoc.DocumentObjectModel.BorderStyle.Single };
            p = sec.AddParagraph();
            p.Format.Borders.Top = newBorder;
            p.Format.SpaceBefore = 0;
            p.Format.SpaceAfter = 0;

            string barcodeStr = string.Format("{0}/{1}", quantita.ToString().PadLeft(6, '0'), SKU);
            System.Drawing.Image img = CreaBarcode(barcodeStr, 300, 50);
            // img.Save(@"c:\temp\barcode.png");

            ImageConverter _imageConverter = new ImageConverter();
            byte[] image = (byte[])_imageConverter.ConvertTo(img, typeof(byte[]));


            string imageStr = MigraDocFilenameFromByteArray(image);
            sec.AddImage(imageStr);

            PdfDocumentRenderer pdf = new PdfDocumentRenderer();
            pdf.Document = doc;
            pdf.RenderDocument();

            MemoryStream ms = new MemoryStream();
            pdf.Save(ms, false);
            return ms;
        }

        private static string MigraDocFilenameFromByteArray(byte[] image)
        {
            return "base64:" + Convert.ToBase64String(image);
        }

        private static System.Drawing.Image CreaBarcode(string barcodeStr, int width, int height)
        {
            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode()
            {
                IncludeLabel = true,
                Alignment = AlignmentPositions.CENTER,
                Width = width,
                Height = height,
                RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
            };

            return barcode.Encode(TYPE.CODE128B, barcodeStr);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                ddlStampanti.Items.Add(printer);
            }
            if (ddlStampanti.Items.Count > 0)
                ddlStampanti.SelectedIndex = 0;

            txtE1_StampatoDa.Text = Properties.Settings.Default.MittenteCodice;
            txtE2_Fornitore.Text = Properties.Settings.Default.Mittente;
            txtE3_Fornitore.Text = Properties.Settings.Default.Mittente;
        }

        private void btnStampaEtichetta2_Click(object sender, EventArgs e)
        {
            lblMessaggio.Text = string.Empty;
            if (ddlStampanti.SelectedIndex == -1)
            {
                lblMessaggio.Text = "Selezionare la stampante";
                return;
            }
            string PrinterName = ddlStampanti.SelectedItem.ToString();

            if (string.IsNullOrEmpty(txtE2_Fornitore.Text) ||
                string.IsNullOrEmpty(txtE2_CodiceModello.Text) ||
                string.IsNullOrEmpty(txtE2_CodiceColore.Text) ||
                string.IsNullOrEmpty(txtE2_DescrizioneColore.Text) ||
                string.IsNullOrEmpty(txtE2_SKU.Text) ||
                string.IsNullOrEmpty(txtE2_Quantita.Text))
            {
                lblMessaggio.Text = "Ci sono campi vuoti nell'etichetta. Impossibile stampare";
                return;
            }

            if (nE2.Value <= 0)
            {
                lblMessaggio.Text = "Indicare il numero di etichette";
                return;
            }

            for (int i = 0; i < nE2.Value; i++)
            {
                ZebraHelper.EtichettaLuisVuitton_2(PrinterName, txtE2_SKU.Text, txtE2_DescrizioneColore.Text, txtE2_Quantita.Text, txtE2_Fornitore.Text, txtE2_CodiceModello.Text, txtE2_CodiceColore.Text);
            }
        }

        private void btnStampaEtichetta3_Click(object sender, EventArgs e)
        {
            lblMessaggio.Text = string.Empty;
            if (ddlStampanti.SelectedIndex == -1)
            {
                lblMessaggio.Text = "Selezionare la stampante";
                return;
            }
            string PrinterName = ddlStampanti.SelectedItem.ToString();

            if (string.IsNullOrEmpty(txtE3_Fornitore.Text) ||
                string.IsNullOrEmpty(txtE3_RagioneSocialeCliente.Text) ||
                string.IsNullOrEmpty(txtE3_CodiceModello.Text) ||
                string.IsNullOrEmpty(txtE3_DescrizioneModelloRiga1.Text) ||
                string.IsNullOrEmpty(txtE3_DescrizioneModelloRiga2.Text) ||
                string.IsNullOrEmpty(txtE3_RigaCommessa.Text) ||
                string.IsNullOrEmpty(txtE3_Quantita.Text))
            {
                lblMessaggio.Text = "Ci sono campi vuoti nell'etichetta. Impossibile stampare";
                return;
            }

            if (nE3.Value <= 0)
            {
                lblMessaggio.Text = "Indicare il numero di etichette";
                return;
            }

            for (int i = 0; i < nE3.Value; i++)
            {
                ZebraHelper.EtichettaLuisVuitton_3(PrinterName, txtE3_RigaCommessa.Text, txtE3_DescrizioneModelloRiga1.Text, txtE3_DescrizioneModelloRiga2.Text, txtE3_Quantita.Text, txtE3_Fornitore.Text, txtArticolo_3.Text, txtE3_CodiceModello.Text);
            }
        }

        private void btnTrova_Click(object sender, EventArgs e)
        {
            using (CreaEtichetteBusiness bEtichetta = new CreaEtichetteBusiness())
            {
                if (string.IsNullOrEmpty(txtModelloRicerca.Text)) return;

                string modelloDaTrovare = txtModelloRicerca.Text.Trim().ToUpper();

                _ds = new EtichetteDS();
                bEtichetta.TrovaArticolo(_ds, modelloDaTrovare);

                dgvArticoli.AutoGenerateColumns = true;
                dgvArticoli.DataSource = _ds;
                dgvArticoli.DataMember = _ds.MAGAZZ.TableName;
                for (int i = 0; i < dgvArticoli.Columns.Count; i++)
                    dgvArticoli.Columns[i].Visible = false;

                dgvArticoli.Columns[1].Visible = true;
                dgvArticoli.Columns[1].Width = 180;
                dgvArticoli.Columns[2].Visible = true;
                dgvArticoli.Columns[2].Width = 400;

                System.Drawing.Font font = new System.Drawing.Font(dgvArticoli.Font.FontFamily, 8);

                dgvArticoli.Font = font;

            }
        }

        private void dgvArticoli_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DataRow r = _ds.MAGAZZ.Rows[e.RowIndex];
            string IDMAGAZZ = (String)r[0];
            string modello = (String)r[1];
            string descrizione = (String)r[2];

            using (CreaEtichetteBusiness bEtichetta = new CreaEtichetteBusiness())
            {
                _ds.USR_IMPORT_MAGAZZ.Clear();
                _ds.ETI_ARTICOLI.Clear();
                bEtichetta.FillETI_ARTICOLI(_ds, IDMAGAZZ);
                bEtichetta.FillUSR_IMPORT_MAGAZZ(_ds, IDMAGAZZ);
            }

            txtIDMAGAZZ.Text = IDMAGAZZ;
            txtModelloMetalplus.Text = modello;
            txtModelloMetalplusDescrizione.Text = descrizione;

            EtichetteDS.ETI_ARTICOLIRow eti = _ds.ETI_ARTICOLI.Where(x => x.IDMAGAZZ == IDMAGAZZ).FirstOrDefault();
            EtichetteDS.USR_IMPORT_MAGAZZRow import = _ds.USR_IMPORT_MAGAZZ.Where(x => x.IDMAGAZZ == IDMAGAZZ).FirstOrDefault();
            if (eti != null)
            {
                txtSKU.Text = eti.SKU;
                txtCodiceModelloCliente.Text = eti.CODICEMODELLO;
                txtModelloCliente.Text = eti.MODELLO;
                txtCodiceColoreCliente.Text = eti.CODICECOLORE;
                txtColoreCliente.Text = eti.COLORE;
                txtParziale.Text = eti.IsPARZIALENull() ? string.Empty : eti.PARZIALE;
            }
            else if (import != null)
            {
                txtSKU.Text = string.Empty;

                string modelloImp = import.IsMODELLOIMPNull() ? string.Empty : import.MODELLOIMP;
                if (!string.IsNullOrEmpty(modelloImp))
                {
                    if (modelloImp[0] == 'R' && modelloImp.Contains('-'))
                    {
                        string sku = modelloImp.Split('-')[0];
                        if (sku.Length < 12)
                        {
                            txtSKU.Text = sku;
                            modelloImp = modelloImp.Split('-')[1];
                        }
                    }
                }

                txtCodiceModelloCliente.Text = modelloImp;
                txtModelloCliente.Text = import.IsDESMAGAZZIMPNull() ? string.Empty : import.DESMAGAZZIMP;
                txtCodiceColoreCliente.Text = string.Empty;
                txtColoreCliente.Text = string.Empty;
                txtParziale.Text = string.Empty;
            }
            else
            {
                txtSKU.Text = string.Empty;
                txtCodiceModelloCliente.Text = string.Empty;
                txtModelloCliente.Text = string.Empty;
                txtCodiceColoreCliente.Text = string.Empty;
                txtColoreCliente.Text = string.Empty;
                txtParziale.Text = string.Empty;
            }
        }

        private void btnSalva_Click(object sender, EventArgs e)
        {
            lblMessaggio.Text = string.Empty;

            string SKU = txtSKU.Text;
            string codiceModello = txtCodiceModelloCliente.Text;
            string modello = txtModelloCliente.Text;
            string codicecolore = txtCodiceColoreCliente.Text;
            string colore = txtColoreCliente.Text;
            string parziale = txtParziale.Text;

            if (string.IsNullOrEmpty(SKU) ||
                string.IsNullOrEmpty(codiceModello) ||
                string.IsNullOrEmpty(modello) ||
                string.IsNullOrEmpty(codicecolore) ||
                string.IsNullOrEmpty(colore) ||
                string.IsNullOrEmpty(parziale)
                )
            {
                lblMessaggio.Text = "I campi CLIENTE non possono essere vuoti";
                return;
            }

            if (string.IsNullOrEmpty(txtIDMAGAZZ.Text))
            {
                lblMessaggio.Text = "IDMAGAZZ non può essere vuoto. Selezionare un articolo.";
                return;
            }

            string IDMAGAZZ = txtIDMAGAZZ.Text;

            EtichetteDS.ETI_ARTICOLIRow eti = _ds.ETI_ARTICOLI.Where(x => x.IDMAGAZZ == IDMAGAZZ).FirstOrDefault();
            if (eti == null)
            {
                eti = _ds.ETI_ARTICOLI.NewETI_ARTICOLIRow();
                eti.IDMAGAZZ = IDMAGAZZ;
                eti.SKU = SKU;
                eti.CODICEMODELLO = codiceModello;
                eti.CODICECOLORE = codicecolore;
                eti.MODELLO = modello;
                eti.COLORE = colore;
                eti.PARZIALE = parziale;
                _ds.ETI_ARTICOLI.AddETI_ARTICOLIRow(eti);
            }
            else
            {
                eti.SKU = SKU;
                eti.CODICEMODELLO = codiceModello;
                eti.CODICECOLORE = codicecolore;
                eti.MODELLO = modello;
                eti.COLORE = colore;
                eti.PARZIALE = parziale;
            }

            using (CreaEtichetteBusiness bEtichetta = new CreaEtichetteBusiness())
            {
                bEtichetta.UpdateETI_ARTICOLI(_ds);
                lblMessaggio.Text = "Articolo salvato";
                _ds.AcceptChanges();
            }
        }

        private void btnPreparaEtichette_Click(object sender, EventArgs e)
        {
            txtE1_SKU.Text = txtSKU.Text;
            txtE1_Parziale.Text = txtParziale.Text;

            txtE2_CodiceModello.Text = txtCodiceModelloCliente.Text;
            txtE2_CodiceColore.Text = txtCodiceColoreCliente.Text;
            txtE2_DescrizioneColore.Text = txtColoreCliente.Text;
            txtE2_SKU.Text = txtSKU.Text;

            string riga1 = txtModelloCliente.Text;
            string riga2 = string.Empty;
            if (txtModelloCliente.Text.Length > 40)
            {
                riga1 = txtModelloCliente.Text.Substring(0, 40);
                riga2 = txtModelloCliente.Text.Substring(40);
            }


            txtE3_CodiceModello.Text = txtCodiceModelloCliente.Text;
            txtE3_DescrizioneModelloRiga1.Text = riga1;
            txtE3_DescrizioneModelloRiga1.Text = riga2;

        }
    }
}
