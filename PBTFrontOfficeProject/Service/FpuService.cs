using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTFrontOfficeProject.Service
{
    /// <summary>
    /// FPU fonksiyonlarını kullanmak için oluşturulmuş Service classı
    /// </summary>
    public class FpuService
    {
        private FPUFuctions FPU
        {
            get { return FPUFuctions.Instance; }
        }

        public FpuService()
        {
            //var result= FPU.CheckOpenOperations();
            //if (result.Count>0)
            //{
            //    var msg=string.Join(Environment.NewLine,result);
            //    UIMessage.ShowErrorMessage(msg);
            //    throw new ApplicationException(msg);
            //}

        }


        /// <summary>
        /// Fpu nun açılışta satış yapabilmesi için gerekli kontrolleri yapar.
        /// Açılışta bu kontrollerin sağlanması gerekir.
        /// </summary>
        /// <returns></returns>
        public List<string> CheckOpenOperations()
        {
            return FPU.CheckOpenOperations();
        }

        public bool CheckSModeStartup()
        {
            return FPU.CheckSModeStartup();
        }

        /// <summary>
        /// Kasiyer Login işlemi için kullanılan Fonksiyondur.
        /// </summary>
        /// <param name="cashierId">Kasiyer Numarası</param>
        /// <param name="cashierPassword">Kasiyer Şifresi</param>
        /// <returns></returns>
        public string CashierLogin(int cashierId, string cashierPassword)
        {
            var result = FPU.CashierLogin(cashierId, cashierPassword);
            return checkFPUResult(result);
        }

#if SERVICES

  /// <summary>
        /// Malileştirme fonksiyonudur. Vergi numarası ve Mali kod ve numaranın set edilmesi gerekmektedir.
        /// </summary>
        /// <param name="serialNumber">Yanlızca mali numara ile gerçekleştirilir</param>
        /// <returns></returns>
        public string Fiscalization()
        {
            return checkFPUResult(FPU.Fiscalization());
        }


        /// <summary>
        /// Servis id si alarak S Mode giriş için id üretir.
        /// Üretilen id ile merkezden şifre istenir.
        /// </summary>
        /// <param name="serviceid"></param>
        /// <returns></returns>
        public string GetIntoSmodeIDCode(string serviceId)
        {
            return checkFPUResult(FPU.GetIntoSmodeIDCode(serviceId));
        }

        /// <summary>
        /// Servis id sine karşılık merkezden üretilen şifre ile servis konumuna girişi sağlar.
        /// </summary>
        /// <param name="verifyCode">Merkezden alınan şifre</param>
        /// <returns></returns>
        public bool IntoSMode(string verifyCode)
        {
            var result = FPU.IntoSMode(verifyCode);
            checkFPUResult(result);
            return result.ErrorCode == 0;
        }

        /// <summary>
        /// S Mode çıkış için üretilen şifreyi alarak çıkış kontrollerini bakarak çıkış işlemi yapar.
        /// </summary>
        /// <param name="verifyCode">S mode çıkış için üretilen şifre</param>
        /// <returns>Çıkış işlemini yapması sorun varsa hatayı dönmesi beklenir.</returns>
        public string ExitSMode(string verifyCode)
        {
            return checkFPUResult(FPU.ExitSMode(verifyCode));
        }

        /// <summary>
        /// Ekü başlatma işlemini gerçekleştirir.
        /// </summary>
        /// <returns></returns>
        public string OpenEJModule()
        {
            return checkFPUResult(FPU.OpenEJModule());
        }

         /// <summary>
        /// Girilen olay kaydı kadar geriye giderek yazdırır.(gün)
        /// Örneğin girilen değer 4 ise 4 gün öncenin olay kayıtlarını yazdırır.
        /// </summary>
        /// <param name="eventNumber">gün</param>
        /// <returns></returns>
        public string PrintEventLog(int eventNumber)
        {
            return checkFPUResult(FPU.PrintEventLog(eventNumber));
        }


        /// <summary>
        /// Girilen olay kaydı kadar olay kaydı yazdırır
        /// Örneğin girilen değer 4 ise son 4 olay kaydını yazdırır.
        /// </summary>
        /// <param name="eventNumber">adet</param>
        /// <returns></returns>
        public string PrintLastEventLog(int eventNumber)
        {
            return checkFPUResult(FPU.PrintLastEventLog(eventNumber));
        }

             /// <summary>
        /// Kullanıcı konumuna almak için kullanılır
        /// </summary>
        /// <param name="serialNumber">Yanlızca mali numara</param>
        /// <returns></returns>
        public string EnterUserMode()
        {
            return checkFPUResult(FPU.EnterUserMode());
        }

         /// <summary>
        /// FPU nun tarih saat ayarlaması için kullanılır.
        /// Öncesinde mali işlem olmaması beklenir.
        /// Eğer mali işlem yapılmışsa Z raporu alınması beklenir.
        /// </summary>
        /// <param name="date">tarih</param>
        /// <param name="time">saat</param>
        /// <returns></returns>
        public string SettingDateTime(string date)
        {
            return checkFPUResult(FPU.SettingDateTime(date));
        }


        /// <summary>
        /// EKÜ Mali Hafıza ve tüm cihaz bilgilerini sıfırlar. 
        /// Ardından cihaz resetlenir.
        /// Bu işlemden sonra SETappVerify Owner seçilmelidir.
        /// </summary>
        /// <returns></returns>
        public string ClearAllData()
        {
            return checkFPUResult(FPU.ClearAllData());
        }

        /// <summary>
        /// İmzalı yazılım doğrulama fonksiyonudur.
        /// </summary>
        /// <param name="singType">imza tipi 0: Owner, 1:Tübitak</param>
        /// <returns></returns>
        public string AppVerifyMode(int signType)
        {
            return checkFPUResult(FPU.AppVerifyMode(signType));
        }
#endif
        /// <summary>
        /// Yönetici konumuna giriş için kullanılır.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string SetModeLogin(string password)
        {
            var result = FPU.SetModeLogin(password);
            return checkFPUResult(result);
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
        public string ExpenseslipInfo(int Number, int CancelNum, double Total, double Tax, double Cash, double Credit, double OtherPayTotal,
                                                             double FcTotal, double FcChange, double CancelTotal)
        {
            var result = FPU.ExpenseslipInfo(Number, CancelNum, Total, Tax, Cash, Credit, OtherPayTotal, FcTotal, FcChange, CancelTotal);
            return checkFPUResult(result);
        }


        /// <summary>
        /// Yazarkasanın bulunduğu mode değiştirilmek için kullanılır
        /// </summary>
        /// <param name="mode">Mode numarası</param>
        /// 0： OFF_MODE；1：REG_MODE 2：EFTPOS_MODE 3：NOUSE_MODE 4：X_MODE 5：Z_MODE
        ///6： EJ_MODE 7:FMREP_MODE 8:ADMIN_MODE 9:SERVICE_MODE 10:SET_MODE
        /// <returns></returns>
        public string ChangeECRMode(int mode)
        {
            var result = FPU.ChangeECRMode(mode);
            return checkFPUResult(result);
        }


        /// <summary>
        /// Diğer ödeme tiplerini tanımlamak için kullanılır.
        /// </summary>
        /// <param name="row">Hangi sıradaki ödeme tipinin seçileceği bildirilir</param>
        /// <param name="description">Ödeme tipinin adının verildiği parametredir.</param>
        /// <returns></returns>
        public string DefAddPayName(int row, string description)
        {
            var result = FPU.DefAddPayName(row, description);
            return checkFPUResult(result);
        }

        /// <summary>
        /// Display yazmak için kullanılır
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        public void WriteToDisplay(string line1, string line2)
        {
            FPU.WriteToDisplay(line1, line2);
        }

        /// <summary>
        /// Satış işlemini başlatmak için kullanılan fonksiyondur. Mali fiş açar.
        /// </summary>
        /// <param name="clerkId">Kasiyer Id</param>
        /// <param name="InfomationSlipType">1 = InvoiceByHand, 2 = E-Invoice, 3 = E-Archive, 4 = InvoiceByPrinter</param>
        /// <param name="ClerkPsw">Kasiyer Şifresi</param>
        /// <param name="CustomerMessage">Müşteri TCkimlik veya Vergi Numarası</param>
        /// <param name="SerialSequenceNo">Elle Fatura seri sıra numarası</param>
        /// <param name="RegisterF">RegisterF : '1' = Registered Customer, '0' = Unregistered Customer(If this field is not sent, you can regarded as '0')</param>
        ///if it is not Information slip, then no need offer these 3 fields: InfomationSlipType,Customer TIN or NIN, RegisterF
        /// <returns></returns>
        public string OpenFiscalReceipt(int clerkId, int InfomationSlipType, string ClerkPsw, string CustomerMessage, string SerialSequenceNo, int RegisterF = 0)
        {
            var result = FPU.OpenFiscalReceipt(clerkId, InfomationSlipType, ClerkPsw, CustomerMessage, SerialSequenceNo, RegisterF);
            return checkFPUResult(result);
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
        public string Registeringsale(string PluName, string Flag, string PluBarcode, string FlagType, int DepId, int GroupId, int DepOrPlu, int unitPrice, int Value, int quantity)
        {
            var result = FPU.Registeringsale(PluName, Flag, PluBarcode, FlagType, DepId, GroupId, DepOrPlu, unitPrice, Value, quantity);
            return checkFPUResult(result);
        }

        /// <summary>
        /// Ödeme fonksiyonudur.
        /// </summary>
        /// <param name="modeType">Ödeme tipi belirtilir. Ödeme tipinin sırası girilir. 0 :Nakit, 1:Kredi EFT-POS suz, 2:Çek 3:-12: diğer öedeme, 13:-18: Döviz, 19:Kredi-EFTPOS</param>
        /// <param name="amount">Ödeme tutarı</param>
        /// <returns></returns>
        public string TotalCalculating(int modeType, int amount)
        {
            var result = FPU.TotalCalculating(modeType, amount);
            return checkFPUResult(result);
        }


        /// <summary>
        /// İndirim/Artırım işlemi için kullanılır.
        /// </summary>
        /// <param name="flagType">P--> -/+ % indirimi/artırımı yapar. D--> -/+ tutar indirim/artırımı yapar.</param>
        /// <param name="value">indirim/artırım tutarı girilir.(-) girilirse indirim (+) girilirse artırım yapar.</param>
        /// <returns></returns>
        public string PreAndDiscount(string flagType, int value)
        {
            var result = FPU.PreAndDiscount(flagType, value);
            return checkFPUResult(result);
        }


        /// <summary>
        /// Satış fişini kapatmak için kullanılan fonksiyondur.
        /// Ödeme alındıysa kullanılabilir. Mali simge basar.
        /// MF basılan fonksiyondur.
        /// </summary>
        /// <returns></returns>
        public string CloseFiscalReceipt()
        {
            var result = FPU.CloseFiscalReceipt();
            return checkFPUResult(result);
        }

        /// <summary>
        /// Satış fişi iptal etmek için kullanılan fonksiyondur.
        /// </summary>
        /// <returns></returns>
        public string CancelFiscalReceipt()
        {
            var result = FPU.CancelFiscalReceipt();
            return checkFPUResult(result);
        }

        /// <summary>
        /// Belge içerisinde aratoplam almak için kullanılan fonksiyondur.
        /// </summary>
        /// <returns></returns>
        public double Subtotal()
        {
            var result = FPU.Subtotal();
            return result;
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
        public string SettingTaxRate(double taxA, double taxB, double taxC, double taxD, double taxE, double taxF, double taxG, double taxH)
        {
            var result = FPU.SettingTaxRate(taxA, taxB, taxC, taxD, taxE, taxF, taxG, taxH);
            return checkFPUResult(result);
        }


        /// <summary>
        /// Eküden Z numarası ve Fiş numarası girilerek tek belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="ZReportNo">İstenen belgenin Z numarası</param>
        /// <param name="ReceiptNo">İstenen belgenin Fiş Numarası</param>
        /// <returns></returns>
        public string ReadEJSpecificReceiptByNumber(int ZReportNo, int ReceiptNo)
        {
            var result = FPU.ReadEJSpecificReceiptByNumber(ZReportNo, ReceiptNo);
            return checkFPUResult(result);
        }


        /// <summary>
        /// Tarih saat girilerek eküden belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="dateTime">Yazdırılmak istenilen tarih ve saat bilgisi</param>
        /// <returns></returns>
        public string ReadEJSpecificReceiptByDatetime(string dateTime)
        {
            var result = FPU.ReadEJSpecificReceiptByDatetime(dateTime);
            return checkFPUResult(result);
        }

        /// <summary>
        /// Tarih ve Fİş Numarası girilerek Eküden belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="currentDatetime">Eküden okunmak istenen belgenin tarihi</param>
        /// <param name="receiptNo">Eküden okunmak istenen belgenin fiş numarası</param>
        /// <returns></returns>
        public string ReadEJSpecificReceiptByDatetimeNo(string currentDatetime, int receiptNo)
        {
            var result = FPU.ReadEJSpecificReceiptByDatetimeNo(currentDatetime, receiptNo);
            return checkFPUResult(result);
        }

        /// <summary>
        /// İki fiş İki Z no arasında eküden dönemsel belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="firstZNumber">İlk Z Numarası</param>
        /// <param name="lastZNumber">Son Z Numarası</param>
        /// <param name="firstReceiptNumber">İlk Fiş Numarası</param>
        /// <param name="lastReceiptNumber">Son Fiş Numarası</param>
        /// <returns></returns>
        public string ReadEJSpecificReceiptByNumberToNumber(int firstZNumber, int lastZNumber, int firstReceiptNumber, int lastReceiptNumber)
        {
            var result = FPU.ReadEJSpecificReceiptByNumberToNumber(firstZNumber, lastZNumber, firstReceiptNumber, lastReceiptNumber);
            return checkFPUResult(result);
        }

        /// <summary>
        /// Girilen iki tarih aralığında eküden belge okuma işlemi yapar.
        /// </summary>
        /// <param name="startdate">Başlangıç Tarihi</param>
        /// <param name="enddate">Bitiş Tarihi</param>
        /// <returns></returns>
        public string ReadEJSummaryByDate(string startdate, string enddate)
        {
            var result = FPU.ReadEJSummaryByDate(startdate, enddate);
            return checkFPUResult(result);
        }

        /// <summary>
        /// X raporu yazdırma işlemi için kullanılır.
        /// </summary>
        /// <returns></returns>
        public string XReport()
        {
            var result = FPU.XReport();
            return checkFPUResult(result);
        }

        /// <summary>
        /// Z raporu yazdırma işlemi için kullanılır.
        /// </summary>
        /// <returns></returns>
        public string ZReport()
        {
            var result = FPU.ZReport();
            //if (result.ErrorCode == 0)
            //{
            //    return "3";
            //}
            //else
                return checkFPUResult(result);
            //return checkFPUResult(result);
        }

        /// <summary>
        /// Girilen değerler aralığında Mali hafıza Detay raporu yazdırma işlemi için kullanılır.
        /// </summary>
        /// <param name="startZNumber">Başlangıç Z Numarası</param>
        /// <param name="endZNumber">Bitiş Z numarası</param>
        /// <returns>Girilen değerler aralığında Z raporu yazdırması beklenir.</returns>
        public string ReadFMDatailByNumber(int startZNumber, int endZNumber)
        {
            return checkFPUResult(FPU.ReadFMDatailByNumber(startZNumber, endZNumber));
        }

        /// <summary>
        /// Dövizli ödeme tipi set etmek için kullanılan methoddur.
        /// </summary>
        /// <param name="currencyIndex">Dövizin kaydedileceği sıra 1-6 arasında değer girilir</param>
        /// <param name="currencyName">Döviz adı maksimum 10 byte olarak girilir.</param>
        /// <param name="exchangeRate">9 digit virgülden sonra 4 hane girilecek şekilde ayarlanmalıdır.</param>
        /// <param name="subsidiary">bağımlılık</param>
        /// <returns></returns>
        public string SetForeignCurrency(int currencyIndex, string currencyName, double exchangeRate, Boolean subsidiary)
        {
            return checkFPUResult(FPU.SetForeignCurrency(currencyIndex, currencyName, exchangeRate, subsidiary));
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
        public string SetDepartments(int DepId, string TaxGr, double Limit, double UnitPrice, string Depname, string IsPrintF)
        {
            return checkFPUResult(FPU.SetDepartments(DepId, TaxGr, Limit, UnitPrice, Depname, IsPrintF));
        }

        /// <summary>
        /// Group Programlama yapmak için kullanılır. Bir departmanın birden fazla grubu olabilir.
        /// </summary>
        /// <param name="GroupId">Group Id</param>
        /// <param name="DepId">Bağlı olduğu departman numarası</param>
        /// <param name="GroupName">Grup adı</param>
        /// <param name="IsPrintF"></param>
        /// <returns></returns>
        public string SetGroup(int GroupId, int DepId, string GroupName, string IsPrintF)
        {
            return checkFPUResult(FPU.SetGroup(GroupId, DepId, GroupName, IsPrintF));
        }

        /// <summary>
        /// 2 satır Belge sonu mesajını programlama işlemi için kullanılır.
        /// </summary>
        /// <param name="item">İki satır belge sonu mesajı programlanır. 1. satır için 1 değeri verilir. 2. satır için 2 değeri verilir.</param>
        /// <param name="message">Belge sonu mesajı girilir.</param>
        /// <returns>belge sonu mesajını programlanması beklenir.</returns>
        public string SettingFootMessage(int item, string message)
        {
            return checkFPUResult(FPU.SettingFootMessage(item, message));
        }


        /// <summary>
        /// Fiş başlığı mesajı programlama işlemleri için kullanılır.
        /// </summary>
        /// <param name="item">hangi sıraya yazılacağı bildirilir.</param>
        /// <param name="message">Fiş başlığı mesajı</param>
        /// <returns></returns>
        public string SettingHeaderMessage(int item, string message)
        {
            return checkFPUResult(FPU.SettingHeaderMessage(item, message));
        }

        /// <summary>
        /// Vergi numarası ve TC Kimlik Numarası Programlama işlemleri 
        /// </summary>
        /// <param name="typeT">T: Vergi Numarası  N: TC Kimlik Numarası</param>
        /// <param name="number">Vegi numarası ve TC Kimlik Numarası</param>
        /// <returns></returns>
        public string SetTinorNinNumber(string typeT, string number)
        {
            return checkFPUResult(FPU.SetTinorNinNumber(typeT, number));
        }

        /// <summary>
        /// Satır sırası girilerek kaydedilen Fiş başlığı okunur.
        /// </summary>
        /// <param name="Item">Okunacak satır sayısı</param>
        /// <returns></returns>
        public string ReadHeaderMessage(int item)
        {
            return checkFPUResult(FPU.ReadHeaderMessage(item));
        }

        /// <summary>
        /// Belge Sonu satır sayısı girilerek kaydedilen mesaj okunur.
        /// </summary>
        /// <param name="item">Okunacak satır sayısı</param>
        /// <returns></returns>
        public string ReadFootMessage(int item)
        {
            return checkFPUResult(FPU.ReadFootMessage(item));
        }

        /// <summary>
        /// Sam karttan mali kod ve mali numarayı alır
        /// </summary>
        /// <returns>Mali kod ve numaranın dönmesi beklenir.</returns>
        public FPUFiscalSerial GetFiscalCodeNum()
        {
            var result = FPU.GetFiscalCodeNum();
            checkFPUResult(result);
            return result;
        }




        /// <summary>
        /// FPU'ya set edilmiş 8 KDV oranını okumak için kullanılır. 
        /// </summary>
        /// <returns></returns>
        public FPUReadTax UploadTaxRate()
        {
            var result = FPU.UploadTaxRate();
            checkFPUResult(result);
            return result;
        }

        /// <summary>
        /// FPU da kaydedilmiş departmanları okumak için eklenmiştir.
        /// </summary>
        /// <param name="depId"></param>
        /// <returns></returns>
        public FPUReadDepartment ReadDepartments(int depId)
        {
            var result = FPU.ReadDepartments(depId);
            checkFPUResult(result);
            return result;
        }

        /// <summary>
        /// Son belge veya içerisinde bulunduğumuz belgenin bilgilerini döner
        /// </summary>
        /// <param name="receiptFlag">"L" Son belge veya rapor bilgilerini döner, "C" : read last receipt or report data (no” RcptType” in reply)</param>
        /// <returns>SerialNo,DDMMYYhhmm,CashierName,CashierNum,EjNo,ZNo,FisCodeNum[,RcptType]</returns>
        public FpuReadLastReceiptDetail ReadLastReceiptDetail(string receiptFlag)
        {
            var result = FPU.ReadLastReceiptDetail(receiptFlag);
            checkFPUResult(result);
            return result;
        }

        /// <summary>
        /// Mali kod ve mali numara set edilme işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="fiscalCode">Mali kod Mali sembolden sonra gelen firma kodu</param>
        /// <param name="fiscalNumber">Mali numara</param>
        /// <returns>İşlem onaylandı mesajının dönmesi beklenir.</returns>
        public string SetFiscalCode(string fiscalCode, string fiscalNumber)
        {
            return checkFPUResult(FPU.SetFiscalCode(fiscalCode, fiscalNumber));
        }





        /// <summary>
        /// Ekü sonlandırma işlemi için kullanılır.
        /// Öncesinde Z Raporu yazdırması beklenir.
        /// </summary>
        /// <returns></returns>
        public string CloseEjModule()
        {
            return checkFPUResult(FPU.CloseEJModule());

        }

        /// <summary>
        /// Ekü detay raporu yazdırmak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public string EJModuleReport()
        {
            return checkFPUResult(FPU.EJModuleReport());
        }


        /// <summary>
        /// Ekü Z Detay raporu yazdırma işlemi için kullanılır.
        /// Ekü yerinde değilse yazdırma işlemi gerçekleştirilemez.
        /// </summary>
        /// <returns></returns>
        public string EJZInfoReport()
        {
            return checkFPUResult(FPU.EJZInfoReport());
        }




        /// <summary>
        /// Kasa Girişi Kasa çıkış işlemleri için kullanılır.
        /// </summary>
        /// <param name="cashAmount">Kasagiriş-çıkış tutarı(-) değer kasa çıkış (+) değer kasagiriş</param>
        /// <returns></returns>
        public string CashInCashOut(double cashAmount, string detailMessage = "")
        {
            return checkFPUResult(FPU.CashInCashOut(cashAmount, detailMessage));
        }


        /// <summary>
        /// Mali bir belge içersine text yazmak için kullanılır.
        /// </summary>
        /// <param name="fiscalFreeText">Yazılmak istenen text</param>
        /// <returns></returns>
        public string PrintFreeFiscalText(string fiscalFreeText)
        {
            return checkFPUResult(FPU.PrintFreeFiscalText(fiscalFreeText));
        }

        /// <summary>
        /// Restoran hizmeti kullanan yerlerde adisyon açılır açılmaz Eküde iz bırakmak için kullanılacak fonksiyondur.
        /// Bu fonksiyon aracılıgıyla X ve Z raporunda gerekli alanlar güncellenir.
        /// </summary>
        /// <param name="CheckId">Adisyon id</param>
        /// <param name="CheckAmount">Adisyon Tutarı</param>
        /// <returns></returns>
        public string Addrestaurantcheck(int CheckId, double CheckAmount)
        {
            return checkFPUResult(FPU.Addrestaurantcheck(CheckId, CheckAmount));
        }


        /// <summary>
        /// Restoran sistemlerinde Adisyon satış fişine dönüştürüleceği zaman belge sağlıklı bir şekilde kapatılırsa beklemedeki adisyonu silebilmek için adisyon id si verilerek kullanılır.
        /// </summary>
        /// <param name="CheckId">Adisyon id</param>
        /// <returns></returns>
        public string Deleterestaurantcheck(int CheckId)
        {
            return checkFPUResult(FPU.Deleterestaurantcheck(CheckId));
        }




        private string checkFPUResult(FPUResult result)
        {
            return checkFPUResult(result, null);
        }



        /// <summary>
        /// FPU dan gelen sonucu ve mesajı döner
        /// </summary>
        /// <param name="result">FPU dan dönen sonuç</param>
        /// <param name="infoMessage">Gelen mesaj</param>
        /// <returns></returns>
        private string checkFPUResult(FPUResult result, string infoMessage)
        {
            if (result.ErrorCode != 0)
            {
                UIMessage.ShowErrorMessage(result.ErrorCode + " " + result.ErrorMessage);

                return result.ErrorCode.ToString();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(infoMessage))
                    UIMessage.ShowInfoMessage(infoMessage);
            }
            return result.Data;
        }
    }
}
