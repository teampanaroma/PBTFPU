using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Panaroma.FPU.Core;
using ServiceProject.Service;
using System.Windows;
using ServiceProject;
using System.Timers;
using System.Windows.Threading;
using System.Threading;

namespace Panaroma.FPU
{
    /// <summary>
    /// FPU.Core dll inin dışa açılan fonksiyonlarını barındırır
    /// </summary>
    public class FpuOperations : IDisposable
    {
        private FpuManager _FPU;
        private List<FpuStatus> sModeExitList;
        public ErrorCode ErrorResult;
        private PrintOperations _PrinterOperation;
        private System.Timers.Timer aTimer;//Otomatik z raporu kontrolüdür.

        private bool _InReceipt = false;

        private bool _Disposed;

        public bool Disposed
        {
            get { return _Disposed; }
        }

        private List<EnumFpuStatus> _SModeList = new List<EnumFpuStatus>(){
                                         EnumFpuStatus.DataSyntaxError, EnumFpuStatus.CmdInvalid,
                                         EnumFpuStatus.CapOpen, EnumFpuStatus.SwitchKeyLost ,
                                         EnumFpuStatus.MeshKeyLost,EnumFpuStatus.SicNotFind,EnumFpuStatus.EJNoCard,
                                         EnumFpuStatus.FMError,
                                         EnumFpuStatus.EventFileErr,EnumFpuStatus.ErrFisAppStart,
                                         EnumFpuStatus.ErrFisAppStart,EnumFpuStatus.ErrFisAppSign,
                                         EnumFpuStatus.ErrFisAppHash,EnumFpuStatus.ErrFisAppRead,EnumFpuStatus.ErrPsamRead,
                                         EnumFpuStatus.ErrPsamVerify,EnumFpuStatus.CapOpen,EnumFpuStatus.FMMiss};

        /// <summary>
        /// Bağlantı tipi ve hangi porttan çalışılacagı belirtilir.
        /// </summary>
        /// <param name="connMode">Bağlantı tipi seri veya USB arayüzü ile çalışır.</param>
        /// <param name="port">Port numarası</param>
        public FpuOperations(int connMode, string port, bool printMode = true)
        {
            _FPU = new FpuManager(connMode, port, printMode);

            if (!_FPU.Open())
                throw new ApplicationException("Cihaz ile Bağlantı Kurulamadı " + "Connection Mode:" + connMode.ToString() + "Port numarası:" + port.ToString());
            AutoZReport();
        }

        #region Otomatik Z raporu yazdırmak için kullanılmıştır.

