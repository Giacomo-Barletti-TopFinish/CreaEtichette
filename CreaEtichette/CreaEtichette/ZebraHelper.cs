using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreaEtichette
{
    public class ZebraHelper
    {
        public static void EtichettaLuisVuitton(string zebraPrinter, string SKU, string descrizione, string quantita, string stampatada)
        {
            // string zebraPrinter = @"\\10.30.1.10\ZDesigner CONF-CHANEL";
            StringBuilder sb = new StringBuilder();
            sb.Append("^XA");
            sb.Append("^FO045,040^AQN,45,40");

            sb.Append(string.Format("^FD{0}^FS", SKU));

            sb.Append("^FO045,90^AQN,45,40");
            sb.Append(string.Format("^FD{0}^FS", descrizione));

            sb.Append("^FO045,170^AQN,45,40");
            sb.Append(string.Format("^FDQty:              {0} UN^FS", quantita));

            sb.Append("^FO045,230^AQN,35,34");
            sb.Append(string.Format("^FDPRINTED BY  -{0}-^FS", stampatada));

            sb.Append("^FO035,260");
            sb.Append("^GB400,2,4,,^FS");

            sb.Append("^FO065,280^BY2");
            sb.Append("^A0N,40,30^BCN,60,N,N,N");
            string barcode = string.Format(@"{0}/{1}", quantita.PadLeft(6,'0'), SKU);
            sb.Append(string.Format(@"^FD{0}^FS", barcode));
            sb.Append("^FO120,340^AQN,35,32");
            sb.Append(string.Format(@"^FD{0}^FS", barcode));
            sb.Append("^XZ");

            RawPrinterHelper.SendStringToPrinter(zebraPrinter, sb.ToString());
        }

        public static void EtichettaLuisVuitton_2(string zebraPrinter, string SKU, string descrizione, string quantita, string fornitore, string articolo, string codice)
        {
            // string zebraPrinter = @"\\10.30.1.10\ZDesigner CONF-CHANEL";
            StringBuilder sb = new StringBuilder();
            string fontGrande = "RN,55,55";
            string fontNormale = "SN,50,50";

            int posizioneX = 80;

            sb.Append("^XA");
            sb.Append(InserisciTesto(posizioneX, 20, fontGrande, fornitore));

            sb.Append(InserisciTesto(posizioneX, 80, fontNormale, articolo));

            sb.Append(InserisciTesto(posizioneX, 120, fontNormale, codice));

            sb.Append(InserisciTesto(posizioneX, 180, fontNormale, descrizione));
            sb.Append(InserisciTesto(posizioneX, 220, fontNormale, SKU));

            string str = string.Format("Quantita' n.     {0}", quantita);
            sb.Append(InserisciTesto(posizioneX, 300, fontNormale, str));

            sb.Append("^XZ");

            RawPrinterHelper.SendStringToPrinter(zebraPrinter, sb.ToString());
        }
        private static string InserisciTesto(int x, int y, string font, string testo)
        {
            return string.Format("^FO{0},{1}^A{2}^FD{3}^FS",x,y,font,testo);
        }
        public static void EtichettaLuisVuitton_3(string zebraPrinter, string SKU, string descrizione1, string descrizione2, string quantita, string fornitore, string articolo, string codice)
        {
            int posizioneX = 60;
            string fontGrande = "UN,60,55";
            string fontNormale = "RN,45,35";
            string fontPiccolo= "QN,30,30";
            // string zebraPrinter = @"\\10.30.1.10\ZDesigner CONF-CHANEL";
            StringBuilder sb = new StringBuilder();
            sb.Append("^XA");
            sb.Append(InserisciTesto(posizioneX, 20, fontGrande, fornitore));

            sb.Append(InserisciTesto(posizioneX, 80, fontPiccolo, articolo));

            sb.Append(InserisciTesto(posizioneX, 120, fontGrande, codice));

            sb.Append(InserisciTesto(posizioneX, 180, fontNormale, descrizione1));
            sb.Append(InserisciTesto(posizioneX, 220, fontNormale, descrizione2));
            sb.Append(InserisciTesto(posizioneX, 260, fontGrande, SKU));

            string str = string.Format("Quantita' n.         {0}", quantita);
            sb.Append(InserisciTesto(posizioneX, 330, fontNormale, str));

            sb.Append("^XZ");


            RawPrinterHelper.SendStringToPrinter(zebraPrinter, sb.ToString());
        }
    }
}
