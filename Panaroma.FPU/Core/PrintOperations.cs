using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaroma.FPU.Core
{
    internal class PrintOperations
    {
        private bool _PrintMode = true;//Printer kontrolünün devreye alınıp alınmaması ile ilgili eklendi.

        internal PrintOperations(bool printMode = true)
        {
            this._PrintMode = printMode;
            Nettuna7000IAPI = new Nettuna7000IClass();
            OpenPrinter();
        }

        private bool BitCheck(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }


        public Nettuna7000IClass Nettuna7000IAPI;//Form Load--> Nettuna7000IAPI=new Nettuna7000IAPI();

        public void writeToDisplay(string line1, string line2)
        {
            Nettuna7000IAPI.clearDisplay(Nettuna7000I_Global.hPrn);
            res = Nettuna7000IAPI.writeDisplay(Nettuna7000I_Global.hPrn, line1, line2);

        }

        /// <summary>
        /// Çekmece açmak için kullanılır
        /// </summary>
        public void OpenDrawer()
        {
            //short res;
            res = Nettuna7000IAPI.openDrawer(Nettuna7000I_Global.hPrn, 0);
        }
        short res;

        private void OpenPrinter()
        {
            Nettuna7000I_Global.hPrn = 0;
            string Port = "USB001";
            short PrinterStatus = 0;
            System.Diagnostics.Process Proc = new System.Diagnostics.Process();

            //Proc.StartInfo.FileName = "ForcePnP_Nettuna7000I.exe";
            //Proc.Start();

            if (Nettuna7000I_Global.Prt100IsOpen == false)
            {
                try
                {
                    res = Nettuna7000IAPI.openDevice(Port, 0, 0, 0, 0, 0, ref Nettuna7000I_Global.hPrn);

                }
                catch (DllNotFoundException de)
                {
                    string ExDesc = de.ToString();

                    //MessageBox.Show("DLL Nettuna7000I.dll or PortComm not found !", "Application Error");
                    return;
                }
                if (res != (short)Nettuna7000I_ErrorCode.Nettuna7000I_SUCCESS)
                {
                    return;
                }
                short Function = (short)Nettuna7000I_StatusRequest.STATUS_ROLLPAPER;

                res = Nettuna7000IAPI.getDeviceStatus(Nettuna7000I_Global.hPrn, 1, ref PrinterStatus);
                if (res != (short)Nettuna7000I_ErrorCode.Nettuna7000I_SUCCESS)
                {
                    //Nettuna7000IAPI.closeDevice(Nettuna7000I_Global.hPrn);
                    return;
                }





                bool a = BitCheck((byte)PrinterStatus, 3);

                if (a == true)
                {
                    res = Nettuna7000IAPI.getDeviceStatus(Nettuna7000I_Global.hPrn, 2, ref PrinterStatus);

                    //Nettuna7000IAPI.closeDevice(Nettuna7000I_Global.hPrn);
                    return;
                }


                Nettuna7000I_Global.Prt100IsOpen = true;

            }
        }

        public void Print(string value)
        {
            Print(value, true);
        }


        public void PrintBold(string value, Nettuna7000IClass Nettuna7000IAPI, string cont)
        {
            short nWrite = (short)0;
            byte[] bytes;
            short FontAttribute;
            //if (value.Contains(cont))
            //{
            bytes = Encoding.GetEncoding("CP857").GetBytes(value.Replace(cont, ""));
            FontAttribute = (short)1;
            int num1 = (int)Nettuna7000IAPI.setFontSize(Nettuna7000I_Global.hPrn, (short)1);
            int num2 = (int)Nettuna7000IAPI.setFontType(Nettuna7000I_Global.hPrn, (short)1);
            //}
            //else
            //{
            //    bytes = Encoding.GetEncoding("CP857").GetBytes(value);
            //    int num1 = (int)Nettuna7000IAPI.setFontSize(Nettuna7000I_Global.hPrn, (short)0);
            //    int num2 = (int)Nettuna7000IAPI.setFontType(Nettuna7000I_Global.hPrn, (short)1);
            //    FontAttribute = (short)0;
            //}
            int num3 = (int)Nettuna7000IAPI.setFontAttribute(Nettuna7000I_Global.hPrn, FontAttribute);
            int num4 = (int)Nettuna7000IAPI.printText(Nettuna7000I_Global.hPrn, bytes, (short)bytes.Length, ref nWrite);
        }

        public void feedAndCut(short feedLine, Nettuna7000IClass Nettuna7000IAPI)
        {
            int num1 = (int)Nettuna7000IAPI.feedPaperByLine(Nettuna7000I_Global.hPrn, feedLine);
            int num2 = (int)Nettuna7000IAPI.totalCut(Nettuna7000I_Global.hPrn);
        }


        public void PrintQrCode(string qrCode)
        {
            printQRCode(qrCode, Nettuna7000IAPI);
        }


        private void printQRCode(string QRCode, Nettuna7000IClass Nettuna7000IAPI)
        {
            if (string.IsNullOrEmpty(QRCode))
            {
                return;
            }

            Encoding enc = Encoding.GetEncoding(857); // 437 is the original IBM PC code page
            byte[] l1Turk = enc.GetBytes(QRCode);
            Nettuna7000IAPI.StringToQRCode(l1Turk, 5);
            Nettuna7000IAPI.setPrintAligment(Nettuna7000I_Global.hPrn, (short)Nettuna7000I_Aligment.ALIGN_CENTER);
            int res = Nettuna7000IAPI.printImage(Nettuna7000I_Global.hPrn, "QRCode.bmp", 0x30);
            switch (res)
            {
                case (Int16)Nettuna7000I_ErrorCode.Nettuna7000I_SUCCESS:
                    //SaveLog("Comand succes");
                    break;
                case (Int16)Nettuna7000I_ErrorCode.Nettuna7000I_E_INUSE:
                    //SaveLog("Error - Printer in use");
                    break;
                case (Int16)Nettuna7000I_ErrorCode.Nettuna7000I_E_TIMEOUT:
                    //SaveLog("Error - timeout");
                    break;
                case (Int16)Nettuna7000I_ErrorCode.Nettuna7000I_E_FAILURE:
                    //SaveLog("Error - unforeseeable");
                    break;
                case (Int16)Nettuna7000I_ErrorCode.Nettuna7000I_E_ILLEGAL:
                    //SaveLog("Error - Invalid parameter");
                    break;
                default:
                    //SaveLog("Unknow condition");
                    break;
            }
            res = Nettuna7000IAPI.feedPaperByLine(Nettuna7000I_Global.hPrn, 1);
        }

        public void Print(string value, bool cut)
        {
            var boldChar = Encoding.GetEncoding(857).GetString(new byte[] { 178 });

            //if (value.Contains(boldChar))
            //{
            //    PrintBold(value, Nettuna7000IAPI, boldChar);
            //}
            //else
            {
                //Int16 res;
                Int16 nWrite = 0;
                Int16 FontAttribute = 0;

                byte[] stringToBytes;
                string replacedString = "";


                stringToBytes = System.Text.Encoding.GetEncoding("CP857").GetBytes(value);
                Nettuna7000IAPI.setFontSize(Nettuna7000I_Global.hPrn, (short)0x00);
                Nettuna7000IAPI.setFontType(Nettuna7000I_Global.hPrn, (short)Nettuna7000I_FontType.FONT_A);
                FontAttribute = (short)Nettuna7000I_FontAttribute.FONT_A_DEFAULT;

                res = Nettuna7000IAPI.setFontAttribute(Nettuna7000I_Global.hPrn, FontAttribute);
                res = Nettuna7000IAPI.printText(Nettuna7000I_Global.hPrn, stringToBytes, (short)stringToBytes.Length, ref nWrite);
            }

            if (cut)
            {
                res = Nettuna7000IAPI.feedPaperByLine(Nettuna7000I_Global.hPrn, 1);
                Nettuna7000IAPI.totalCut(Nettuna7000I_Global.hPrn);
            }
        }


        /// <summary>
        /// Kağıt Kapak Kontrolü kağıt yoksa hiç bir işlem yapmaz
        /// </summary>
        /// <param name="yazici"></param>
        /// <returns></returns>
        //public bool yaziciKagitKontrol(bool yazici = true)
        //{
        //    bool ret = false;
        //    //BelgeBasimIslemleri bbi = new BelgeBasimIslemleri();
        //    string str = "";
        //    if (yazici)
        //        str = this.paperAndPrinterControl();
        //    else
        //        str = this.paperControl();

        //    if (str != "")
        //    {
        //        if (str.Contains("bitti") || str.Contains("Kapak Açık"))
        //        {
        //            //lblKagitDurumu.Text = "Kağıt Bitti / Mandal / Kağıt Takılı Değil";
        //            //return new ExecutedCommandResult { ErrorCode = 101, Message = "FPU Busy" };
        //            new ExecutedCommandResult { ErrorCode = 555, Message = "KAĞIT RULOSU TAKILI DEĞİL VEYA BİTMİŞ OLABİLİR! --- PRİNTER KAPAK VE MANDALINI KONTROL EDİNİZ" };
        //            //MetroFramework.MetroMessageBox.Show(this, "Kağıdınız bitmiş ya da yazıcıya düzgün takılmamış. kontrol ediniz!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            ret = false;
        //            return ret;
        //        }
        //        else if (str.Contains("Kağıt çok az Kağıt var "))
        //        {
        //             new ExecutedCommandResult { ErrorCode = 556, Message = "KAĞIT BİTMEYE YAKIN" };
        //            ret = true;
        //            return ret;
        //        }
        //        //else if (str.Contains("Kağıt var"))
        //        //{
        //        //    lblKagitDurumu.Text = "Kağıt var";
        //        //    ret = true;
        //        //}
        //        //else if (str.Contains("Kağıt Az Kağıt Uygun"))
        //        //{
        //        //    lblKagitDurumu.Text = "Kağıt Az Kağıt Uygun";
        //        //    lblKagitDurumu.BackColor = Color.Pink;
        //        //    ret = true;
        //        //}
        //        //else if (str.Contains("Kapak Açık"))
        //        //{
        //        //    new ExecutedCommandResult { ErrorCode = 557, Message = "PRİNTER KAPAK VE MANDALINI KONTROL EDİNİZ" };
        //        //    ret = false;
        //        //    return ret;

        //        //}
        //        else
        //            ret = true;
        //    }
        //    else
        //    {
        //        ret = true;
        //    }
        //    return ret;
        //}

        public bool yaziciKagitKontrol()
        {
            bool ret = false;
            string str = "";
            if (!this._PrintMode)
            {
                ret = true;
            }
            else
            {
                str = this.paperAndPrinterControl();

                if (str != "")
                {
                    //if (str.Contains("bitti"))
                    //{
                    //    ret = false;
                    //}

                    //if (str.Contains("var"))
                    //{
                    //    ret = true;
                    //}
                    //else if (str.Contains("az") && str.Contains("Kapalı"))
                    //{
                    //    ret = true;
                    //}
                    //else if ( str.Contains("Açık"))
                    //{
                    //    ret = false;
                    //}
                    //else if (str.Contains("var") && str.Contains("Açık"))
                    //{
                    //    ret = false;
                    //}
                    //else
                    //    ret = true;

                    if (str.Contains("5") || str.Contains("6"))
                    {
                        ret = false;
                    }
                    else
                    {
                      

                        ret = true;
                    }
                }
                else
                {
                    ret = false;//false 
                }
            }
            return ret;
        }

        /// <summary>
        /// Kağıt durum kontrol senaryoları eklendi
        /// </summary>
        /// <param name="Nettuna7000IAPI"></param>
        /// <returns></returns>
        public string paperAndPrinterControl()
        {
            ////Nettuna7000IAPI = new Nettuna7000IClass();
            string str = "";
            short PrinterStatus = 0;
            //short res;
            //string Port = "USB001";
            ////Nettuna7000I_Global.hPrn = 0;
            short Function = (short)Nettuna7000I_StatusRequest.STATUS_ROLLPAPER;
            //if (Nettuna7000I_Global.Prt100IsOpen == false)
            //{
            //    try
            //    {
            //        res = Nettuna7000IAPI.openDevice(Port, 0, 0, 0, 0, 0, ref Nettuna7000I_Global.hPrn);

            //    }
            //    catch (DllNotFoundException de)
            //    {
            //        string ExDesc = de.ToString();

            //        return ExDesc;
            //    }

            //    if (res != (short)Nettuna7000I_ErrorCode.Nettuna7000I_SUCCESS)
            //    {
            //        return "";
            //    }
            //}

            res = Nettuna7000IAPI.getDeviceStatus(Nettuna7000I_Global.hPrn, Function, ref PrinterStatus);
            if (((BitCheck((byte)PrinterStatus, 2)) == true) &&
                                (BitCheck((byte)PrinterStatus, 3)) == true)
                str += "1";//kağıt cok az
            if (((BitCheck((byte)PrinterStatus, 5)) == true) &&
                 (BitCheck((byte)PrinterStatus, 6)) == true)
                str += "6";//Kağıt bitti
            if (((BitCheck((byte)PrinterStatus, 5)) == false) &&
                (BitCheck((byte)PrinterStatus, 6)) == false)
                str += "2"; //Paper Present//kağıt var

            if (((BitCheck((byte)PrinterStatus, 2)) == false) &&
                (BitCheck((byte)PrinterStatus, 3)) == false)
                str += "3";//Paper Adeguate;   //kagıt az uygun
            Nettuna7000IAPI.cardReaderEnable(Nettuna7000I_Global.hPrn, true, 0);
            res = Nettuna7000IAPI.getDeviceStatus(Nettuna7000I_Global.hPrn, 0x01, ref PrinterStatus);
            if (((BitCheck((byte)PrinterStatus, 3) == true)))
                str += "5";//kapak açık
            else
                str += "4";//kapak kapalı

            return str;

        }

        public string paperControl()
        {
            string str = "";
            short PrinterStatus = 0;
            //status oku
            //short res;
            short Function = (short)Nettuna7000I_StatusRequest.STATUS_ROLLPAPER; res = Nettuna7000IAPI.getDeviceStatus(Nettuna7000I_Global.hPrn, Function, ref PrinterStatus);
            if (((BitCheck((byte)PrinterStatus, 2)) == true) &&
                                (BitCheck((byte)PrinterStatus, 3)) == true)
                str += "Kağıt çok az ";
            if (((BitCheck((byte)PrinterStatus, 5)) == true) &&
                 (BitCheck((byte)PrinterStatus, 6)) == true)
                str += "Kağıt bitti ";
            if (((BitCheck((byte)PrinterStatus, 5)) == false) &&
                (BitCheck((byte)PrinterStatus, 6)) == false)
                str += "Kağıt var "; //Paper Present

            if (((BitCheck((byte)PrinterStatus, 2)) == false) &&
                (BitCheck((byte)PrinterStatus, 3)) == false)
                str += "Kağıt Az Kağıt Uygun ";//Paper Adeguate;
            //Function = (short)Nettuna7000I_StatusRequest.STATUS_OFFLINE;
            Nettuna7000IAPI.cardReaderEnable(Nettuna7000I_Global.hPrn, true, 0);
            res = Nettuna7000IAPI.getDeviceStatus(Nettuna7000I_Global.hPrn, 0x01, ref PrinterStatus);
            if (((BitCheck((byte)PrinterStatus, 3) == true)))
                str += "Kapak Açık";
            else
                str += "Kapak Kapalı";

            return str;

        }







        //public void Print(string value, bool cut)
        //{
        //    Int16 res;
        //    Int16 nWrite = 0;
        //    Int16 FontAttribute = 0;

        //    byte[] stringToBytes;
        //    string replacedString = "";

        //    //IEnumerable<string> source = IEnumerable<string>value;
        //    //if (Enumerable.Count<string>(source) > 1)
        //    if (!string.IsNullOrEmpty(value))
        //    {
        //        //if (value.Contains(cont)
        //        //{

        //        this.PrintBold(value, Nettuna7000IAPI, "\x00B2");
        //        //}

        //        //if (!cut)
        //        //  this.printQRCode(this.sbBarkod(printer).ToString(), Nettuna7000IAPI);
        //        if (cut)
        //            this.feedAndCut((short)1, Nettuna7000IAPI);

        //        stringToBytes = System.Text.Encoding.GetEncoding("CP857").GetBytes(value);
        //        Nettuna7000IAPI.setFontSize(Nettuna7000I_Global.hPrn, (short)0x00);
        //        Nettuna7000IAPI.setFontType(Nettuna7000I_Global.hPrn, (short)Nettuna7000I_FontType.FONT_A);
        //        FontAttribute = (short)Nettuna7000I_FontAttribute.FONT_A_DEFAULT;

        //        res = Nettuna7000IAPI.setFontAttribute(Nettuna7000I_Global.hPrn, FontAttribute);
        //        res = Nettuna7000IAPI.printText(Nettuna7000I_Global.hPrn, stringToBytes, (short)stringToBytes.Length, ref nWrite);
        //        if (cut)
        //        {
        //            res = Nettuna7000IAPI.feedPaperByLine(Nettuna7000I_Global.hPrn, 1);
        //            Nettuna7000IAPI.totalCut(Nettuna7000I_Global.hPrn);
        //        }
        //    }
        //}


    }
}
