using BarcodeLib;
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
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStampa_Click(object sender, EventArgs e)
        {
            if (ddlStampanti.SelectedIndex == -1)
            {
                MessageBox.Show("Selezionare la stampante", "ERRRORE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string PrinterName = @"\\10.30.1.10\ZDesigner CONF-CHANEL";
            PrinterName = ddlStampanti.SelectedItem.ToString();
            //            string PrinterName = string.Empty;
            //          PrintDialog pd = new PrintDialog();
            //         if (pd.ShowDialog() == DialogResult.OK)
            if (nEtichette.Value <= 0) return;

            for (int i = 0; i < nEtichette.Value; i++)
            {
                ZebraHelper.EtichettaLuisVuitton(PrinterName, txtSKU.Text, txtDescrizione.Text, txtQuantita.Text, txtStampaDa.Text);
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
        }

        private void btnEtichetta2_Click(object sender, EventArgs e)
        {
            if (ddlStampanti.SelectedIndex == -1)
            {
                MessageBox.Show("Selezionare la stampante", "ERRRORE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string PrinterName = ddlStampanti.SelectedItem.ToString();

            if (nEtichette_2.Value <= 0) return;

            for (int i = 0; i < nEtichette_2.Value; i++)
            {
                ZebraHelper.EtichettaLuisVuitton_2(PrinterName, txtSku_2.Text, txtDescrizione_2.Text, txtQuantita_2.Text,txtFornitore.Text,txtArticolo.Text,txtCodice.Text);
            }
        }

        private void btnStampa_3_Click(object sender, EventArgs e)
        {
            if (ddlStampanti.SelectedIndex == -1)
            {
                MessageBox.Show("Selezionare la stampante", "ERRRORE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string PrinterName = ddlStampanti.SelectedItem.ToString();

            if (nEtichette_3.Value <= 0) return;

            for (int i = 0; i < nEtichette_3.Value; i++)
            {
                ZebraHelper.EtichettaLuisVuitton_3(PrinterName, txtSku_3.Text, txtDescrizione3.Text,txtDescrizione3_2.Text, txtQuantita_3.Text, txtFornitore_3.Text, txtArticolo_3.Text, txtCodice_3.Text);
            }
        }
    }
}