        private void AutoZReport()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 120);//60 saniyedeki kontrolü saat bası z raporu yazdırıldımı seklinde kontrol değiştirildi(120sn).
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!_InReceipt)
            {
                var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
                if (statusResult.StatusList.Check(EnumFpuStatus.ZRepOver24Hour))
                {
                    if (!statusResult.StatusList.Check(EnumFpuStatus.FiscalRcptOpen) && !statusResult.StatusList.Check(EnumFpuStatus.NonFisRcptOpen))
                    {
                        Over24HourZRep("");
                        Thread.Sleep(2000);
                    }
                }
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
            if (!statusResult.StatusList.Check(EnumFpuStatus.NonFisRcptOpen) && !statusResult.StatusList.Check(EnumFpuStatus.FiscalRcptOpen) && !statusResult.StatusList.Check(EnumFpuStatus.PreServiceMode))
            {
                if (statusResult.StatusList.Check(EnumFpuStatus.ZRepOver24Hour))
                {
                    GetZReport();
                }
            }
            else
                return;
        }

        #endregion Otomatik Z raporu yazdırmak için kullanılmıştır.

        public void Dispose()
        {
            if (_Disposed)
                throw new ApplicationException("FPU Portu zaten kapatıldı");

            _FPU.Close();
            _Disposed = true;
        }

        /// <summary>
        /// Satış işlemini başlatmak için kullanılan fonksiyondur. Mali fiş açar.
        /// </summary>
        /// <param name="clerkId">Kasiyer Id</param>
        /// <param name="InfomationSlipType">1 = InvoiceByHand, 2 = E-Invoice, 3 = E-Archive, 4 = InvoiceByPrinter 5=Yemek</param>
        /// <param name="ClerkPsw">Kasiyer Şifresi</param>
        /// <param name="CustomerMessage">Müşteri TCkimlik veya Vergi Numarası</param>
        /// <param name="SerialSequenceNo">Elle Fatura seri sıra numarası</param>
        /// <param name="RegisterF">RegisterF : '1' = Registered Customer, '0' = Unregistered Customer(If this field is not sent, you can regarded as '0')</param>
        ///if it is not Information slip, then no need offer these 3 fields: InfomationSlipType,Customer TIN or NIN, RegisterF
        /// <returns></returns>
        public FpuOperationResult OpenFiscalReceipt(int clerkId, int InfomationSlipType, string ClerkPsw, string CustomerMessage, string SerialSequenceNo, int RegisterF = 0)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.OpenFiscalReceipt, new object[] { clerkId, InfomationSlipType, ClerkPsw, CustomerMessage, SerialSequenceNo, RegisterF }));
            //if (fpuResult.Data == "")
            //{
            //    ErrorResult = ErrorCode.GetErrorExplanation("999");
            //    fpuResult.errorDetail = ErrorResult;
            //    fpuResult.ErrorMessage = "Belge Açılamadı";
            //    fpuResult.ErrorCode = 999;
            //    return fpuResult;
            //}
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            _InReceipt = true;
            return fpuResult;
        }

        /// <summary>
        /// Fpu nun açılışta satış yapabilmesi için gerekli kontrolleri yapar.
        /// Açılışta bu kontrollerin sağlanması gerekir.
        /// </summary>
        /// <returns></returns>
        public List<string> CheckOpenOperations()
        {
            //Açılışta dönecek hataları bildirir.
            List<string> ErrorList = new List<string>();
            if (!_FPU.FpuBusy && _InReceipt)
            {
                ErrorList.Add("OPEN FISCAL RECEIPT PLEASE OPEN DEMO APP AFTER CLOSE FISCAL RECEIPT");
            }

            if (!_FPU.FpuBusy && !_InReceipt)
            {
                var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
                //Kasa nın servis konumunda olup olmadığı kontrol edilir.
                if (CanStartSMode())
                {
                    ErrorList.Add("Please Login S Mode");
                }
                //Herhangi bir departman Programlı mı?
                ////if (!CheckAnyDepartmentDefined())
                ////{
                ////    ErrorList.Add("Department not Program ");
                ////}
                //Vergi oranları Programlı mı?

                if (statusResult.StatusList.Check(EnumFpuStatus.TaxNotSet))
                {
                    ErrorList.Add("Please Set Tax");
                }

                //if (!statusResult.StatusList.Check(EnumFpuStatus.FiscalMode))
                //{
                //    ErrorList.Add("Not Fiscal Mode.");
                //}

                ////Grup Programlı mı?
                //if (!CheckAnyGroupDefined())
                //{
                //    ErrorList.Add("Sales Test should be start");
                //}

                //Açık Belge var mı?

                if (statusResult.StatusList.Check(EnumFpuStatus.FiscalRcptOpen) || statusResult.StatusList.Check(EnumFpuStatus.NonFisRcptOpen))
                {
                    ErrorList.Add("OPEN FISCAL RECEIPT PLEASE OPEN DEMO APP AFTER CLOSE FISCAL RECEIPT");
                }

                //24 saat geçtiyse otomatik Z raporu yazdırılsın
                //if (!_InReceipt)
                //{
                if (!statusResult.StatusList.Check(EnumFpuStatus.FiscalRcptOpen) && !statusResult.StatusList.Check(EnumFpuStatus.NonFisRcptOpen))
                {
                    if (statusResult.StatusList.Check(EnumFpuStatus.ReprintZRep) && statusResult.StatusList.Check(EnumFpuStatus.AllowPrnZRep) && !statusResult.StatusList.Check(EnumFpuStatus.PreServiceMode))
                    {
                        if (statusResult.StatusList.Check(EnumFpuStatus.ZRepOver24Hour))
                        {
                            Over24HourZRep("");
                        }
                        else
                        {
                            GetZReport();
                        }
                    }
                    else if (statusResult.StatusList.Check(EnumFpuStatus.ZRepOver24Hour) && statusResult.StatusList.Check(EnumFpuStatus.AllowPrnZRep))
                    {
                        Over24HourZRep("");
                    }
                }
                //}
            }
            //List
            return ErrorList;
        }

        /// <summary>
        /// Departmanların programlı olup olmadığı kontrol edilir.
        /// 12 departmanın her biri programlı mı diye kontrol edilir.
        /// </summary>
        /// <returns></returns>
        public bool CheckAnyDepartmentDefined()
        {
            for (int i = 1; i <= 12; i++)
            {
                var result = ReadDepartment(i);
                if (result.ErrorCode == 0 && result.DepId == i)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Grupların programlı olup olmadığını kontrol eder.
        /// İlk 12 tane grup kontrol edilmiştir.
        /// </summary>
        /// <returns></returns>
        public bool CheckAnyGroupDefined()
        {
            for (int i = 1; i <= 12; i++)
            {
                var result = ReadGroup(i);
                if (result.ErrorCode == 0 && result.GroupId == i)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Kasiyer Login işlemi için kullanılan Fonksiyondur.
        /// </summary>
        /// <param name="cashierId">Kasiyer Numarası</param>
        /// <param name="cashierPassword">Kasiyer Şifresi</param>
        /// <returns></returns>
        public FpuOperationResult CashierLogin(int cashierId, string cashierPassword)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.CashierLogin, new object[] { cashierId, cashierPassword }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        public FpuOperationResult PrintOKorInterrupt(string TFlag)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintOKorInterrupt, new object[] { TFlag }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Satış işlemini başlatıldıktan sonra ürün bilgileri bu fonksiyon aracılığıyla FPU ya gönderilir. Aynı zamanda ürün ve indirim/artırım iptal işlemi için de kullanılır.
        /// </summary>
        /// <param name="PluName">Ürün adı</param>
        /// <param name="Flag">"" kullanılırsa normal satış "R" FPU da değil CRV de kullanılıyor "-" iptal satırı</param>
        /// <param name="PluBarcode">Ürün barkodu</param>
        /// <param name="FlagType">"P" yüzde indirim/artırım "D" tutar indirim artırım . İndirim/Artırım satırı iptal edilirken de bu flag kullanılır.</param>
        /// <param name="DepId">Bağlı oldugu departmanın id si</param>
        /// <param name="GroupId">Bağlı oldugu grup id si</param>
        /// <param name="DepOrPlu">0 departman satışı. 1 barkodlu satış</param>
        /// <param name="unitPrice">Birim Fiyat</param>
        /// <param name="Value">İptal edilecek indirim/artırım tutarı gönderilir. indirim iptal edilecek ise değerin başına (-) konulur.</param>
        /// <param name="quantity">Adedi</param>
        /// <returns></returns>
        public FpuOperationResult Registeringsale(string PluName, string Flag, string PluBarcode, string FlagType, int DepId, int GroupId, int DepOrPlu, decimal unitPrice, decimal Value, decimal quantity)
        {
            unitPrice = unitPrice * 100;
            quantity = quantity * 1000;
            Value = 0m;
            if (Flag == "-")
            {
                Value = Value * 100;
            }

            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.Registeringsale, new object[] { PluName, Flag, PluBarcode, FlagType, DepId, GroupId, DepOrPlu, Convert.ToInt32(unitPrice), Convert.ToInt32(Value), Convert.ToInt32(quantity) }));
            //if (fpuResult.Data == "")
            //{
            //    ErrorResult = ErrorCode.GetErrorExplanation("999");
            //    fpuResult.errorDetail = ErrorResult;
            //    fpuResult.ErrorMessage = "Satış Gönderilemedi";
            //    fpuResult.ErrorCode = 999;
            //    return fpuResult;
            //}

            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Satış fişi iptal etmek için kullanılan fonksiyondur.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult CancelFiscalReceipt()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.CancelFiscalReceipt));
            if (fpuResult.Data == "")
            {
                ErrorResult = ErrorCode.GetErrorExplanation("999");
                fpuResult.errorDetail = ErrorResult;
                fpuResult.ErrorMessage = "Belge Kapatılamadı";
                fpuResult.ErrorCode = 999;
                return fpuResult;
            }
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            _InReceipt = false;
            return fpuResult;
        }

        /// <summary>
        /// Müşteri displayine data yazmak için kullanılır
        /// </summary>
        /// <param name="line1">1. satır</param>
        /// <param name="line2">2. satır</param>
        public void WriteToDisplay(string line1, string line2)
        {
            _FPU.WriteToDisplay(line1, line2);
        }

        /// <summary>
        /// Para çekmecesi açma fonksiyonudur.
        /// </summary>
        public void OpenDrawer()
        {
            _FPU.OpenDrawer();
        }

        /// <summary>
        /// Satır sırası girilerek kaydedilen Fiş başlığı okunur.
        /// </summary>
        /// <param name="Item">Okunacak satır sayısı</param>
        /// <returns></returns>
        public FpuOperationResult ReadHeaderMessage(int item)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadHeaderMessage, new object[] { item }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Belge Sonu satır sayısı girilerek kaydedilen mesaj okunur.
        /// </summary>
        /// <param name="item">Okunacak satır sayısı</param>
        /// <returns></returns>
        public FpuOperationResult ReadFootMessage(int item)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadFootMessage, new object[] { item }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Dövizli ödeme tipi set etmek için kullanılan methoddur.
        /// </summary>
        /// <param name="currencyIndex">Dövizin kaydedileceği sıra 1-6 arasında değer girilir</param>
        /// <param name="currencyName">Döviz adı maksimum 10 byte olarak girilir.</param>
        /// <param name="exchangeRate">9 digit virgülden sonra 4 hane girilecek şekilde ayarlanmalıdır.</param>
        /// <param name="subsidiary">bağımlılık</param>
        /// <returns></returns>
        public FpuOperationResult SetForeignCurrency(int currencyIndex, string currencyName, double exchangeRate, Boolean subsidiary)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetForeignCurrency, new object[] { currencyIndex, currencyName, exchangeRate, subsidiary }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Muayene Ücreti eklemek için kullanılır
        /// Ödemeden önce gönderilmesi gerekmektedir.
        /// </summary>
        /// <param name="Price">Muayene ücreti</param>
        /// <returns></returns>
        public FpuOperationResult checkuppayment(double Price)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.checkuppayment, new object[] { Price }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Reçete Katkı Payı eklemek için kullanılır
        /// Ödemeden önce gönderilmesi gerekmektedir.
        /// </summary>
        /// <param name="Price">Reçete Katkı Payı ücreti</param>
        /// <returns></returns>
        public FpuOperationResult presaddedvalue(double Price)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.presaddedvalue, new object[] { Price }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Diğer ödeme tiplerini tanımlamak için kullanılır.
        /// </summary>
        /// <param name="row">Hangi sıradaki ödeme tipinin seçileceği bildirilir</param>
        /// <param name="description">Ödeme tipinin adının verildiği parametredir.</param>
        /// <returns></returns>
        public FpuOperationResult DefAddPayName(int row, string description)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.DefAddPayName, new object[] { row, description }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// 16 haneli Mersis numarasını sisteme set etmek için kullanılan alandır.
        /// </summary>
        /// <param name="mersisNo">16 haneli mersis numarası</param>
        /// <returns></returns>
        public FpuOperationResult SettingMersisNo(string mersisNo)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SettingMersisNo, new object[] { mersisNo }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Acil durum belge iptali / ödeme varsa dahi iptal eder.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult SpecilCancelFiscalReceipt()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SpecilCancelFiscalReceipt));
            //if (fpuResult.Data == "")
            //{
            //    ErrorResult = ErrorCode.GetErrorExplanation("999");
            //    fpuResult.errorDetail = ErrorResult;
            //    fpuResult.ErrorMessage = "Belge Kapatılamadı";
            //    fpuResult.ErrorCode = 999;
            //    return fpuResult;
            //}
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            _InReceipt = false;
            return fpuResult;
        }

        /// <summary>
        /// Kayıtlı mersis numarası varsa okumak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult ReadMersisNo()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadMersisNo));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Maksimum satış fişi limitini belirler. Belirlenen limitin aşılmasına izin verilmez.
        /// Örneğin, bu limit aşıldığı zaman kullanıcının faturaya geçmesi istenebilir.
        /// </summary>
        /// <param name="priceLimit">Maksimum fiş satış limiti</param>
        /// <returns></returns>
        public FpuOperationResult SetPriceLimit(double priceLimit)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetPriceLimit, new object[] { priceLimit }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Mali bir belge içersine text yazmak için kullanılır.
        /// </summary>
        /// <param name="fiscalFreeText">Yazılmak istenen text</param>
        /// <returns></returns>
        public FpuOperationResult PrintFreeFiscalText(string fiscalFreeText)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintFreeFiscalText, new object[] { fiscalFreeText }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Mali olmayan belge içerisine text yazmak için kullanılır.
        /// </summary>
        /// <param name="nonFreemessage">Yazılmak istenen mesaj</param>
        /// <returns></returns>
        public FpuOperationResult PrintNonFiscalFreeText(string nonFreemessage)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintNonFiscalFreeText, new object[] { nonFreemessage }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Kaydedilmiş Seri numarasını okumak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult ReadMachineSerialId()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadMachineSerialId));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Mali Olmayan Belge Başlatma işlemini yapar.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult OpenNonFiscalReceipt()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.OpenNonFiscalReceipt));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            _InReceipt = true;
            return fpuResult;
        }

        /// <summary>
        /// Mali olmayan belge kapatma işlemini yapar.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult CloseNonFiscalReceipt()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.CloseNonFiscalReceipt));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            _InReceipt = false;
            return fpuResult;
        }

        ///// <summary>
        ///// Fatura dışındaki belgelerin X ve Z raporlarındaki alanlarını doldurmak için oluşturulmuştur.
        ///// </summary>
        ///// <param name="ExpeNumber"></param>
        ///// <param name="ExpeCancelNum"></param>
        ///// <param name="ExpeTotal"></param>
        ///// <param name="ExpeTax"></param>
        ///// <param name="ExpeCash"></param>
        ///// <param name="ExpeCredit"></param>
        ///// <param name="ExpeOtherPayTotal"></param>
        ///// <param name="ExpeFcTotal"></param>
        ///// <param name="ExpeFcChange"></param>
        ///// <param name="ExpeCancelTotal"></param>
        ///// <param name="ShipNumber"></param>
        ///// <param name="ShipCancelNum"></param>
        ///// <param name="ShipTotal"></param>
        ///// <param name="ShipTax"></param>
        ///// <param name="ShipCash"></param>
        ///// <param name="ShipCredit"></param>
        ///// <param name="ShipOtherPayTotal"></param>
        ///// <param name="ShipFcTotal"></param>
        ///// <param name="ShipFcChange"></param>
        ///// <param name="ShipCancelTotal"></param>
        ///// <param name="PayNumber"></param>
        ///// <param name="PayCancelNum"></param>
        ///// <param name="PayTotal"></param>
        ///// <param name="PayTax"></param>
        ///// <param name="PayCash"></param>
        ///// <param name="PayCredit"></param>
        ///// <param name="PayOtherPayTotal"></param>
        ///// <param name="PayFcTotal"></param>
        ///// <param name="PayFcChange"></param>
        ///// <param name="PayCancelTotal"></param>
        ///// <param name="ExcNumber"></param>
        ///// <param name="ExcCancelNum"></param>
        ///// <returns></returns>
        //public FpuOperationResult SetReportOtherInformation(int ExpeNumber, int ExpeCancelNum, double ExpeTotal, double ExpeTax, double ExpeCash, double ExpeCredit, double ExpeOtherPayTotal,
        //                                                     double ExpeFcTotal, double ExpeFcChange, double ExpeCancelTotal,
        //                                                     int ShipNumber, int ShipCancelNum, double ShipTotal, double ShipTax, double ShipCash, double ShipCredit, double ShipOtherPayTotal,
        //                                                     double ShipFcTotal, double ShipFcChange, double ShipCancelTotal,
        //                                                     int PayNumber, int PayCancelNum, double PayTotal, double PayTax, double PayCash, double PayCredit, double PayOtherPayTotal,
        //                                                     double PayFcTotal, double PayFcChange, double PayCancelTotal,
        //                                                     int ExcNumber, int ExcCancelNum)
        //{
        //    return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.setReportOtherInformation, new object[]{
        //                                                     ExpeNumber, ExpeCancelNum, ExpeTotal, ExpeTax, ExpeCash, ExpeCredit, ExpeOtherPayTotal,
        //                                                     ExpeFcTotal,  ExpeFcChange,  ExpeCancelTotal,
        //                                                     ShipNumber,  ShipCancelNum,  ShipTotal,  ShipTax,  ShipCash,  ShipCredit,  ShipOtherPayTotal,
        //                                                     ShipFcTotal,  ShipFcChange,  ShipCancelTotal,
        //                                                     PayNumber, PayCancelNum,  PayTotal,  PayTax,  PayCash,  PayCredit,  PayOtherPayTotal,
        //                                                     PayFcTotal, PayFcChange,  PayCancelTotal,
        //                                                     ExcNumber, ExcCancelNum}));
        //}

        /// <summary>
        /// Kasiyer Adını değiştirmek için kullanılır.
        /// </summary>
        /// <param name="clerkId">Kasiyer id</param>
        /// <param name="clerkName">Kasiyer Adı</param>
        /// <returns></returns>
        public FpuOperationResult SetClerkName(int clerkId, string clerkName)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetClerkName, new object[] { clerkId, clerkName }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Kasiyer şifresi set etmek için kullanılır.
        /// </summary>
        /// <param name="ClerkId">Kasiyer id</param>
        /// <param name="newPassword">Şifresi</param>
        /// <param name="ClerkName">Kasiyer Adı</param>
        /// <returns></returns>
        public FpuOperationResult SetClerkPswd(int ClerkId, string newPassword, string ClerkName)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetClerkPswd, new object[] { ClerkId, newPassword, ClerkName }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Askıya belge kaydetmek için kullanılır.
        /// </summary>
        /// <param name="fiscalReceiptNo">Askıya alınacak fiş numarası.</param>
        /// <returns></returns>
        public FpuOperationResult SaveFiscalReceipt(int fiscalReceiptNo)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SaveFiscalReceipt, new object[] { fiscalReceiptNo }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Askıya alınmış bir belgeyi geri çağırmak için kullanılır.
        /// </summary>
        /// <param name="fiscalReceiptNo">Fiş numarası</param>
        /// <param name="total">Toplam tutarı</param>
        /// <param name="totTax">Toplam Kdv tutarı</param>
        /// <returns></returns>
        public FpuOperationResult PrintRecallinfomation(int fiscalReceiptNo, double total, double totTax)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintRecallinfomation, new object[] { fiscalReceiptNo, total, totTax }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// KDV ayarlarını set etme işlemi yapılır. Toplamda 8 adet vardır.
        /// </summary>
        /// <param name="taxA">1 numaralı KDV</param>
        /// <param name="taxB">2 numaralı KDV</param>
        /// <param name="taxC">3 numaralı KDV</param>
        /// <param name="taxD">4 numaralı KDV</param>
        /// <param name="taxE">5 numaralı KDV</param>
        /// <param name="taxF">6 numaralı KDV</param>
        /// <param name="taxG">7 numaralı KDV</param>
        /// <param name="taxH">8 numaralı KDV</param>
        /// <returns></returns>
        public FpuOperationResult SettingTaxRate(double taxA, double taxB, double taxC, double taxD, double taxE, double taxF, double taxG, double taxH)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SettingTaxRate, new object[] { taxA, taxB, taxC, taxD, taxE, taxF, taxG, taxH }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// İndirim/Artırım işlemi için kullanılır.
        /// </summary>
        /// <param name="flagType">P--> -/+ % indirimi/artırımı yapar. D--> -/+ tutar indirim/artırımı yapar.</param>
        /// <param name="value">indirim/artırım tutarı girilir.(-) girilirse indirim (+) girilirse artırım yapar.</param>
        /// <returns></returns>
        public FpuOperationResult PreAndDiscount(string flagType, decimal value)
        {
            value = value * 100;
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PreAndDiscount, new object[] { flagType, Convert.ToInt32(value) }));
            if (fpuResult.Data == "")
            {
                ErrorResult = ErrorCode.GetErrorExplanation("999");
                fpuResult.errorDetail = ErrorResult;
                fpuResult.ErrorMessage = "İndirim Yapılamadı";
                fpuResult.ErrorCode = 999;
                return fpuResult;
            }
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Departman Set edilmek için kullanılan fonksiyondur. Toplam 12 adet departman bulunmaktadır.
        /// </summary>
        /// <param name="DepId">1-12 arası değer girilmelidir.</param>
        /// <param name="TaxGr">Bağlı olduğu kdv oranı</param>
        /// <param name="Limit">Maximum satış yapılacağı tutar</param>
        /// <param name="UnitPrice">Dep satışındaki tutarı</param>
        /// <param name="Depname">Departmana verilecek isim</param>
        /// <param name="IsPrintF"></param>
        /// <returns></returns>
        public FpuOperationResult SetDepartments(int DepId, string TaxGr, double Limit, double UnitPrice, string Depname, string IsPrintF)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetDepartments, new object[] { DepId, TaxGr, Limit, UnitPrice, Depname, IsPrintF }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Group Programlama yapmak için kullanılır. Bir departmanın birden fazla grubu olabilir.
        /// </summary>
        /// <param name="GroupId">Group Id</param>
        /// <param name="DepId">Bağlı olduğu departman numarası</param>
        /// <param name="GroupName">Grup adı</param>
        /// <param name="IsPrintF"></param>
        /// <returns></returns>
        public FpuOperationResult SetGroup(int GroupId, int DepId, string GroupName, string IsPrintF)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetGroup, new object[] { GroupId, DepId, GroupName, IsPrintF }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Kaydedilmiş dövizleri okur. Indeksi verilerek döviz bilgileri okunur.
        /// </summary>
        /// <param name="currencyIndex">Döviz index</param>
        /// <returns></returns>
        public FpuOperationResult ReadForeignCurrency(int currencyIndex)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadForeignCurrency, new object[] { currencyIndex }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Sırasını girerek ödeme isimlerini okumak için kullanılır.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public FpuOperationResult ReadPayNmae(int option)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadPayNmae, new object[] { option }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Sıfır tutarlı belge özelliğini açar.
        /// </summary>
        /// <param name="type">1 olursa bu özellik açılır.</param>
        /// <returns></returns>
        public FpuOperationResult FinalizingreceiptSwitch(int type)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.finalizingreceiptSwitch, new object[] { type }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Kasa nakit durumunu sorgulamak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public GetCashInOutDrawer GetCashInDrawer()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.GetCashInDrawer);
            GetCashInOutDrawer result = new GetCashInOutDrawer
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
            };
            if (fpuResult.ErrorCode != 0)
            {
                ErrorResult = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());

                return new GetCashInOutDrawer { errorDetail = ErrorResult };
            }
            if (fpuResult.DataArray.Length > 3)
            {
                result.Cash = fpuResult.DataArray[1];
                result.CashIn = fpuResult.DataArray[2];
                result.CashOut = fpuResult.DataArray[3];
            }
            return result;
        }

        /// <summary>
        /// Satış fişini kapatmak için kullanılan fonksiyondur.
        /// Ödeme alındıysa kullanılabilir. Mali simge basar.
        /// MF basılan fonksiyondur.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult CloseFiscalReceipt()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.CloseFiscalReceipt));
            if (fpuResult.Data == "")
            {
                ErrorResult = ErrorCode.GetErrorExplanation("999");
                fpuResult.errorDetail = ErrorResult;
                fpuResult.ErrorMessage = "Belge Kapatılamadı";
                fpuResult.ErrorCode = 999;
                return fpuResult;
            }
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            _InReceipt = false;
            return fpuResult;
        }

        /// <summary>
        /// Fpu nun anlık statülerini öğrenmek için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult ReciveFPUStatus()
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus));
        }

        /// <summary>
        /// Son mali işlem kaydın bilgilerini veren fonsksiyondur.
        /// </summary>
        /// <param name="receiptFlag">"" Boş geçilirse Z raporu bilgilerini "R" son mali fiş bilgilerini verir.</param>
        /// <returns></returns>
        public FpuOperationResult ReadLastFiscal(string receiptFlag)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadLastFiscal, new object[] { receiptFlag }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Fpu nun anlık hangi modda olduğunu öğrenmek için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult GetFpuCurrentMode()
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.GetFpuCurrentMode));
        }

        /// <summary>
        /// Fpunun servis yada preservismode içerisinde olup olmadığını verir
        /// </summary>
        /// <returns>True dönerse kasa servis konumuna girmek zorundadır   </returns>
        public bool GetFpuServiceMode()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
            if (statusResult.StatusList.Check(EnumFpuStatus.InServiceMode) || statusResult.StatusList.Check(EnumFpuStatus.PreServiceMode))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Bu ne işi yarıyor öğren ????
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult GetFreeNumber()
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.GetFreeNumber));
        }

        /// <summary>
        /// Ödeme fonksiyonudur.
        /// </summary>
        /// <param name="modeType">Ödeme tipi belirtilir. Ödeme tipinin sırası girilir. 0 :Nakit, 1:Kredi EFT-POS suz, 2:Çek 3:-12: diğer öedeme, 13:-18: Döviz, 19:Kredi-EFTPOS</param>
        /// <param name="amount">Ödeme tutarı</param>
        /// <returns></returns>
        public FpuOperationResult TotalCalculating(int ModeType, decimal Amount, int iType = 0, int InstalmentNumber = 0)
        {
            Amount = Amount * 100;
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.Totalcalculating, new object[] { ModeType, Convert.ToInt32(Amount), iType, InstalmentNumber }));
            //if (fpuResult.Data == "")
            //{
            //    ErrorResult = ErrorCode.GetErrorExplanation("999");
            //    fpuResult.errorDetail = ErrorResult;
            //    fpuResult.ErrorMessage = "Ödeme Alınamadı";
            //    fpuResult.ErrorCode = 999;
            //    return fpuResult;
            //}
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Elle fatura düzenleme fonksiyonudur.
        /// </summary>
        /// <param name="SerialNo">Seri numarası</param>
        /// <param name="InvoiceNo">Fatura numarası</param>
        /// <param name="Total">Toplam tutar</param>
        /// <param name="Cash">Nakit</param>
        /// <param name="Credit">Kredi</param>
        /// <param name="Check">Çek</param>
        /// <param name="FcName1">Döviz1</param>
        /// <param name="FcName2">Döviz2</param>
        /// <param name="FcName3">Döviz3</param>
        /// <param name="FcName4">Döviz4</param>
        /// <param name="FcName5">Döviz5</param>
        /// <param name="FcName6">Döviz6</param>
        /// <param name="FcAmt1">Döviz Tutarı1</param>
        /// <param name="FcAmt2">Döviz Tutarı2</param>
        /// <param name="FcAmt3">Döviz Tutarı3</param>
        /// <param name="FcAmt4">Döviz Tutarı4</param>
        /// <param name="FcAmt5">Döviz Tutarı5</param>
        /// <param name="FcAmt6">Döviz Tutarı6</param>
        /// <param name="FcLocal1"></param>
        /// <param name="FcLocal2"></param>
        /// <param name="FcLocal3"></param>
        /// <param name="FcLocal4"></param>
        /// <param name="FcLocal5"></param>
        /// <param name="FcLocal6"></param>
        /// <param name="InvoiceDate">Fatura Tarihi</param>
        /// <returns></returns>
        public FpuOperationResult Printinvoicebyhand(string SerialNo, string InvoiceNo, double Total, double Cash, double Credit, double Check,
                                                         string FcName1, string FcName2, string FcName3, string FcName4, string FcName5, string FcName6,
                                                         double FcAmt1, double FcAmt2, double FcAmt3, double FcAmt4, double FcAmt5, double FcAmt6,
                                                         double FcLocal1, double FcLocal2, double FcLocal3, double FcLocal4, double FcLocal5, double FcLocal6, string InvoiceDate)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.Printinvoicebyhand, new object[]{SerialNo,InvoiceNo,Total,Cash,Credit,Check,
                FcName1,FcName2,FcName3,FcName4,FcName5,FcName6,FcAmt1,FcAmt2,FcAmt3,FcAmt4,FcAmt5,FcAmt6,
                FcLocal1,FcLocal2,FcLocal3,FcLocal4,FcLocal5,FcLocal6,InvoiceDate}));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Fatura başlığı programlamak için kullanılır.
        /// </summary>
        /// <param name="InvCustomerStr0">Müşteri satırı</param>
        /// <param name="InvCustomerStr1">Müşteri satırı</param>
        /// <param name="InvCustomerStr2">Müşteri satırı</param>
        /// <param name="InvCustomerStr3">Müşteri satırı</param>
        /// <param name="InvCustomerStr4">Müşteri satırı</param>
        /// <param name="SerialNo">Seri Numrası</param>
        /// <param name="InvoiceNum">Fatura Numarası</param>
        /// <returns></returns>
        public FpuOperationResult PrintDataInRec(string InvCustomerStr0, string InvCustomerStr1, string InvCustomerStr2, string InvCustomerStr3, string InvCustomerStr4, string SerialNo, string InvoiceNum)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintDataInRec, new object[] { InvCustomerStr0, InvCustomerStr1, InvCustomerStr3, InvCustomerStr4, SerialNo, InvoiceNum }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// SOR????
        /// </summary>
        /// <param name="repTypeIndex"></param>
        /// <returns></returns>
        public FpuOperationResult PrintMessage(int repTypeIndex)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintMessage, new object[] { repTypeIndex }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Eküden Z numarası ve Fiş numarası girilerek tek belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="ZReportNo">İstenen belgenin Z numarası</param>
        /// <param name="ReceiptNo">İstenen belgenin Fiş Numarası</param>
        /// <returns></returns>
        public FpuOperationResult ReadEJSpecificReceiptByNumber(int ZReportNo, int ReceiptNo)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadEJSpecificReceiptByNumber, new object[] { ZReportNo, ReceiptNo }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Ekü detay raporu yazdırmak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult EJModuleReport()
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.EJModuleReport));
        }

        /// <summary>
        /// Tarih saat girilerek eküden belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="dateTime">Yazdırılmak istenilen tarih ve saat bilgisi</param>
        /// <returns></returns>
        public FpuOperationResult ReadEJSpecificReceiptByDatetime(string dateTime)
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadEJSpecificReceiptByDatetime, new object[] { dateTime }));
        }

        /// <summary>
        /// Tarih ve Fİş Numarası girilerek Eküden belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="currentDatetime">Eküden okunmak istenen belgenin tarihi</param>
        /// <param name="receiptNo">Eküden okunmak istenen belgenin fiş numarası</param>
        /// <returns></returns>
        public FpuOperationResult ReadEJSpecificReceiptByDatetimeNo(string currentDatetime, int receiptNo)
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadEJSpecificReceiptByDatetimeNo, new object[] { currentDatetime, receiptNo }));
        }

        /// <summary>
        /// İki fiş İki Z no arasında eküden dönemsel belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="firstZNumber">İlk Z Numarası</param>
        /// <param name="lastZNumber">Son Z Numarası</param>
        /// <param name="firstReceiptNumber">İlk Fiş Numarası</param>
        /// <param name="lastReceiptNumber">Son Fiş Numarası</param>
        /// <returns></returns>
        public FpuOperationResult ReadEJSpecificReceiptByNumberToNumber(int firstZNumber, int lastZNumber, int firstReceiptNumber, int lastReceiptNumber)
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadEJSpecificReceiptByNumberToNumber, new object[] { firstZNumber, lastZNumber, firstReceiptNumber, lastReceiptNumber }));
        }

        /// <summary>
        /// Girilen iki tarih aralığında eküden belge okuma işlemi yapar.
        /// </summary>
        /// <param name="startdate">Başlangıç Tarihi</param>
        /// <param name="enddate">Bitiş Tarihi</param>
        /// <returns></returns>
        public FpuOperationResult ReadEJSummaryByDate(string startdate, string enddate)
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadEJSummaryByDate, new object[] { startdate, enddate }));
        }

        /// <summary>
        /// Parametreleri yazdırma Fonksiyonudur.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult PrintParameter()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintParameter));
            if (fpuResult.ErrorCode != 0)
            {
                //ErrorResult = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
                //return new FpuOperationResult { errorDetail = ErrorResult };
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            }
            return fpuResult;
        }

        /// <summary>
        /// reads rmp settings
        /// </summary>
        /// <returns></returns>
        public RmpSettingInfo ReadFiscalSetting()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.ReadRmpPara);
            RmpSettingInfo resultRmpInfo = new RmpSettingInfo
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.ErrorCode != 0)
            {
                return new RmpSettingInfo { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            }
            if (fpuResult.DataArray.Length > 1)
            {
                resultRmpInfo.serialNo = fpuResult.DataArray[0];
                resultRmpInfo.taxNo = fpuResult.DataArray[1];
            }
            return resultRmpInfo;
        }

        /// <summary>
        /// gets fiscal sw version
        /// </summary>
        /// <returns></returns>
        public FiscalSwInfo ReadFiscalSw()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.GetVersion);
            FiscalSwInfo resultSwInfo = new FiscalSwInfo
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message
            };
            if (fpuResult.ErrorCode != 0)
            {
                return new FiscalSwInfo { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            }
            if (fpuResult.DataArray.Length > 2)
            {
                resultSwInfo.os = fpuResult.DataArray[0];
                resultSwInfo.sdk = fpuResult.DataArray[1];
                resultSwInfo.app1 = fpuResult.DataArray[2];
                resultSwInfo.app2 = fpuResult.DataArray[3];
                resultSwInfo.ssl = fpuResult.DataArray[4];
                resultSwInfo.securityIc = fpuResult.DataArray[5];
            }
            return resultSwInfo;
        }

        /// <summary>
        /// Reads tms param
        /// </summary>
        /// <returns></returns>
        public TmsParamInfo ReadTmsParam()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.GetTMSParameter);
            TmsParamInfo result = new TmsParamInfo
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
            };
            if (fpuResult.ErrorCode != 0)
            {
                ErrorResult = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());

                return new TmsParamInfo { errorDetail = ErrorResult };
            }
            if (fpuResult.DataArray.Length > 3)
            {
                result.tmsAddress = fpuResult.DataArray[0];
                result.tmsFirstPort = fpuResult.DataArray[1];
                result.tmsSecondPort = fpuResult.DataArray[2];
            }
            return result;
        }

        /// <summary>
        ///  reads fpu Ip
        /// </summary>
        /// <param name="nType"></param>
        /// <returns></returns>
        public IpAddressInfo ReadEcrIp(string nType, bool rdIpv4, bool rdIpv6)
        {
            ExecutedCommandResult result;

            if (rdIpv4)
            {
                result = _FPU.ExecuteCommand(FpuCommands.ReadIpv4Adr, new object[] { nType });
            }
            else
            {
                result = _FPU.ExecuteCommand(FpuCommands.ReadIpv6Adr, new object[] { nType });
            }
            var xresult = new IpAddressInfo
            {
                Data = result.Data,
                ErrorMessage = result.Message,
                ErrorCode = result.ErrorCode,
            };
            if (result.ErrorCode != 0)
            {
                return new IpAddressInfo { errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };
            }
            if (result.DataArray.Length >= 2)
            {
                xresult.isDhcp = result.DataArray[0] == "0" ? false : true;
                xresult.ipAdd = result.DataArray[1];
                xresult.mask = result.DataArray[2];
                if (result.DataArray.Length > 3)
                {
                    xresult.gw = result.DataArray[3];
                    xresult.dns = result.DataArray[4];
                }
            }
            return xresult;
        }

        /// <summary>
        /// Reads network type
        /// </summary>
        /// <returns></returns>
        public NetworkType GetNetworkType()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadNetworkType));

            var result = new NetworkType
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.ErrorMessage
            };
            if (result.ErrorCode != 0)
            {
                return new NetworkType { errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };
            }
            if (result.Data.Length > 0)
            {
                result.networkType = result.Data[0];
            }
            return result;
        }

        /// <summary>
        /// Sets network type ,kullanılmıyor
        /// </summary>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public FpuOperationResult SetNetworkType(bool rdLanNetwork, bool rdGprsNetwork, bool rdPstnNetwork)
        {
            string type;
            if (rdLanNetwork) type = "L";
            else if (rdGprsNetwork) type = "G";
            else type = "P";
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetNetworkType, new object[] { type }));
        }

        /// <summary>
        /// Deletes selected device (liste gelmediği için tamamlanamadı)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FpuOperationResult DelExternalHardWareMsg(int index)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.DelExternalHardWareMsg, new object[] { 0 }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mainConnType"></param>
        /// <param name="cableConnType"></param>
        /// <param name="ecrIpAdd"></param>
        /// <param name="ecrPortAdd"></param>
        /// <returns></returns>
        public FpuOperationResult RmpExternalHardWate(int mainConnType, int cableConnType, string ecrIpAdd, string ecrPortAdd)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.RmpExternalHardWate, new object[] { mainConnType, cableConnType, ecrIpAdd, ecrPortAdd }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Returns handshaked devices count
        /// </summary>
        /// <returns></returns>
        public GetPosListResult GetExternalHardwareCount()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.GetExternalHardWareCount);
            var result = new GetPosListResult
            {
                posCount = Convert.ToInt32(fpuResult.DataArray[0])
            };

            return result;
        }

        /// <summary>
        /// Eşleşmiş Cihaz listesini getirir
        /// </summary>
        /// <returns></returns>
        public GetPosListResult GetExternalHardWareMsg(int index)
        {
            var fpuResult = (_FPU.ExecuteCommand(FpuCommands.GetExternalHardWareMsg, new object[] { index - 1 }));

            var result = new GetPosListResult
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message
            };

            if (fpuResult.DataArray != null && fpuResult.DataArray.Length >= 2)
            {
                result.serialNo = fpuResult.DataArray[0];
                result.posConsumer = fpuResult.DataArray[1];
                result.modelNo = fpuResult.DataArray[2];
                result.ecrVersion = fpuResult.DataArray[4];
            }
            return result;

            //return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.GetExternalHardWareMsg, new object[]{1} ));
        }

        /// <summary>
        /// FPU ya X raporu yazma komutunu gönderir.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult GetXReport()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintXReport));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            //return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            return fpuResult;
        }

        /// <summary>
        /// FPU ya Z raporu yazma komutu gönderir.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult GetZReport()
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintZReport));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// 24 saat geçtimi otomatik Z Raporu yazdırır.
        /// </summary>
        /// <param name="LastZDateTime"></param>
        /// <returns></returns>
        public FpuOperationResult Over24HourZRep(string LastZDateTime)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.Over24HourZRep, new object[] { LastZDateTime }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Z numarasına göre Mali Hafıza Raporu yazdırma işlemi yapar.
        /// </summary>
        /// <param name="startZNumber">Başlangıç Z numarası</param>
        /// <param name="endZNumber">Bitiş Z numarası</param>
        /// <returns></returns>
        public FpuOperationResult ReadFMDatailByNumber(int startZNumber, int endZNumber)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadFMDatailByNumber, new object[] { startZNumber, endZNumber }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Tarihe göre Mali Hafıza Raporu yazdırma işlemi yapar.
        /// </summary>
        /// <param name="startDate">Başlangıç Z numarası</param>
        /// <param name="endDate">Bitiş Z numarası</param>
        /// <returns></returns>
        public FpuOperationResult ReadFMDatailByDate(string startDate, string endDate)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadFMDatailByDate, new object[] { startDate, endDate }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Belge sonu mesajı yazdırma
        /// </summary>
        /// <param name="item">Belge sonu mesajı iki satır olarak belirlendi. 1 ve 2 olarak gönderilir.</param>
        /// <param name="message">Belge sonu satır mesajı</param>
        /// <returns></returns>
        public FpuOperationResult SettingFootMessage(int item, string message)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SettingFootMessage, new object[] { item, message }));
            if (fpuResult.ErrorCode != 0)
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            return fpuResult;
        }

        /// <summary>
        /// Fiş başlığı programlama işlemleri yapılır
        /// </summary>
        /// <param name="item">Fiş başlığı sırası</param>
        /// <param name="message">Yazdırılacak mesaj</param>
        /// <returns></returns>
        public FpuOperationResult SettingHeaderMessage(int item, string message)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SettingHeaderMessage, new object[] { item, message }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Fatura Tahsilat Belgesi.
        /// </summary>
        /// <param name="Company">Tahsilat edilen faturanın kurumu</param>
        /// <param name="Date">Tarihi</param>
        /// <param name="BillNo">Nosu</param>
        /// <param name="SubscriberNo">Abone numarası</param>
        /// <param name="InvoiceAmount"></param>
        /// <param name="CommissionAmount">Komisyon tutarı</param>
        /// <param name="Cash">Nakit ödenen</param>
        /// <param name="Credit">kredi kartıyla ödenen</param>
        /// <param name="Check">çek ile ödenen</param>
        /// <returns></returns>
        public FpuOperationResult InvoicePayment(string Company, string Date, string BillNo, string SubscriberNo, double InvoiceAmount, double CommissionAmount,
                                                  double Cash, double Credit, double Check)
        {
            if (InvoiceAmount != (Cash + Credit + Check))
            {
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation("Lütfen girdiğiniz tutarları kontrol ediniz.") };
            }
            else
            {
                var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.InvoicePayment, new object[] { Company, Date, BillNo, SubscriberNo, InvoiceAmount, CommissionAmount, Cash, Credit, Check }));
                if (fpuResult.ErrorCode != 0)
                    fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
                return fpuResult;
            }
        }

        /// <summary>
        /// Avans Bilgi Fişidir.
        /// </summary>
        /// <param name="TinOrNinNum">TC kimlik veya Vergi Numarası</param>
        /// <param name="NameTitle">Adı-Soyadı Alanı</param>
        /// <param name="AdvanceAmount">Toplam ödeme</param>
        /// <param name="Cash">Nakit</param>
        /// <param name="Credit">Kredi kartı</param>
        /// <param name="Check">çek</param>>
        /// <returns></returns>
        public FpuOperationResult AdvacePayment(string TinOrNinNum, string NameTitle, double AdvanceAmount, double Cash, double Credit, double Check)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.AdvacePayment, new object[] { TinOrNinNum, NameTitle, AdvanceAmount, Cash, Credit, Check }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Vergi numarası veya TC Kimlik Numarası Programlamak için kullanılır
        /// </summary>
        /// <param name="typeT"> T:Vergi Numarası, N:TCKimlik Numarası</param>
        /// <param name="number">Vergi Numarası 10 karakter TC kimlik numarası 11 karakter alınır.</param>
        /// <returns></returns>
        public FpuOperationResult SetTinorNinNumber(string typeT, string number)
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(statusResult.ErrorCode.ToString()) };

            if (!string.IsNullOrEmpty(number))
            {
                if (typeT == "N")
                {
                    if (number.Length == 11)
                    {
                        var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetTinorNinNumber, new object[] { typeT, number }));
                        if (result.ErrorCode != 0)
                            result.errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
                        return result;
                    }
                    else
                        return new FpuOperationResult { ErrorCode = 1231, ErrorMessage = "Lütfen TC Kimlik Numarasını 11 karakter giriniz" };
                }
                else if (typeT == "T")
                {
                    if (number.Length == 10)
                    {
                        var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetTinorNinNumber, new object[] { typeT, number }));
                        if (result.ErrorCode != 0)
                            return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };
                    }
                    else
                        return new FpuOperationResult { ErrorCode = 1231, ErrorMessage = "Lütfen Vergi Numarasını 10 karakter giriniz." };
                }
                return new FpuOperationResult { ErrorCode = 1231, ErrorMessage = "Lütfen girdiğiniz değerleri kontrol ediniz." };
            }
            else
                return new FpuOperationResult { ErrorCode = 1231, ErrorMessage = "Lütfen Girdiğiniz Değeri Kontrol Ediniz." };
        }

        /// <summary>
        /// Mali kod ve numara yı almak için kullanılır.
        /// </summary>
        /// <returns>Mali kod ve numaranın dönmesi beklenir.[0] => FiscalCode, [1] => FiscalNum</returns>
        public FiscalSerialResult GetFiscalCodeNum()
        {
            var fpuResult = (_FPU.ExecuteCommand(FpuCommands.GetFiscalCodeNum));

            var result = new FiscalSerialResult
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (result.ErrorCode != 0)
                result.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());

            if (fpuResult.DataArray != null && fpuResult.DataArray.Length >= 2)
            {
                result.FiscalCode = fpuResult.DataArray[0];
                result.FiscalNumber = fpuResult.DataArray[1];
            }
            return result;
        }

        /// <summary>
        /// Mali kod ve mali numarayı programlamak için kullanılır.
        /// </summary>
        /// <param name="fiscalCode">Mali sembolden sonra gelen firma kodu</param>
        /// <param name="fiscalNumber">8 haneli mali numara</param>
        /// <returns>Hata veya işlemin gerçekleştirmesi beklenir.</returns>
        public FpuOperationResult SetFiscalCode(string fiscalCode, string fiscalNumber)
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
                return new FpuOperationResult { errorDetail = ErrorCode.CreateError("E000", "İşlem Tamamlanamadı", "Cihazı Servis Konumuna Alınız") };

            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetFiscalCode, new object[] { fiscalCode, fiscalNumber }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        public List<FpuStatus> GetFpuStatusList()
        {
            var result = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
            return result.StatusList;
        }

        /// <summary>
        /// Smode dan çıkış kontrolü yapar
        /// S mode içerisinden çıkabilmek Listede bulunan tüm sorunların çözülmüş olması gerekir
        /// </summary>
        /// <returns>Hata listesi döner</returns>
        public List<EnumFpuStatus> GetSModeErrorStatusList()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
            return statusResult.StatusList.Where(c => (_SModeList.Contains(c.Status) && c.Flag)
                || (c.Status == EnumFpuStatus.UserMode && !c.Flag)).Select(c => c.Status).ToList();
        }

        /// <summary>
        /// S Mode dan çıkış kontrol işlemlerini yapar. Hatalardan biri çıkarsa S Mode dan çıkmasına izin vermez.
        /// </summary>
        /// <returns></returns>
        public bool SmodeExitControl()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            //bool result = statusResult.StatusList.Check(EnumFpuStatus.DataSyntaxError) || statusResult.StatusList.Check(EnumFpuStatus.CmdInvalid) ||
            //    statusResult.StatusList.Check(EnumFpuStatus.SModeOpenCap) || statusResult.StatusList.Check(EnumFpuStatus.SwitchKeyLost) ||
            //    statusResult.StatusList.Check(EnumFpuStatus.MeshKeyLost) || statusResult.StatusList.Check(EnumFpuStatus.SicNotFind) ||
            //    statusResult.StatusList.Check(EnumFpuStatus.EJNoCard) || statusResult.StatusList.Check(EnumFpuStatus.FisCodNumNotSet) ||
            //    statusResult.StatusList.Check(EnumFpuStatus.FMError) || !statusResult.StatusList.Check(EnumFpuStatus.UserMode) ||
            //    statusResult.StatusList.Check(EnumFpuStatus.EventFileErr) || statusResult.StatusList.Check(EnumFpuStatus.ErrFisAppStart) ||
            //    statusResult.StatusList.Check(EnumFpuStatus.ErrFisAppSign) || statusResult.StatusList.Check(EnumFpuStatus.ErrFisAppHash) ||
            //    statusResult.StatusList.Check(EnumFpuStatus.ErrFisAppRead) || statusResult.StatusList.Check(EnumFpuStatus.ErrPsamRead) ||
            //    statusResult.StatusList.Check(EnumFpuStatus.ErrPsamVerify) || statusResult.StatusList.Check(EnumFpuStatus.CapOpen) ||
            //    statusResult.StatusList.Check(EnumFpuStatus.FMMiss) || !statusResult.StatusList.Check(EnumFpuStatus.FiscalMode);

            sModeExitList = statusResult.StatusList.Where(c => (_SModeList.Contains(c.Status) && c.Flag)
                || ((c.Status == EnumFpuStatus.UserMode || c.Status == EnumFpuStatus.FisCodNumNotSet) && !c.Flag)).ToList();

            return sModeExitList.Count == 0;
            //return result;
        }

        /// <summary>
        /// Smodunda başlayabilir mi? kontrolü için kullanılır.
        /// </summary>
        /// <returns></returns>
        public bool CanStartSMode()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
            if (!statusResult.StatusList.Check(EnumFpuStatus.ErrFisAppSign))
            {
                if (statusResult.HasError)
                    throw new ApplicationException(statusResult.Message);
            }
            return statusResult.StatusList.Check(EnumFpuStatus.PreServiceMode) || statusResult.StatusList.Check(EnumFpuStatus.InServiceMode);
        }

        /// <summary>
        /// Ekü Sonlandırma işlemini gerçekleştirir. Öncesinde Z Raporu alınması gerekmektedir.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult CloseEJModule()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(statusResult.ErrorCode.ToString()) };
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.CloseEjModule));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// ReadsEJSummaryBy date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public FpuOperationResult ReadEJSummaryByDate(DateTime startDate, DateTime endDate)
        {
            string startDateTime = startDate.Date.Day.ToString("D2") +
                    startDate.Date.Month.ToString("D2") +
                    startDate.Date.Year.ToString().Substring(2, 2) + //DDMMYYHHMM

                    startDate.Date.Hour.ToString("D2") +
                    startDate.Date.Minute.ToString("D2");
            string EndDateTime = endDate.Date.Day.ToString("D2") +
                       endDate.Date.Month.ToString("D2") +
                       endDate.Date.Year.ToString().Substring(2, 2) +
                       endDate.Date.Hour.ToString("D2") +
                       endDate.Date.Minute.ToString("D2");
            var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadEJSummaryByDate, new object[] { startDateTime, EndDateTime }));
            if (result.ErrorCode != 0)
                result.errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
            return result;
        }

        /// <summary>
        /// Read Ekü info by giving Z and Fis Number interval
        /// </summary>
        /// <param name="firstZNo"></param>
        /// <param name="secondZNo"></param>
        /// <param name="firstFNo"></param>
        /// <param name="secondFNo"></param>
        /// <returns></returns>
        public FpuOperationResult ReadEJSummaryByZNo(int firstZNo, int secondZNo, int firstFNo, int secondFNo)
        {
            if (secondZNo < firstFNo)
            {
                return new FpuOperationResult { errorDetail = ErrorCode.CreateError("P001", "*", "Bitiş Z numarası Başlangış Z numarasından az olamaz") };
            }
            var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadEJSumaryByNumber, new object[] { firstZNo, secondZNo, firstFNo, secondZNo }));
            if (result.ErrorCode != 0)
                result.errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
            return result;
        }

        /// <summary>
        /// Prints Ej Module Report
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult EJModuleInfoReport()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
            if (statusResult.StatusList.Check(EnumFpuStatus.EJNoCard))
            {
                return new FpuOperationResult { ErrorCode = 1231, ErrorMessage = "Ekü Takılı değil" };
            }
            else
            {
                return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.EJModuleReport));
            }
        }

        /// <summary>
        /// Ekü Z Detay Raporu yazdırmak için kullanılır. Ekü yerinde değilse bu işlem gerçekleştirilemez.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult EJZInfoReport()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            if (statusResult.StatusList.Check(EnumFpuStatus.EJNoCard))
            {
                return new FpuOperationResult { ErrorCode = 1231, ErrorMessage = "Ekü takılı değil." };
            }
            else
            {
                return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.EJZInfoReport));
            }
        }

        /// <summary>
        /// FPU nun tarih saatini okumak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public DateTimeInfo ReadDateTime()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.ReadDateTime);
            DateTimeInfo result = new DateTimeInfo
            {
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
                Data = fpuResult.Data
            };
            if (fpuResult.DataArray.Length >= 1)
            {
                result.Date = fpuResult.DataArray[0];
                result.Time = fpuResult.DataArray[1];
            }

            return result; ;
        }

        /// <summary>
        /// FPU Firmware Versiyon okuma işlemini gerçekleştirir.
        /// </summary>
        /// <returns></returns>
        public FPUGetVersion GetVersion()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.GetVersion);
            var result = new FPUGetVersion
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.DataArray != null && fpuResult.DataArray.Length == 6)
            {
                result.OSVersion = fpuResult.DataArray[0].ToString();
                result.SDKVersion = fpuResult.DataArray[1].ToString();
                result.AppVersion = fpuResult.DataArray[2].ToString();
                result.App2Version = fpuResult.DataArray[3].ToString();
                result.SSLVersion = fpuResult.DataArray[4].ToString();
                result.SecurityICVersion = fpuResult.DataArray[5].ToString();
            }
            return result;
        }

        /// <summary>
        /// Cihazın farklı bir adrese ping atması için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult ConnectToBaidu()
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ConnectToBaidu));
        }

        /// <summary>
        /// Cihazın bağlantı testlerinin yapıldığı fonksiyondur.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult ConnectTest()
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ConnectTest));
        }

        /// <summary>
        /// Set mode giriş işlemi için kullanılır. 3 defa yanlış girildiği takdirde cihaz kilitlenir.
        /// </summary>
        /// <param name="password">Şifre</param>
        /// <returns></returns>
        public FpuOperationResult SetModeLogin(string password)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetModeLogin, new object[] { password }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Yazarkasanın bulunduğu mode değiştirilmek için kullanılır
        /// </summary>
        /// <param name="mode">Mode numarası</param>
        /// 0： OFF_MODE；1：REG_MODE 2：EFTPOS_MODE 3：NOUSE_MODE 4：X_MODE 5：Z_MODE
        ///6： EJ_MODE 7:FMREP_MODE 8:ADMIN_MODE 9:SERVICE_MODE 10:SET_MODE
        /// <returns></returns>
        public FpuOperationResult ChangeECRMode(int mode)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ChangeEcrMode, new object[] { mode }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Departmanları okuma görevini yapar
        /// </summary>
        /// <param name="depId"></param>
        /// <returns>Departman bilgilerini döner</returns>
        public FpuDepartment ReadDepartment(int depId)
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.ReadDepartments, new object[] { depId });
            var result = new FpuDepartment
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.DataArray != null && fpuResult.DataArray.Length == 5)
            {
                result.DepId = fpuResult.DataArray[0].AsInt();
                result.Tax = fpuResult.DataArray[1];
                result.DepLimit = fpuResult.DataArray[2].AsDouble();
                result.DepPrice = fpuResult.DataArray[3].AsDouble();
                result.DepName = fpuResult.DataArray[4];
            }

            return result;
        }

        /// <summary>
        /// Id si verilen tanımlanmış grup bilgilerini okumak için kullanılır.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>grup bilgilerini döner</returns>
        public FPUGroup ReadGroup(int groupId)
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.ReadGroup, new object[] { groupId });
            var result = new FPUGroup
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.DataArray != null && fpuResult.DataArray.Length == 3)
            {
                result.GroupId = fpuResult.DataArray[0].AsInt();
                result.DepId = fpuResult.DataArray[1].AsInt();
                result.GroupName = fpuResult.DataArray[2];
            }
            return result;
        }

        /// <summary>
        /// FPU ipv4 ayarlarını read etmek için kullanılır.
        /// </summary>
        /// <param name="networkType"> Lan PBT990 "L"</param>
        /// <returns></returns>
        public FPUReadIpV4 ReadIpV4(string networkType)
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.ReadIpv4Adr, new object[] { networkType });
            var result = new FPUReadIpV4
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.DataArray != null || fpuResult.DataArray.Length == 3)
            {
                result.DHCP = fpuResult.DataArray[0].ToString();
                result.IP = fpuResult.DataArray[1].ToString();
                result.Mask = fpuResult.DataArray[2].ToString();
                //result.Gateway = fpuResult.DataArray[3].ToString();
                //result.DNS = fpuResult.DataArray[4].ToString();
            }
            return result;
        }

        /// <summary>
        /// readsTsmParam
        /// </summary>
        /// <returns></returns>
        public TsmParamInfo ReadTsmParam()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.ReadTSMPara);
            var result = new TsmParamInfo
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.DataArray != null)
            {
                result.firstIp = fpuResult.DataArray[0].ToString();
                result.firstPort = fpuResult.DataArray[1].ToString();
                result.secondIp = fpuResult.DataArray[2].ToString();
                result.secondPort = fpuResult.DataArray[3].ToString();
            }
            return result;
        }

        /// <summary>
        /// Tsm Parametrelerini okumak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FPUReadTSMParameter ReadTSMParameter()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.ReadTSMPara);
            var result = new FPUReadTSMParameter
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.DataArray != null)
            {
                result.TSMIp = fpuResult.DataArray[0].ToString();
            }
            return result;
        }

        /// <summary>
        /// Tsm Parametrelerini okumak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult SetTSMParameter(string firstIp, string firstPort, string secondIp, string secondPort)
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetTSMPara, new object[]{firstIp,firstPort,secondIp,secondPort,String.Empty,String.Empty,String.Empty,String.Empty,
                                                                                                                           String.Empty,String.Empty,String.Empty,String.Empty}));
        }

        /// <summary>
        /// FPU'ya set edilmiş 8 KDV oranını okumak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FPUReadAllTax UploadTaxRate()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.UploadTaxRate);
            var result = new FPUReadAllTax
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.DataArray != null && fpuResult.DataArray.Length == 8)
            {
                result.TaxAmount1 = fpuResult.DataArray[0].AsDouble();
                result.TaxAmount2 = fpuResult.DataArray[1].AsDouble();
                result.TaxAmount3 = fpuResult.DataArray[2].AsDouble();
                result.TaxAmount4 = fpuResult.DataArray[3].AsDouble();
                result.TaxAmount5 = fpuResult.DataArray[4].AsDouble();
                result.TaxAmount6 = fpuResult.DataArray[5].AsDouble();
                result.TaxAmount7 = fpuResult.DataArray[6].AsDouble();
                result.TaxAmount8 = fpuResult.DataArray[7].AsDouble();
            }
            return result;
        }

        /// <summary>
        /// Son belge veya içerisinde bulunduğumuz belgenin bilgilerini döner
        /// </summary>
        /// <param name="receiptFlag">"L" Son belge veya rapor bilgilerini döner, "C" : read last receipt or report data (no” RcptType” in reply)</param>
        /// <returns>SerialNo,DDMMYYhhmm,CashierName,CashierNum,EjNo,ZNo,FisCodeNum[,RcptType]</returns>
        public ReadLastorCurrentReceiptDetail ReadLastorCurrentReceipt(string receiptFlag)
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.ReadLastorCurrentReceipt, new object[] { receiptFlag });
            var result = new ReadLastorCurrentReceiptDetail
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.DataArray != null && fpuResult.DataArray.Length <= 8)
            {
                result.ReceiptNumber = fpuResult.DataArray[0];
                result.ReceiptDateTime = fpuResult.DataArray[1];
                result.CashierNumber = fpuResult.DataArray[2];
                result.EjNumber = fpuResult.DataArray[3];
                result.ZNo = fpuResult.DataArray[4];
                result.FiscalCodeNumber = fpuResult.DataArray[5];
                result.ReceiptType = fpuResult.DataArray[6];
                //result.CashierName = string.(fpuResult.DataArray[7]);
            }
            return result;
        }

        /// <summary>
        /// Fpu mac adresini okumak için kullanılır
        /// </summary>
        /// <returns></returns>
        public ReadFpuMacAdress GetMacMessage()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.GetMacMessage);
            var result = new ReadFpuMacAdress
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.DataArray != null && fpuResult.DataArray.Length != 0)
            {
                result.FpuMacAdress = fpuResult.DataArray[0];
            }
            return result;
        }

        /// <summary>
        /// Belge içerisinde aratoplam almak için kullanılan fonksiyondur.
        /// </summary>
        /// <returns></returns>
        public double Subtotal()
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.Subtotal);
            var result = new FPUSubTotal
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.DataArray != null && fpuResult.DataArray.Length == 10)
            {
                result.SubTotal = fpuResult.DataArray[0].AsDouble();
            }

            return result.SubTotal;
        }

        /// <summary>
        /// Restoran hizmeti kullanan yerlerde adisyon açılır açılmaz Eküde iz bırakmak için kullanılacak fonksiyondur.
        /// Bu fonksiyon aracılıgıyla X ve Z raporunda gerekli alanlar güncellenir.
        /// </summary>
        /// <param name="CheckId">Adisyon id</param>
        /// <param name="CheckAmount">Adisyon Tutarı</param>
        /// <returns></returns>
        public FpuOperationResult Addrestaurantcheck(int CheckId, double CheckAmount)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.Addrestaurantcheck, new object[] { CheckId, CheckAmount }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Restoran sistemlerinde Adisyon satış fişine dönüştürüleceği zaman belge sağlıklı bir şekilde kapatılırsa beklemedeki adisyonu silebilmek için adisyon id si verilerek kullanılır.
        /// </summary>
        /// <param name="CheckId">Adisyon id</param>
        /// <returns></returns>
        public FpuOperationResult Deleterestaurantcheck(int CheckId)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.Deleterestaurantcheck, new object[] { CheckId }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// X ve Z raporuna fatura yazıcısından düzenlenecek olan Gider Pusulası bilgi girişi yapılır.
        /// </summary>
        /// <param name="Number">Düzenlenen gider pusulası adedi</param>
        /// <param name="CancelNum">İptal edilen gider pusulası adedi</param>
        /// <param name="Total">Toplam tutarı</param>
        /// <param name="Tax">KDV si</param>
        /// <param name="Cash">Nakit</param>
        /// <param name="Credit">Kredi</param>
        /// <param name="OtherPayTotal">diğer ödeme</param>
        /// <param name="FcTotal">Döviz toplam</param>
        /// <param name="FcChange">Döviz para üstü</param>
        /// <param name="CancelTotal">İptal toplam</param>
        /// <returns></returns>
        public FpuOperationResult ExpenseslipInfo(int Number, int CancelNum, double Total, double Tax, double Cash, double Credit, double OtherPayTotal,
                                                             double FcTotal, double FcChange, double CancelTotal)
        {
            OpenNonFiscalReceipt();
            PrintNonFiscalFreeText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
            PrintNonFiscalFreeText("------- GİDER PUSULASI BİLGİ FİŞİ -------");
            PrintNonFiscalFreeText(" ");
            PrintNonFiscalFreeText("NAKİT TUTAR:               " + Cash.ToString() + "  TL");
            PrintNonFiscalFreeText(" ");
            PrintNonFiscalFreeText("KREDİLİ TUTAR:            " + Credit.ToString() + "  TL");
            PrintNonFiscalFreeText(" ");
            PrintNonFiscalFreeText("ÖDENEN TOPLAM TUTAR:       " + Total.ToString() + "  TL");
            PrintNonFiscalFreeText(" ");
            PrintNonFiscalFreeText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
            PrintNonFiscalFreeText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
            PrintMessage(0);
            CloseNonFiscalReceipt();
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ExpenseslipInfo, new object[] { Number,  CancelNum,  Total,  Tax,  Cash,  Credit, OtherPayTotal,
                                                              FcTotal,  FcChange,  CancelTotal }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// X ve Z raporuna fatura yazıcısından düzenlenecek olan İrsaliye bilgi girişi yapılır.
        /// </summary>
        /// <param name="Number">Düzenlenen irsaliye adedi</param>
        /// <param name="CancelNum">İptal edilen irsaliye adedi</param>
        /// <param name="Total">Toplam tutarı</param>
        /// <param name="Tax">KDV si</param>
        /// <param name="Cash">Nakit</param>
        /// <param name="Credit">Kredi</param>
        /// <param name="OtherPayTotal">diğer ödeme</param>
        /// <param name="FcTotal">Döviz toplam</param>
        /// <param name="FcChange">Döviz para üstü</param>
        /// <param name="CancelTotal">İptal toplam</param>
        /// <returns></returns>
        public FpuOperationResult ShippingpaperInfo(int Number, int CancelNum, double Total, double Tax, double Cash, double Credit, double OtherPayTotal,
                                                             double FcTotal, double FcChange, double CancelTotal)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ShippingpaperInfo, new object[] { Number,  CancelNum,  Total,  Tax,  Cash,  Credit, OtherPayTotal,
                                                              FcTotal,  FcChange,  CancelTotal }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// X ve Z raporuna fatura yazıcısından düzenlenecek olan Tahsilat makbuzu yapılır.
        /// </summary>
        /// <param name="Number">Düzenlenen Tahsilat makbuzu adedi</param>
        /// <param name="CancelNum">İptal edilen irsaliye adedi</param>
        /// <param name="Total">Toplam tutarı</param>
        /// <param name="Tax">KDV si</param>
        /// <param name="Cash">Nakit</param>
        /// <param name="Credit">Kredi</param>
        /// <param name="OtherPayTotal">diğer ödeme</param>
        /// <param name="FcTotal">Döviz toplam</param>
        /// <param name="FcChange">Döviz para üstü</param>
        /// <param name="CancelTotal">İptal toplam</param>
        /// <returns></returns>
        public FpuOperationResult PaymentpaperInfo(int Number, int CancelNum, double Total, double Tax, double Cash, double Credit, double OtherPayTotal,
                                                             double FcTotal, double FcChange, double CancelTotal)
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PaymentpaperInfo, new object[] { Number,  CancelNum,  Total,  Tax,  Cash,  Credit, OtherPayTotal,
                                                              FcTotal,  FcChange,  CancelTotal }));
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Kasa giriş-çıkış işlemleri için kullanılır
        /// </summary>
        /// <param name="cashAmount">(-) Değer girilirse çıkış, (+) Değer girilirse giriş yapılır.</param>
        /// <returns></returns>
        public FpuOperationResult CashInCashOut(double cashAmount, string detailMessage = "")
        {
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.CashInOut, new object[] { cashAmount, detailMessage }));
            if (fpuResult.Data == "")
            {
                ErrorResult = ErrorCode.GetErrorExplanation("999");
                fpuResult.errorDetail = ErrorResult;
                fpuResult.ErrorMessage = "İşlem Yapılamadı";
                fpuResult.ErrorCode = 999;
                return fpuResult;
            }
            if (fpuResult.ErrorCode != 0)
                fpuResult.errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString());
            return fpuResult;
        }

        /// <summary>
        /// Qrcode yazdırmak için kullanılır.
        /// </summary>
        /// <param name="code">Qrcode a dönüştürülecek data.</param>
        public void PrintQrCode(string code)
        {
            _FPU.PrintQrCode(code);
        }

