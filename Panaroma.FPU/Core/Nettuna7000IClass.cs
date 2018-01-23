using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Panaroma.FPU.Core
{
    public enum Nettuna7000I_ErrorCode : short
    {
        Nettuna7000I_SUCCESS = 0,
        Nettuna7000I_E_ILLEGAL = 0x1001,
        Nettuna7000I_E_NOHANDLE = 0x1002,
        Nettuna7000I_E_NOHARDWARE = 0x1003,
        Nettuna7000I_E_FAILURE = 0x1004,
        Nettuna7000I_E_INUSE = 0x1005,
        Nettuna7000I_E_BUSY = 0x1006,
        Nettuna7000I_E_TIMEOUT = 0x1007,
        Nettuna7000I_E_INVALID_KEYCODE = 0x1009,
        Nettuna7000I_E_INVALID_IMAGE = 0x1010,
        Nettuna7000I_E_INVALID_COMMAND = 0x1011,
        Nettuna7000I_ERR_BAD_RESPONSE = 0x2000,		// DF_Turc
        Nettuna7000I_ERR_NO_CARD_DATA = 0x2001,		// DF_Turc
        Nettuna7000I_DATA_TO_LONG = 0x2002,		// DF_Turc
        Nettuna7000I_ERR_WRITE_DISPLAY = 0x2003,		// DF_Turc
        FileTooBig = -4

    }

    // Character set
    public enum Nettuna7000I_charSet : short
    {
        CHARSET_Usa = 0,
        CHARSET_France = 1,
        CHARSET_Germany = 2,
        CHARSET_Uk = 3,
        CHARSET_Denmark_I = 4,
        CHARSET_Sweden = 5,
        CHARSET_Italy = 6,
        CHARSET_Spain_I = 7,
        CHARSET_Japan = 8,
        CHARSET_Norway = 9,
        CHARSET_Denmark_II = 10,
        CHARSET_Spain_II = 11,
        CHARSET_LatinAmerica = 12,
        CHARSET_Korea = 13,
    }

    // Character code
    public enum Nettuna7000I_charCode : short
    {
        PAGECODE_PC437 = 0,
        PAGECODE_PC850 = 2,
        PAGECODE_PC860 = 3,
        PAGECODE_PC863 = 4,
        PAGECODE_PC865 = 5,
        PAGECODE_WPC1252 = 16,
        PAGECODE_PC866 = 17,
        PAGECODE_PC852 = 18,
        PAGECODE_PC858 = 19,

        PAGECODE_PC857_AZ = 27, //PIETRO azerbaijan

        PAGECODE_PC864 = 128,
        PAGECODE_PC857 = 129,
        PAGECODE_PC210 = 135,
        PAGECODE_PC862 = 136,
        PAGECODE_WPC1255 = 137,
        PAGECODE_WPC1257 = 138,
        PAGECODE_WPC1253 = 140,
        PAGECODE_WPC1254 = 141,
        PAGECODE_WPC1251 = 142,
    }

    //Status request
    public enum Nettuna7000I_StatusRequest : short
    {
        STATUS_PRINTER = 0x01,
        STATUS_OFFLINE = 0x02,
        STATUS_ERROR = 0x03,
        STATUS_ROLLPAPER = 0x04,
    }
    //DF_3
    public enum Nettuna7000I_PrinterID : short
    {
        ID_MODEL = 0x31,                  //ID MODEL
        FW_VERSION = 0x41,                //FW Version
    }
    //Status printer   
    public enum Nettuna7000I_StatusPrinter : int
    {
        Nettuna7000I_S_ONLINE = 0x0000000,
        Nettuna7000I_S_OFF = 0x0000011,
        Nettuna7000I_S_ROLLPAPER_NOPAPER = 0x0000012,

        Nettuna7000I_S_OFFLINE = 0x0010000,
        Nettuna7000I_S_OFFLINE_ERROR = 0x0000001,
        Nettuna7000I_S_COVER_OPEN = 0x0000002,
        Nettuna7000I_S_SECOND_COVER_OPEN = 0x0000003,		//DF_EVOTING
        Nettuna7000I_S_PAPER_EMPTY = 0x0000004,
        Nettuna7000I_S_NEAREND_PAPER_EMPTY = 0x0000005,		//DF_EVOTING
        Nettuna7000I_S_MECHANICAL_ERROR = 0x0000008,
        Nettuna7000I_S_UNRECOVERABLE_ERROR = 0x0000010,
        Nettuna7000I_S_POWER_OFF = 0x0000020,

    }
    public enum Nettuna7000I_Units : short
    {
        MAP_MODE_DOT = 1,
        MAP_MODE_INCH = 2,
        MAP_MODE_METRIC = 3,
    }

    public enum Nettuna7000I_MapModeUnit : int
    {
        MAX_FEED_DOT = 5900,
        MAX_FEED_METRIC = 10000,
        MAX_FEED_INCH = 40000,
    }

    public enum Nettuna7000I_BarcodeType : short
    {
        BARCODE_UPC_A = 65,
        BARCODE_UPC_E = 66,
        BARCODE_JAN13 = 67,
        BARCODE_JAN8 = 68,
        BARCODE_CODE39 = 69,
        BARCODE_ITF = 70,
        BARCODE_CODABAR = 71,
        BARCODE_CODE93 = 72,
        BARCODE_CODE128 = 73,
        BARCODE_PDF417 = 74,
    }

    public enum Nettuna7000I_TextPosition : short
    {
        BARCODE_NO_TEXT = 0,
        BARCODE_TEXT_ABOVE = 1,
        BARCODE_TEXT_BELOW = 2,
        BARCODE_TEXT_BOTH = 3,
    }
    // parity
    public enum Nettuna7000I_Parity : short
    {
        P_NOPARITY = 0,
        P_ODDPARITY = 1,
        P_EVENPARITY = 2,
    }
    // StopBits
    public enum Nettuna7000I_StopBits : short
    {
        P_ONESTOPBITS = 0,
        P_TWOSTOPBITS = 1,
    }
    // Baud
    public enum Nettuna7000I_Baud : int
    {
        P_1200 = 1200,
        P_2400 = 2400,
        P_4800 = 4800,
        P_9600 = 9600,
        P_19200 = 19200,
        P_38400 = 38400,
        P_57600 = 57600,
        P_115200 = 115200,
    }

    // DataBits
    public enum Nettuna7000I_Databits : short
    {
        P_SEVEN = 7,
        P_EIGHT = 8,
    }

    // Flow Control
    public enum Nettuna7000I_FlowControl : short
    {
        P_NONE = 0,
        P_HARDWARE = 1,
        P_XON_XOFF = 2,
    }

    public enum Nettuna7000I_FontType : short
    {
        FONT_A = 1,
        FONT_B = 2,
    }

    public enum Nettuna7000I_FontSize : short
    {
        FONT_HORZ_X1 = 0x00,
        FONT_HORZ_X2 = 0x10,
        FONT_HORZ_X3 = 0x20,
        FONT_HORZ_X4 = 0x30,
        FONT_HORZ_X5 = 0x40,
        FONT_HORZ_X6 = 0x50,
        FONT_HORZ_X7 = 0x60,
        FONT_HORZ_X8 = 0x70,

        FONT_VERT_X1 = 0x00,
        FONT_VERT_X2 = 0x01,
        FONT_VERT_X3 = 0x02,
        FONT_VERT_X4 = 0x03,
        FONT_VERT_X5 = 0x04,
        FONT_VERT_X6 = 0x05,
        FONT_VERT_X7 = 0x06,
        FONT_VERT_X8 = 0x07,
    }
    public enum Nettuna7000I_FontAttribute : short
    {
        FONT_A_DEFAULT = 0,
        FONT_A_BOLD = 0x01,
        FONT_A_DOUBLE_STRIKE = 0x02,
        FONT_A_UNDERLINE_1DOT = 0x04,
    }

    public enum Nettuna7000I_MapMode : short
    {
        MAP_MODE_DOT = 1,
        MAP_MODE_INCH = 2,
        MAP_MODE_METRIC = 3,
    }

    public enum Nettuna7000I_Aligment : short
    {
        ALIGN_LEFT = 1,
        ALIGN_CENTER = 2,
        ALIGN_RIGHT = 3,
    }
    public enum Nettuna7000I_Rotation : short
    {
        ROTATE_NORMAL = 1,
        ROTATE_RIGHT = 2,
        ROTATE_UP = 3,
    }

    public enum Nettuna7000I_Mode : short
    {
        MODE_NORMAL = 0x30,
        MODE_DB_WIDTH = 0x31,
        MODE_DB_HEIGHT = 0x32,
        MODE_DB_WIDTH_DB_HEIGHT = 0x33,
    }
    // in c# non ci sono variabili globali
    // per cui devo usare questo "trucco" di passare tramite una classe
    // che ha in se una variabile statica
    public class Nettuna7000I_Global
    {
        public static string Global;
        public static bool Prt100IsOpen;
        public static UInt16 hPrn;

        public static string NumeroScontrini;
    }

    public class Nettuna7000IClass
    {
        #region DllImport declaration


        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 OpenDevice([MarshalAs(UnmanagedType.LPStr)] string Portdevice, Int16 BaudRate, Int16 Parity, Int16 DataBits,
            Int16 StopBit, Int16 FlowControl, ref UInt16 hPrn);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 FeedAndCut(UInt16 hPrn, Int16 PositionCut);

        //DF_7
        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 PartialCut(UInt16 hPrn);
        //DF_7
        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 TotalCut(UInt16 hPrn);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 CloseDevice(UInt16 hPrn);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 SetDeviceAttribute(UInt16 hPrn, Int16 charSet, Int16 charCode);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 CheckDeviceStatus(UInt16 hPrn, ref int status, Int16 timeout);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 GetDeviceStatus(UInt16 hPrn, Int16 Function, ref Int16 status);

        //DF_3
        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 GetFWRelease(UInt16 hPrn, Int16 Function, StringBuilder rel);
        //private static extern Int16 GetFWRelease(UInt16 hPrn, Int16 Function , ref char[] rel);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 GetDllRelease(UInt16 hPrn, ref byte rel);
        //fine DF_3

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 ReadData(UInt16 hPrn, ref byte Data, Int16 nData, ref Int16 nRead);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 FeedPaperByLines(UInt16 hPrn, Int16 NumLines);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 FeedPaperByUnits(UInt16 hPrn, Int16 Units, Int16 MapMode);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 PrintText(UInt16 hPrn, [MarshalAs(UnmanagedType.LPStr)] string Data, Int16 nData, ref Int16 nWrite);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 PrintBarCode(UInt16 hPrn, Int16 BarcodeType, Int16 BarcodeWidth, Int16 BarcodeHeight, Int16 FontHri,
            Int16 TextPosition, StringBuilder Data, Int16 nData);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 SetFontAttribute(UInt16 hPrn, Int16 FontAttribute);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 SetFontSize(UInt16 hPrn, Int16 FontSize);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 SetFontType(UInt16 hPrn, Int16 FontType);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 SetLineSpacing(UInt16 hPrn, Int16 Spacing, Int16 MapMode);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 SetPrintableArea(UInt16 hPrn, Int16 MarginLeft, Int16 AreaWidth, Int16 MapMode);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 SetPrintAligment(UInt16 hPrn, Int16 Aligment);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 SetRotationMode(UInt16 hPrn, Int16 Rotation);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 GetGraphicsMemoryInfo(UInt16 hPrn, ref int Freememory, ref int UsedMemory);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 GetListCodeImage(UInt16 hPrn, ref Int16 nImage, IntPtr imgList, ref Int16 ImgLen);
        // private static extern Int16 GetListCodeImage(UInt16 hPrn, ref Int16 nImage, ref byte[] imgList, ref Int16 ImgLen);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 DeleteStoredImage(UInt16 hPrn, byte KeyC1, byte KeyC2);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 PrintImage(UInt16 hPrn, [MarshalAs(UnmanagedType.LPStr)] string ImageName, Int16 Mode);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 PrintStoredImage(UInt16 hPrn, byte KeyC1, byte KeyC2, Int16 Mode);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 StoreImage(UInt16 hPrn, [MarshalAs(UnmanagedType.LPStr)] string ImageName, byte KeyC1, byte KeyC2, Int16 ColorMode);
        #endregion
        #region DllImport_Turchia declaration
        //DF_Turc
        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 CardReaderEnable(UInt16 hPrn, bool bEnable, Int16 wReserved);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 CardReaderData(UInt16 hPrn, StringBuilder pReadBuff, ref int pdwBytesRead, Int16 dwReadTimeout);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 OpenDrawer(UInt16 hPrn, Int16 wReserved);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 ClearDisplay(UInt16 hPrn);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 DisplayOnOff(UInt16 hPrn, bool DispOn);

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 WriteDisplay(UInt16 hPrn, [MarshalAs(UnmanagedType.LPStr)] string Data1, [MarshalAs(UnmanagedType.LPStr)] string Data2);
        //Overlap write display
        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 WriteDisplay(UInt16 hPrn, byte[] Data1, byte[] Data2);
        // fine

        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 WriteExtDisplay(UInt16 hPrn, [MarshalAs(UnmanagedType.LPStr)] string Data, int Size);


        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 CalibratePaper(UInt16 hPrn);

        //Overlap PrintText
        [DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 PrintText(UInt16 hPrn, byte[] Data, Int16 nData, ref Int16 nWrite);

        //Prova QRCode
        [DllImport("Lib\\OL_QRCode.dll", CharSet = CharSet.Ansi)]
        private static extern Int16 OL_StringToQRCode(byte[] Data, int Scale);

        //NOT USED
        //[DllImport("Lib\\Nettuna7000I.dll", CharSet = CharSet.Ansi)]
        //private static extern Int16 SelectDeviceActive(UInt16 hPrn, Int16 Device);

        #endregion
        // fine DF_Turc

        public Int16 openDevice(string Port, Int16 BaudRate, Int16 Parity, Int16 DataBits,
            Int16 StopBit, Int16 FlowControl, ref UInt16 hPrn)
        {
            Int16 w_rc;

            w_rc = OpenDevice(Port, BaudRate, Parity, DataBits, StopBit, FlowControl, ref hPrn);

            return w_rc;
        }

        public Int16 feedAndCut(UInt16 hPrn, Int16 CutPosition)
        {
            Int16 w_rc;
            w_rc = FeedAndCut(hPrn, CutPosition);
            return w_rc;
        }

        public Int16 partialCut(UInt16 hPrn)
        {
            Int16 w_rc;
            w_rc = PartialCut(hPrn);
            return w_rc;
        }
        public Int16 totalCut(UInt16 hPrn)
        {
            Int16 w_rc;
            w_rc = TotalCut(hPrn);
            return w_rc;
        }

        public Int16 closeDevice(UInt16 hPrn)
        {
            Int16 w_rc;
            w_rc = CloseDevice(hPrn);
            return w_rc;
        }

        public Int16 setDeviceAttribute(UInt16 hPrn, Int16 charSet, Int16 charCode)
        {
            Int16 w_rc;
            w_rc = SetDeviceAttribute(hPrn, charSet, charCode);
            return w_rc;
        }
        public Int16 checkDeviceStatus(UInt16 hPrn, ref int status, Int16 timeout)
        {
            Int16 w_rc;
            w_rc = CheckDeviceStatus(hPrn, ref status, timeout);
            return w_rc;
        }

        public Int16 getDeviceStatus(UInt16 hPrn, Int16 Function, ref Int16 status)
        {
            Int16 w_rc;
            w_rc = GetDeviceStatus(hPrn, Function, ref status);
            return w_rc;
        }
        //DF_3
        public Int16 getFWRelease(UInt16 hPrn, Int16 Function, StringBuilder release)
        {
            Int16 w_rc;
            w_rc = GetFWRelease(hPrn, Function, release);
            return w_rc;
        }

        public Int16 getDllRelease(UInt16 hPrn, ref byte release)
        {
            Int16 w_rc;
            w_rc = GetDllRelease(hPrn, ref release);
            return w_rc;
        }
        // fine DF_3

        public Int16 readData(UInt16 hPrn, ref byte Data, Int16 nData, ref Int16 nRead)
        {
            Int16 w_rc;
            w_rc = ReadData(hPrn, ref Data, nData, ref nRead);
            return w_rc;
        }

        public Int16 feedPaperByLine(UInt16 hPrn, Int16 NumLines)
        {
            Int16 w_rc;
            w_rc = FeedPaperByLines(hPrn, NumLines);
            return w_rc;
        }

        public Int16 feedPaperByUnits(UInt16 hPrn, Int16 Units, Int16 MapMode)
        {
            Int16 w_rc;
            w_rc = FeedPaperByUnits(hPrn, Units, MapMode);
            return w_rc;
        }

        public Int16 printText(UInt16 hPrn, string Data, Int16 nData, ref Int16 nWrite)
        {
            Int16 w_rc;
            w_rc = PrintText(hPrn, Data, nData, ref nWrite);
            return w_rc;
        }
        public Int16 printText(UInt16 hPrn, byte[] Data, Int16 nData, ref Int16 nWrite)
        {
            Int16 w_rc;
            w_rc = PrintText(hPrn, Data, nData, ref nWrite);
            return w_rc;
        }

        public Int16 printBarCode(UInt16 hPrn, Int16 BarcodeType, Int16 BarcodeWidth, Int16 BarcodeHeight, Int16 FontHri,
            Int16 TextPosition, StringBuilder Data, Int16 nData)
        {
            Int16 w_rc;
            w_rc = PrintBarCode(hPrn, BarcodeType, BarcodeWidth, BarcodeHeight, FontHri, TextPosition, Data, nData);
            return w_rc;
        }

        public Int16 setFontAttribute(UInt16 hPrn, Int16 FontAttribute)
        {
            Int16 w_rc;
            w_rc = SetFontAttribute(hPrn, FontAttribute);
            return w_rc;
        }

        public Int16 setFontSize(UInt16 hPrn, Int16 FontSize)
        {
            Int16 w_rc;
            w_rc = SetFontSize(hPrn, FontSize);
            return w_rc;
        }

        public Int16 setFontType(UInt16 hPrn, Int16 FontType)
        {
            Int16 w_rc;
            w_rc = SetFontType(hPrn, FontType);
            return w_rc;
        }

        public Int16 setLineSpacing(UInt16 hPrn, Int16 Spacing, Int16 MapMode)
        {
            Int16 w_rc;
            w_rc = SetLineSpacing(hPrn, Spacing, MapMode);
            return w_rc;
        }

        public Int16 setPrintableArea(UInt16 hPrn, Int16 MarginLeft, Int16 AreaWidth, Int16 MapMode)
        {
            Int16 w_rc;
            w_rc = SetPrintableArea(hPrn, MarginLeft, AreaWidth, MapMode);
            return w_rc;
        }

        public Int16 setPrintAligment(UInt16 hPrn, Int16 Aligment)
        {
            Int16 w_rc;
            w_rc = SetPrintAligment(hPrn, Aligment);
            return w_rc;
        }

        public Int16 setRotationMode(UInt16 hPrn, Int16 Rotation)
        {
            Int16 w_rc;
            w_rc = SetRotationMode(hPrn, Rotation);
            return w_rc;
        }

        public Int16 getGraphicsmemoryInfo(UInt16 hPrn, ref int Freememory, ref int UsedMemory)
        {
            Int16 w_rc;
            w_rc = GetGraphicsMemoryInfo(hPrn, ref Freememory, ref UsedMemory);
            return w_rc;
        }

        public Int16 getListCodeImage(UInt16 hPrn, ref Int16 nImage, IntPtr imgList, ref Int16 ImgLen)
        {
            Int16 w_rc;
            w_rc = GetListCodeImage(hPrn, ref nImage, imgList, ref ImgLen);
            return w_rc;
        }

        public Int16 deleteStoredImage(UInt16 hPrn, byte KeyC1, byte KeyC2)
        {
            Int16 w_rc;
            w_rc = DeleteStoredImage(hPrn, KeyC1, KeyC2);
            return w_rc;
        }

        public Int16 printImage(UInt16 hPrn, string ImageName, Int16 Mode)
        {
            Int16 w_rc;
            w_rc = PrintImage(hPrn, ImageName, Mode);
            return w_rc;
        }

        public Int16 printStoredImage(UInt16 hPrn, byte KeyC1, byte KeyC2, Int16 Mode)
        {
            Int16 w_rc;
            w_rc = PrintStoredImage(hPrn, KeyC1, KeyC2, Mode);
            return w_rc;
        }

        public Int16 storeImage(UInt16 hPrn, string ImageName, byte KeyC1, byte KeyC2, Int16 ColorMode)
        {
            Int16 w_rc;
            w_rc = StoreImage(hPrn, ImageName, KeyC1, KeyC2, ColorMode);
            return w_rc;
        }

        //DF_Turc
        //        private static extern Int16 OL_StringToQRCode(byte[] Data);
        public Int16 StringToQRCode(byte[] Data1, int Scale)
        {
            Int16 w_rc;
            w_rc = OL_StringToQRCode(Data1, Scale);
            return w_rc;
        }
        public Int16 writeDisplay(UInt16 hPrn, string Data1, string Data2)
        {
            Int16 w_rc;
            w_rc = WriteDisplay(hPrn, Data1, Data2);
            return w_rc;
        }
        //public Int16 writeDisplay(UInt16 hPrn, byte[] Data1, byte[] Data2)
        //{
        //    Int16 w_rc;
        //    w_rc = WriteDisplay(hPrn, Data1, Data2);
        //    return w_rc;
        //}

        public Int16 displayOnOff(UInt16 hPrn, bool DispOn)
        {
            Int16 w_rc;
            w_rc = DisplayOnOff(hPrn, DispOn);
            return w_rc;
        }

        public Int16 clearDisplay(UInt16 hPrn)
        {
            Int16 w_rc;
            w_rc = ClearDisplay(hPrn);
            return w_rc;
        }

        public Int16 openDrawer(UInt16 hPrn, Int16 wReserved)
        {
            Int16 w_rc;
            w_rc = OpenDrawer(hPrn, wReserved);
            return w_rc;
        }

        //public Int16 cardReaderData(UInt16 hPrn, ref byte[] pReadBuff, ref Int16 pdwBytesRead, Int16 dwReadTimeout)
        public Int16 cardReaderData(UInt16 hPrn, StringBuilder pReadBuff, ref int pdwBytesRead, Int16 dwReadTimeout)
        {
            Int16 w_rc;
            w_rc = CardReaderData(hPrn, pReadBuff, ref pdwBytesRead, dwReadTimeout);
            return w_rc;
        }

        public Int16 cardReaderEnable(UInt16 hPrn, bool bEnable, Int16 wReserved)
        {
            Int16 w_rc;
            w_rc = CardReaderEnable(hPrn, bEnable, wReserved);
            return w_rc;
        }
        public Int16 PaperCalibrate(UInt16 hPrn)
        {
            Int16 w_rc;
            w_rc = CalibratePaper(hPrn);
            return w_rc;
        }
        //NOT USED
        //public Int16 selectDeviceActive(UInt16 hPrn, Int16 Device)
        //{
        //    Int16 w_rc;
        //    w_rc = SelectDeviceActive(hPrn, Device);
        //    return w_rc;
        //}

        public Int16 writeExtDisplay(UInt16 hPrn, string Data, int Size)
        {
            Int16 w_rc;
            w_rc = WriteExtDisplay(hPrn, Data, Size);
            return w_rc;
        }

        //Fine DF_Turc
    }
}
