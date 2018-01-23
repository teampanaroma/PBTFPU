using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaroma.FPU.Core
{
    /// <summary>
    /// Fpu durumunu okumak için oluşturulmuştur.
    /// </summary>
    public class FpuStatus : INotifyPropertyChanged
    {
        public EnumFpuStatus Status { get; set; }

        public bool Flag { get; set; }

        private string _TurkishStatus;

        public string TurkishStatus
        {
            get { return _TurkishStatus; }
            set { _TurkishStatus = value; OnPropertyChanged("TurkishStatus"); }
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} :{2}", Status, Flag, TurkishStatus);
        }

        //public string TurkishStatus{get;set;}


        List<FpuStatus> sList;
        public FpuStatus()
        {

        }
        public string GetTurkishEquivalent(EnumFpuStatus status)
        {
            if (sList == null)
            {
                sList = new List<FpuStatus>();
                sList.Add(new FpuStatus { Status = EnumFpuStatus.DataSyntaxError, TurkishStatus = "Veri Giriş Hatası" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.CmdInvalid, TurkishStatus = "Yanlış Komut" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.SModeOpenCap, TurkishStatus = "Servis Modunda Kapak Açık" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.SwitchKeyLost, TurkishStatus = "Anahtarlar Silindi" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.MeshKeyLost, TurkishStatus = "Mesh Anahtarları Silindi" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.SicNotFind, TurkishStatus = "Secure IC Yazılımı Bulunamadı" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.RtcBatteryLow, TurkishStatus = "Batarya Düşük" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.EJNoCard, TurkishStatus = "Ekü Bulunamadı" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.DailyMemFull, TurkishStatus = "Günlük Hafıza Dolu" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.EJFull, TurkishStatus = "Ekü Dolu" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.EJNearFull, TurkishStatus = "Ekü Dolmak Üzere" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.Not60EntryFM, TurkishStatus = "" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.FMFull, TurkishStatus = "Mali Hafıza Dolur" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.FisCodNumNotSet, TurkishStatus = "Mali Kod Ayarlanmadı" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.TinOrNinNotSet, TurkishStatus = "TC No ya da Vergi No Ayarlanmadı" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.TaxNotSet, TurkishStatus = "Vergi No Ayarlanmadı" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.FiscalRcptOpen, TurkishStatus = "Mali Fiş Açık" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.NonFisRcptOpen, TurkishStatus = "Mali Olmayan Fiş Açık" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.UserMode, TurkishStatus = "Kullanıcı Modu" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.FiscalMode, TurkishStatus = "Mali Mod" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.FMError, TurkishStatus = "Mali Hafıza Hatası" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.DailyMemErr, TurkishStatus = "Günlük Hafıza Hatası" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.EventFileErr, TurkishStatus = "Olay Kaydı Dosyası Hatası" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.TsmParaErr, TurkishStatus = "TSM Parametre Hatası" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.LastZReportNotOK, TurkishStatus = "Son Z raporu Hatalı" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.PreServiceMode, TurkishStatus = "Pre-Servis Modu" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.InServiceMode, TurkishStatus = "Servis Modu" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.PoweronALLVOID, TurkishStatus = "Elektrik Kesintisinden Yarım Kalan Belge" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.InvoiceData, TurkishStatus = "" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.PrnBufHaveData, TurkishStatus = "Buffer da Data Mevcut" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.WaitAppPrnStatus, TurkishStatus = "" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.ErrFisAppStart, TurkishStatus = "Mali Uygulama Başlatılamadı" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.ErrFisAppSign, TurkishStatus = "Mali Uygulama İmza Hatası" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.ErrFisAppHash, TurkishStatus = "Mali Uygulama Hash Hatası" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.ErrFisAppRead, TurkishStatus = "Mali Uygulama Okuma Hatası" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.ErrPsamRead, TurkishStatus = "Sam Kart Okuma Hatası" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.ErrPsamVerify, TurkishStatus = "Sam Kart Doğrulama Hatası" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.NeedToReboot, TurkishStatus = "Restar Edilmesi Gerek" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.CapOpen, TurkishStatus = "Kapaklar Açık" });
                sList.Add(new FpuStatus { Status = EnumFpuStatus.FMMiss, TurkishStatus = "Mali Hafıza Yerinde Değil" });

            }
            return sList.FirstOrDefault(x => x.Status == status).TurkishStatus;


        }



        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }



    /// <summary>
    /// Fpu 
    /// </summary>
    public static class FpuStausExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">FPU'nun rüm statuslerini alır ve geriye aktif olanları döner.</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool Check(this List<FpuStatus> obj, EnumFpuStatus status)
        {
            if (obj == null || obj.Count == 0)
                return false;

            var item = obj.Where(c => c.Status == status).FirstOrDefault();
            if (item == null)
                return false;
            return item.Flag;
        }


    }

    /// <summary>
    /// FPU STATUS
    /// </summary>
    public enum EnumFpuStatus
    {
        DataSyntaxError = 01,
        CmdInvalid = 02,
        SModeOpenCap = 03,
        SwitchKeyLost = 04,
        MeshKeyLost = 05,
        SicNotFind = 06,
        RtcBatteryLow = 10,
        EJNoCard = 11,
        DailyMemFull = 12,
        EJFull = 13,
        EJNearFull = 14,
        Not60EntryFM = 15,
        FMFull = 16,
        FisCodNumNotSet = 40,
        TinOrNinNotSet = 41,
        TaxNotSet = 42,
        FiscalRcptOpen = 43,
        NonFisRcptOpen = 44,
        UserMode = 45,
        FiscalMode = 46,
        FMError = 50,
        DailyMemErr = 51,
        EventFileErr = 52,
        TsmParaErr = 53,
        LastZReportNotOK = 54,
        PreServiceMode = 55,
        InServiceMode = 56,
        PoweronALLVOID = 62,
        InvoiceData = 63,
        ZRepOver24Hour = 64,
        PrnBufHaveData = 65,
        WaitAppPrnStatus = 66,
        ErrFisAppStart = 71,
        ErrFisAppSign = 72,
        ErrFisAppHash = 73,
        ErrFisAppRead = 74,
        ErrPsamRead = 75,
        ErrPsamVerify = 76,
        PrintingZReport = 80,
        EventFull = 81,
        ZCIrf = 82,
        NeedToReboot = 83,
        CapOpen = 84,
        FMMiss = 85,
        ReprintZRep = 91,
        OldEjF = 92,
        OtherEcrEj = 93,
        OneEjCopyIsBroken = 94,
        EndApproveF = 95,
        UnApproveF = 96,
        NTPOver45Day = 100,
        NTPOver60Day = 101,
        AllowPrnZRep = 102,
        PrnZRep=103,
        TmsNewVersion=105,
        InFPMode=106
    }



}