#if SERVICES

        /// <summary>
        /// FPU ipv4 ayaralarını set etmek için kullanılan fonksiyondur.
        /// </summary>
        /// <param name="dhcpFlag">Otomatik IP ayarlamak için kullanılır</param>
        /// <param name="NetworkType">"L->Lan","G->GPRS" "P->PSTN"</param>
        /// <param name="IPaddress"></param>
        /// <param name="mask">Alt ağ maskesi</param>
        /// <param name="gw">Gateway</param>
        /// <param name="dns"></param>
        /// <returns></returns>
        public FpuOperationResult SetIpv4Adr(int dhcpFlag, string NetworkType, string IPaddress, string mask, string gw, string dns)
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetIpv4Adr, new object[] { dhcpFlag, NetworkType, IPaddress, mask, gw, dns }));
        }

          /// <summary>
        /// Get into Fiscal Mode
        /// </summary>
        /// <param name="fiscalCode"></param>
        /// <param name="fiscalNumber"></param>
        /// <returns></returns>
        public FpuOperationResult GetInFiscalMode(string fiscalCode,string fiscalNumber)
        {
            var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetFiscalCode, new object[] { fiscalCode, fiscalNumber }));
            if (result.ErrorCode != 0)
            {
                ErrorResult=ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
               return new FpuOperationResult { errorDetail=ErrorResult };
            };
            return result;
        }

        /// <summary>
        ///  writes cumulative data
        /// </summary>
        /// <param name="fisNo"></param>
        /// <param name="cumTot"></param>
        /// <param name="cumTax"></param>
        /// <returns></returns>
        public FpuOperationResult WriteRecoveryData(string fisNo,double cumTot,double cumTax)
        {
            var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetRecoveryData, new object[] { fisNo, cumTot, cumTax }));
            if (result.ErrorCode != 0)
            {
                ErrorResult = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
                return new FpuOperationResult { errorDetail = ErrorResult };
            }
            return result;
        }

        /// <summary>
        /// tests eku or sdcard by given parameter
        /// </summary>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public FpuOperationResult TestSdCard(int cardType)
        {
            var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.TestSdCard, new object[] { cardType }));
            if (result.ErrorCode != 0)
            {
                ErrorResult = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
                return new FpuOperationResult { errorDetail = ErrorResult };
            }
            return result;
        }

            /// <summary>
        /// Makina seri numarası set etmek için kullanılır.
        /// </summary>
        /// <param name="machineSerialNo">Makina seri numarası</param>
        /// <returns></returns>
        public FpuOperationResult SetMachineSerialId(string machineSerialNo)
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetMachineSerialId, new object[] { machineSerialNo }));
        }

             /// <summary>
        /// Makina Id set etmek için kullanılan fonksiyondur.
        /// </summary>
        /// <param name="machineId">Makina id</param>
        /// <returns></returns>
        public FpuOperationResult SetMachineId(string machineId)
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetMachineId, new object[] { machineId }));
        }

        /// <summary>
        ///  changes mother board to another
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult ChangeMotherBoard()
        {
            var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.FixFmBelongToAnother));
            if (result.ErrorCode != 0)
            {
                ErrorResult = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
                return new FpuOperationResult { errorDetail = ErrorResult };
            }
            return result;
        }

        /// <summary>
        /// changes SdCard
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult ChangeSdCard()
        {
            var result= fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ChangeInterdSd));
            if (result.ErrorCode != 0)
            {
                ErrorResult = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
                return new FpuOperationResult { errorDetail = ErrorResult };
            }
            return result;
        }

        /// <summary>
        /// sets ecr active to passive
        /// </summary>
        /// <param name="setPos"></param>
        /// <returns></returns>
        public FpuOperationResult EcrActive(bool setPos)
        {
            var result= fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetEcrActive, new object[] { setPos }));
            if (result.ErrorCode != 0)
            {
                ErrorResult = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
                return new FpuOperationResult { errorDetail = ErrorResult };
            }
            return result;
        }

              /// <summary>
        /// Resets Ecr given by serial no
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public FpuOperationResult EcrReset(string serialNo)
        {
            var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ResetOperation, new object[] { serialNo }));
            if (result.ErrorCode != 0)
            {
                ErrorResult = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
                return new FpuOperationResult { errorDetail = ErrorResult };
            }
            return result;
        }

               /// <summary>
        /// updates fiscal sw
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult UpdateFiscalSw()
        {
            var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.TMSUpgrade));
            if(result.ErrorCode!=0)
                return new FpuOperationResult {errorDetail=ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };

            return result;
        }

         /// <summary>
        /// updates tms
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult UpdateTms()
        {
            var result= fpuResultMap(_FPU.ExecuteCommand(FpuCommands.TMSUpgrade));
            if(result.ErrorCode!=0)
                return new FpuOperationResult { errorDetail=ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };

            return result;
        }

        /// <summary>
        /// Sets rmpSetting
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="taxNo"></param>
        /// <returns></returns>
        public FpuOperationResult SetRmpSettings(string serialNo,string taxNo)
        {
            //Tubtiak 3.3.3 sertifika hesaplaması
            int certType=0x01 + (0x10 * 2);

            var result= fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetRmpPara, new object[] { serialNo, "", "", taxNo, "", "", "", "", "", "", "", "", "", "", certType }));
            if (result.ErrorCode != 0)
            {
                ErrorResult = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString());
                return new FpuOperationResult { errorDetail = ErrorResult };
            }
            return result;
        }

           /// <summary>
        /// sets tms param
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port1"></param>
        /// <param name="port2"></param>
        /// <returns></returns>
        public FpuOperationResult SetTmsParam(string address,string port1,string port2)
        {
            var result= fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetTMSParameter, new object[]{address,Convert.ToInt32(port1),Convert.ToInt32(port2),0,0}));
            if (result.ErrorCode != 0)
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };
            return result;
        }

         /// <summary>
        /// EKÜ Mali Hafıza ve tüm cihaz bilgilerini sıfırlar.
        /// Ardından cihaz resetlenir.
        /// Bu işlemden sonra SETappVerify Owner seçilmelidir.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult ClearAllData()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
            {
                return new FpuOperationResult { ErrorCode = 321, ErrorMessage = "Bu komut servis modunda çalıştırılabilir. " };
            }
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ClearAllData));
        }

           /// <summary>
        /// Servis konumunda FPUnun tarihini değiştirmek için kullanılabilir.
        /// Öncesinde mali bir işlem olmaması beklenir. Mali bir işlem varsa Z raporu yazdırılmalıdır.
        /// </summary>
        /// <param name="date">Tarih</param>
        /// <returns></returns>
        public FpuOperationResult SettingDate(string date)
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
            {
                return new FpuOperationResult { errorDetail=ErrorCode.CreateError("111","*","Sadece Servis Konumunda Çalıştırılabilir.") };
            }

            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SettingDate, new object[] { date}));
            if (fpuResult.ErrorCode != 0)
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            return fpuResult;
        }

        /// <summary>
        /// Servis konumunda FPU nun saatini ayarlamak için kullanılır
        /// Öncesinde mali bir işlem olmaması beklenir. Mali bir işlem varsa Z raporu yazdırılmalıdır.
        /// </summary>
        /// <param name="time">saat</param>
        /// <returns></returns>
        public FpuOperationResult SettingTime(string time)
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
            {
                return new FpuOperationResult { errorDetail =ErrorCode.CreateError("111", "*", "Sadece Servis Konumunda Çalıştırılabilir.") };
            }

            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SettingTime, new object[] { time }));
            if (fpuResult.ErrorCode != 0)
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            return fpuResult;
        }

        /// <summary>
        /// İmzalı yazılım doğrulama fonksiyonudur.
        /// </summary>
        /// <param name="singType">imza tipi 0: Owner, 1:Tübitak</param>
        /// <returns></returns>
        public FpuOperationResult AppVerifyMode(int signType)
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);
            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
            {
                return new FpuOperationResult { ErrorCode = 321, ErrorMessage = "Bu komut servis modunda çalıştırılabilir. " };
            }
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.AppVerifyMode, new object[] { signType }));
        }

        /// <summary>
        /// Firewall durumunu öğrenmek için kullanılmaktadır.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult ReadFireWall()
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ReadFireWall));
        }

        /// <summary>
        /// Firewall ayarlarını değiştirmek için kullanılır.
        /// </summary>
        /// <param name="openFlag">True ise aç false ise kapat</param>
        /// <returns></returns>
        public FpuOperationResult SetFireWall(Boolean openFlag)
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetFireWall, new object[] { openFlag }));
        }

        /// <summary>
        /// Fpu USB port testi
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult UsbHostTest()
        {
            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.UsbHostTest));
        }

           /// <summary>
        /// Servis Şifresi alır.
        /// Servis şifresiyle Servis id sini oluşturur.
        /// Oluşturulan id ile servis merkezden giriş şifresi alır.
        /// </summary>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        public FpuOperationResult GetIntoSmodeIDCode(string serviceID)
        {
            var mode = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            if (mode.StatusList.Check(EnumFpuStatus.InServiceMode))
            {
                    return new FpuOperationResult { errorDetail = ErrorCode.CreateError("E000","In Service Mode","Servis Konumunda bu Fonksiyon çalıştırılamaz.") };
            };

            var commandResult = _FPU.ExecuteCommand(FpuCommands.GetIntoSmodeIDCode, new object[] { serviceID });
            var result = new FpuOperationResult
            {
                ErrorCode = commandResult.ErrorCode,
                ErrorMessage = commandResult.Message,
            };

            if (!commandResult.HasError && (commandResult.DataArray == null || commandResult.DataArray.Length == 0))
            {
                result.ErrorCode = 1000;
                result.ErrorMessage = "Service kodu alınamadı";
            }
            else
            {
                var serviceId = commandResult.DataArray[0];
                var serviceIdStr = "";
                for (int i = 0; i < serviceId.Length; i++)
                {
                    serviceIdStr += serviceId[i];
                    if ((i + 1) % 4 == 0)
                        serviceIdStr += " ";
                }
                result.Data = serviceIdStr;
            }

            return result;
        }

        /// <summary>
        /// Servis id sine karşılık merkezden üretilen şifre ile servis konumuna girişi sağlar.
        /// </summary>
        /// <param name="verifyCode">Merkezden alınan şifre</param>
        /// <returns></returns>
        public FpuOperationResult IntoSMode(string verifyCode)
        {
            var result = _FPU.ExecuteCommand(FpuCommands.IntoSmode, new object[] { verifyCode });

            if (!result.HasError && !result.StatusList.Check(EnumFpuStatus.InServiceMode))
            {
                return new FpuOperationResult { errorDetail=ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };
            }
            else
                return fpuResultMap(result);
        }

        /// <summary>
        /// Servis id sine karşılık üretilen 16 hanelik id yi alarak Servise verilecek şifreyi üretir.
        /// </summary>
        /// <param name="SourceId">Id Code 16 Hane</param>
        /// <param name="DestId">16 hane koda karsılık 8 haneli şifre üretir</param>
        /// <returns></returns>
        public FpuOperationResult GenerateVerifyCode(string SourceId, StringBuilder DestId)
        {
            var fpuResult= fpuResultMap(_FPU.ExecuteCommand(FpuCommands.GenerateVerifyCode, new object[] { SourceId, DestId }));
            if (fpuResult.ErrorCode != 0)
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            return fpuResult;
        }

        /// <summary>
        /// Servis Id sine karşılık 16 haneli Servis Çıkış Süresi üretir.
        /// </summary>
        /// <param name="ServiceId">Servis Id</param>
        /// <returns></returns>
        public FPUGetExitModeCode GetExitSmodeIDCode(string ServiceId)
        {
            var fpuResult = _FPU.ExecuteCommand(FpuCommands.GetExitSmodeIDCode, new object[] { ServiceId });
            var result = new FPUGetExitModeCode
            {
                Data = fpuResult.Data,
                ErrorCode = fpuResult.ErrorCode,
                ErrorMessage = fpuResult.Message,
            };
            if (fpuResult.ErrorCode != 0)
                return new FPUGetExitModeCode { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            if (fpuResult.DataArray != null && fpuResult.DataArray.Length == 1)
            {
                result.GetExitMode = fpuResult.DataArray[0];
            }

            return result;
        }
        /// <summary>
        /// Smode çıkış için üretilen şifreyi alarak çıkış işlemini yapar.
        /// </summary>
        /// <param name="verifyCode">Üretilen çıkış kodu</param>
        /// <returns>Smode kontrolü sağlıklı yapıldıysa çıkış işlemi yapması beklenir.</returns>
        public FpuOperationResult ExitSMode(string verifyCode)
        {
            var errorStatusList = GetSModeErrorStatusList();
            SmodeExitControl();
            if (errorStatusList.Count != 0)
            {
                StringBuilder errorMessage = new StringBuilder();

                errorMessage.AppendLine("S Mode Status List: ");
                foreach (var item in sModeExitList)
                {
                    errorMessage.AppendLine(item.Status.ToString());
                }
                return new FpuOperationResult { ErrorCode = 102, ErrorMessage = "Lütfen Çıkış Bilgilerinizi Kontrol Ediniz." };
            }

            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.ExitSmode, new object[] { verifyCode }));
            if (fpuResult.ErrorCode != 0)
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            return fpuResult;
        }

        /// <summary>
        /// EKÜ Başlatma işlemini gerçekleştirir.
        /// </summary>
        /// <returns></returns>
        public FpuOperationResult OpenEJModule()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(statusResult.ErrorCode.ToString()) };
            var fpuResult = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.OpenEjModule));
            if (fpuResult.ErrorCode != 0)
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            return fpuResult;

            //return new FpuOperationResult { ErrorCode = 1231, ErrorMessage = "Bu komut Servis konumunda çalıştırılır" };
        }

           /// <summary>
        /// sets Ecr IP adress or Sets Dhcp
        /// </summary>
        /// <param name="ipv4"></param>
        /// <param name="ipv6"></param>
        /// <param name="dhcp"></param>
        /// <param name="ip"></param>
        /// <param name="mask"></param>
        /// <param name="gw"></param>
        /// <param name="dns"></param>
        /// <returns></returns>
        public FpuOperationResult SetEcrIP(bool ipv4, bool ipv6, bool dhcp, string ip, string mask, string gw, string dns)
        {
            if (ipv4)
            {
                var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetIpv4Adr, new object[] { Convert.ToInt32(dhcp), "L", ip, mask, gw, dns }));
                if (result.ErrorCode != 0)
                {
                    return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };
                }

                return result;
            }
            else
            {
                var result = fpuResultMap(_FPU.ExecuteCommand(FpuCommands.SetIpv4Adr, new object[] { Convert.ToInt32(dhcp), "L", ip, mask, gw, dns }));
                if (result.ErrorCode != 0)
                {
                    return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };
                }
                return result;
            }
        }

        /// <summary>
        /// Malileştirme için kullanılır.
        /// </summary>
        /// <param name="serial">mali numara</param>
        /// <returns>Mali konuma alınması beklenir.</returns>
        public FpuOperationResult Fiscalization()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
                return new FpuOperationResult { errorDetail = ErrorCode.CreateError("E000","İşlem Tamamlanamadı","Cihazı Servis Konumuna Alınız") };
            else if (statusResult.StatusList.Check(EnumFpuStatus.FiscalMode))
                return new FpuOperationResult { errorDetail = ErrorCode.CreateError("EE11", "İşlem Tamamlanamadı","Cihaz zaten Mali Modda")};
            else if (statusResult.StatusList.Check(EnumFpuStatus.TinOrNinNotSet))
                return new FpuOperationResult { errorDetail = ErrorCode.CreateError("EE11", "İşlem Tamamlanamadı", "TC no Ya da Vergi NO ayarlanmadı") };

            var fiscalCodeNumber = GetFiscalCodeNum();
            if (fiscalCodeNumber.ErrorCode != 0)
                return fiscalCodeNumber;

            if (statusResult.StatusList.Check(EnumFpuStatus.FisCodNumNotSet))
            {
                var setResult = SetFiscalCode(fiscalCodeNumber.FiscalCode, fiscalCodeNumber.FiscalNumber);
                if (setResult.ErrorCode != 0)
                    return setResult;
            }
            var serial = fiscalCodeNumber.FiscalNumber;
            var result= fpuResultMap(_FPU.ExecuteCommand(FpuCommands.Fiscalization, new object[] { serial }));
            if (result.ErrorCode != 0)
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };
            return result;
        }

        /// <summary>
        /// Kullanıcı konumuna almak için kullanılır.
        /// </summary>
        /// <param name="serial">mali numarası</param>
        /// <returns>Hata veya işlemin gerçekleştirilmesi için beklenir</returns>
        public FpuOperationResult EnterUserMode()
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
                return new FpuOperationResult { errorDetail = ErrorCode.CreateError("E000", "İşlem Tamamlanamadı", "Cihazı Servis Konumuna Alınız") };
            else if (statusResult.StatusList.Check(EnumFpuStatus.UserMode))
                return new FpuOperationResult { errorDetail = ErrorCode.CreateError("EE12", "İşlem Tamamlanamadı", "Cihaz Zaten Kullanıcı Konumunda") };

            var fiscalCodeNumber = GetFiscalCodeNum();
            if (fiscalCodeNumber.ErrorCode != 0)
                return fiscalCodeNumber;
            if (statusResult.StatusList.Check(EnumFpuStatus.FisCodNumNotSet))
            {
                var setResult = SetFiscalCode(fiscalCodeNumber.FiscalCode, fiscalCodeNumber.FiscalNumber);
                if (setResult.ErrorCode != 0)
                    return setResult;
            }
            var serial = fiscalCodeNumber.FiscalNumber;
            var result= fpuResultMap(_FPU.ExecuteCommand(FpuCommands.EnterUserMode, new object[] { serial }));
            if (result.ErrorCode != 0)
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(result.ErrorCode.ToString()) };
            return result;
        }

         /// <summary>
        /// Olay kaydı Yazdırma İşlemi Girilen değer kadar geriye giderek yazdırır.
        /// Örneğin 5 girilirse değer 5 gün öncesini Yazdırır.
        /// </summary>
        /// <param name="eventNumber">Gün 0-90 arasında değer yazdırır.</param>
        /// <returns></returns>
        public FpuOperationResult PrintEventLog(int eventNumber)
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
                return new FpuOperationResult { errorDetail=ErrorCode.GetErrorExplanation(statusResult.ErrorCode.ToString()) };

            return fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintEventLog, new object[] { eventNumber }));
        }

        /// <summary>
        /// Olay kaydı Yazdırma İşlemi Girilen değer kadar son olay kayıtlarını yazdırır.
        ///
        /// </summary>
        /// <param name="eventNumber">Girilen değer kadar olay kaydı yazdırılır.</param>
        /// <returns></returns>
        public FpuOperationResult PrintLastEventLog(int eventNumber)
        {
            var statusResult = _FPU.ExecuteCommand(FpuCommands.ReciveFPUStatus);

            if (!statusResult.StatusList.Check(EnumFpuStatus.InServiceMode))
                return new FpuOperationResult { errorDetail = ErrorCode.CreateError("E000", "İşlem Tamamlanamadı", "Cihazı Servis Konumuna Alınız") };

            var fpuResult= fpuResultMap(_FPU.ExecuteCommand(FpuCommands.PrintLastEventLog, new object[] { eventNumber }));
            if (fpuResult.ErrorCode != 0)
                return new FpuOperationResult { errorDetail = ErrorCode.GetErrorExplanation(fpuResult.ErrorCode.ToString()) };
            return fpuResult;
        }

#endif

        /// <summary>
        /// ExecutedCommandResult map işlemini yaparak dönen değerleri FpuOperationResult classına çevirir.
        /// </summary>
        /// <param name="commandResult">Sorgu sonucunda dönen değerler.</param>
        /// <returns></returns>
        private FpuOperationResult fpuResultMap(ExecutedCommandResult commandResult)
        {
            Console.WriteLine(commandResult.StatusList.Count);

            return new FpuOperationResult
            {
                ErrorCode = commandResult.ErrorCode,
                ErrorMessage = commandResult.Message,
                Data = commandResult.Data,
            };
        }
    }
}