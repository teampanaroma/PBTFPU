using Panaroma.FPU;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PBTFrontOfficeProject
{
    /// <summary>
    /// FpuOperations classında tanımlanmış fonksiyonları kullanılır.
    /// </summary>
    public class FPUFuctions
    {
        private FpuOperations _FPU;// FpuOperations nesnesi tanımlanır.
        private static FPUFuctions _Instance;//Singleton Instance

        public static FPUFuctions Instance
        {
            get
            {
                return _Instance ?? (_Instance = new FPUFuctions());
            }
        }

        public static bool IsInstanceCreated
        {
            get { return _Instance != null; }
        }

        private bool printMode = true;

        //Singleton tasarım deseni
        private FPUFuctions()
        {
            string strPrintMode = ConfigurationManager.AppSettings.Get("PMode"); //fonksiyona çevrilecek.

            if (String.IsNullOrEmpty(strPrintMode))
                printMode = true;

            if (strPrintMode == "FALSE" || strPrintMode == "false" || strPrintMode == "False")
            {
                printMode = false;
            }
            else
                printMode = true;

            //Convert.ToBoolean()
            _FPU = new FpuOperations(Convert.ToInt32(ConfigurationManager.AppSettings.Get("FPUConnMode")), ConfigurationManager.AppSettings.Get("FPUPort"), printMode);
            //Açılış ve Bağlantı işlemi varsa burda yapılacak.
        }

        private static void DisposeInstance()
        {
            _Instance = null;
        }

        public void Dispose()
        {
            if (_FPU != null)
            {
                if (!_FPU.Disposed)
                    _FPU.Dispose();
                _FPU = null;
            }

            FPUFuctions.DisposeInstance();
        }

#if SERVICES//servis modunda kullanılacak fonksiyonlar

         /// <summary>
        /// Servis id sini alarak merkezden şifre alınabilecek değeri üretir.
        /// </summary>
        /// <param name="serviceId">servisid</param>
        /// <returns></returns>
        public FPUResult GetIntoSmodeIDCode(string serviceId)
        {
            return resultMap(_FPU.GetIntoSmodeIDCode(serviceId));
        }

        /// <summary>
        /// Servis id sine karşılık merkezden üretilen şifre ile servis konumuna girişi sağlar.
        /// </summary>
        /// <param name="verifyCode">Merkezden alınan şifre</param>
        /// <returns></returns>
        public FPUResult IntoSMode(string verifyCode)
        {
            return resultMap(_FPU.IntoSMode(verifyCode));
        }

        /// <summary>
        /// S mode çıkış için üretilen şifreyi alarak çıkış yapar
        /// </summary>
        /// <param name="verifyCode">S mode çıkış şifresi</param>
        /// <returns>Hata yoksa çıkış yapması beklenir</returns>
        public FPUResult ExitSMode(string verifyCode)
        {
            return resultMap(_FPU.ExitSMode(verifyCode));
        }

        /// <summary>
        /// Ekü başlatma işlemini gerçekleştirir.
        /// </summary>
        /// <returns></returns>
        public FPUResult OpenEJModule()
        {
            return resultMap(_FPU.OpenEJModule());
        }

             /// <summary>
        /// Malileştirme işlemi yapılır.
        /// </summary>
        /// <param name="serialNumber">yalnızca numara için kullanılır</param>
        /// <returns>Malileştirme işlemini gerçekleştirmesi beklenir.</returns>
        public FPUResult Fiscalization()
        {
            return resultMap(_FPU.Fiscalization());
        }

           /// <summary>
        /// FPU nun tarih saat ayarlaması için kullanılır.
        /// Öncesinde mali işlem yapılmamış olması beklenir.
        /// Eğer bir mali işlem yapılmışsa Z raporu yazdırıldıktan sonra komut çalıştırılmalıdır.
        /// </summary>
        /// <param name="date">tarih</param>
        /// <param name="time">saat</param>
        /// <returns></returns>
        public FPUResult SettingDateTime(string date)
        {
            return resultMap(_FPU.SettingDate(date));
        }

        /// <summary>
        /// İmzalı yazılım doğrulama fonksiyonudur.
        /// </summary>
        /// <param name="singType">imza tipi 0: Owner, 1:Tübitak</param>
        /// <returns></returns>
        public FPUResult AppVerifyMode(int signType)
        {
            return resultMap(_FPU.AppVerifyMode(signType));
        }

        /// <summary>
        /// EKÜ Mali Hafıza ve tüm cihaz bilgilerini sıfırlar.
        /// Ardından cihaz resetlenir.
        /// Bu işlemden sonra SETappVerify Owner seçilmelidir.
        /// </summary>
        /// <returns></returns>
        public FPUResult ClearAllData()
        {
            return resultMap(_FPU.ClearAllData());
        }

             /// <summary>
        /// Firewall durumunu öğrenmek için kullanılmaktadır.
        /// </summary>
        /// <returns></returns>
        public FPUResult ReadFireWall()
        {
            return resultMap(_FPU.ReadFireWall());
        }

        /// <summary>
        /// Firewall ayarlarını değiştirmek için kullanılır.
        /// </summary>
        /// <param name="openFlag">True ise aç false ise kapat</param>
        /// <returns></returns>
        public FPUResult SetFireWall(Boolean openFlag)
        {
            return resultMap(_FPU.SetFireWall(openFlag));
        }
        /// <summary>
        /// Makina Id set etmek için kullanılan fonksiyondur.
        /// </summary>
        /// <param name="machineId">Makina id</param>
        /// <returns></returns>
        public FPUResult SetMachineId(string machineId)
        {
            return resultMap(_FPU.SetMachineId(machineId));
        }

        /// <summary>
        /// Makina seri numarası set etmek için kullanılır.
        /// </summary>
        /// <param name="machineSerialNo">Makina seri numarası</param>
        /// <returns></returns>
        public FPUResult SetMachineSerialId(string machineSerialNo)
        {
            return resultMap(_FPU.SetMachineSerialId(machineSerialNo));
        }

             /// <summary>
        /// Kullanıcı konumuna almak için kullanılır
        /// </summary>
        /// <param name="serialNumber">yalnızca numara için kullanılır</param>
        /// <returns>Kullanıcı konumuna almak için kullanılır.</returns>
        public FPUResult EnterUserMode()
        {
            return resultMap(_FPU.EnterUserMode());
        }

          /// <summary>
        /// Girilen değer kadar geriye giderek yazdırır.
        /// Örneğin 5 yazılırsa 5 gün öncenin olay kaydını yazdırır
        /// </summary>
        /// <param name="eventNumber">değer kadar geriye gidilir. 0-90 aralığında</param>
        /// <returns></returns>
        public FPUResult PrintEventLog(int eventNumber)
        {
            return resultMap(_FPU.PrintEventLog(eventNumber));
        }

        /// <summary>
        /// Girilen değer kadar son olay kaydını yazdırır.
        /// Örneğin 5 yazılırsa son 5 olay kaydını yazdırır.
        /// </summary>
        /// <param name="eventNumber">son olay kaydı</param>
        /// <returns></returns>
        public FPUResult PrintLastEventLog(int eventNumber)
        {
            return resultMap(_FPU.PrintLastEventLog(eventNumber));
        }
#endif

        /// <summary>
        /// Servis Uygulaması Çalıştırılsın mı kontrolünü yapar.
        /// </summary>
        /// <returns></returns>
        public bool CheckSModeStartup()
        {
            if (!_FPU.CanStartSMode())
                throw new ApplicationException("Uygulama Servis Konumunda Değil");
            return true;
        }

        /// <summary>
        /// Fpu nun açılışta satış yapabilmesi için gerekli kontrolleri yapar.
        /// Açılışta bu kontrollerin sağlanması gerekir.
        /// </summary>
        /// <returns></returns>
        public List<string> CheckOpenOperations()
        {
            return _FPU.CheckOpenOperations();
        }

        /// <summary>
        /// X raporu yazdırma işlemi gerçekleştirilir.
        /// </summary>
        /// <returns>hata veya X raporu dönmesi beklenir.</returns>
        public FPUResult XReport()
        {
            return resultMap(_FPU.GetXReport());
        }

        /// <summary>
        /// Z raporu yazdırma işlemi gerçekleştirilir.
        /// </summary>
        /// <returns>Hata veya Z raporu dönmesi beklenir.</returns>
        public FPUResult ZReport()
        {
            return resultMap(_FPU.GetZReport());
        }

        /// <summary>
        /// FPU Tarih saat okumak için kullanılan fonksiyondur
        /// </summary>
        /// <returns></returns>
        public DateTimeInfo ReadDateTime()
        {
            return _FPU.ReadDateTime();
        }

        /// <summary>
        /// Mali hafıza detay raporu yazdırma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="startZNumber"> Başlangıç Z Numarası</param>
        /// <param name="endZNumber">Bitiş Z numarası</param>
        /// <returns>Girilen aralıkta Mali hafıza raporu veya hata dönmesi beklenir.</returns>
        public FPUResult ReadFMDatailByNumber(int startZNumber, int endZNumber)
        {
            return resultMap(_FPU.ReadFMDatailByNumber(startZNumber, endZNumber));
        }

        /// <summary>
        /// Tarihe göre Mali Hafıza Raporu yazdırma işlemi yapar.
        /// </summary>
        /// <param name="startDate">Başlangıç Z numarası</param>
        /// <param name="endDate">Bitiş Z numarası</param>
        /// <returns></returns>
        public FPUResult ReadFMDatailByDate(string startDate, string endDate)
        {
            return resultMap(_FPU.ReadFMDatailByDate(startDate, endDate));
        }

        /// <summary>
        /// Fiş sonu mesajı 2 satır programlama işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="item">2 satır programlanacağı için 1.satır için 1 ikinci satır için 2 değeri verilir.</param>
        /// <param name="message">Belge sonu mesajı</param>
        /// <returns>Hata veya programlama işleminin gerçekleştirilmesi beklenir.</returns>
        public FPUResult SettingFootMessage(int item, string message)
        {
            return resultMap(_FPU.SettingFootMessage(item, message));
        }

        /// <summary>
        /// Fiş Başlığı programlama işlemi için kullanılır
        /// </summary>
        /// <param name="item">Hangi sıraya yazılacağı bildirilir.</param>
        /// <param name="message">Hangi mesajı yazacağı bildirilir.</param>
        /// <returns>Hata veya fiş başlığı programlaması beklenir.</returns>
        public FPUResult SettingHeaderMessage(int item, string message)
        {
            return resultMap(_FPU.SettingHeaderMessage(item, message));
        }

        /// <summary>
        /// Vergi numarası ve TCKimlik Numarası programlama işlemleri için kullanılır.
        /// </summary>
        /// <param name="typeT">T:Vergi Numarası N:Kimlik Numarası</param>
        /// <param name="number"></param>
        /// <returns></returns>
        public FPUResult SetTinorNinNumber(string typeT, string number)
        {
            return resultMap(_FPU.SetTinorNinNumber(typeT, number));
        }

        /// <summary>
        /// Mali kod ve numarayı almak için kullanılır.
        /// </summary>
        /// <returns>Mali kod ve numara dönüşü yapar</returns>
        public FPUFiscalSerial GetFiscalCodeNum()
        {
            return resultFiscalSerialMap(_FPU.GetFiscalCodeNum());
        }

        /// <summary>
        /// FPU'ya set edilmiş 8 KDV oranını okumak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FPUReadTax UploadTaxRate()
        {
            return resultReadTaxMap(_FPU.UploadTaxRate());
        }

        /// <summary>
        /// FPU da kaydedilmiş departmanları okumak için eklenmiştir.
        /// </summary>
        /// <param name="depId"></param>
        /// <returns></returns>
        public FPUReadDepartment ReadDepartments(int depId)
        {
            return resultReadDep(_FPU.ReadDepartment(depId));
        }

        /// <summary>
        /// Mali kod ve numara programlamak için kullanılır.
        /// </summary>
        /// <param name="fiscalCode">Firma kodu</param>
        /// <param name="fiscalNumber">Mali numara</param>
        /// <returns>Hata veya Programlandı mesajı döner</returns>
        public FPUResult SetFiscalCode(string fiscalCode, string fiscalNumber)
        {
            return resultMap(_FPU.SetFiscalCode(fiscalCode, fiscalNumber));
        }

        /// <summary>
        /// Ekü Sonlandırma işlemini gerçekleştirir. Öncesinde Z Raporu alınması gerekmektedir.
        ///         /// </summary>
        /// <returns></returns>
        public FPUResult CloseEJModule()
        {
            return resultMap(_FPU.CloseEJModule());
        }

        /// <summary>
        /// Ekü detay raporu yazdırmak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FPUResult EJModuleReport()
        {
            return resultMap(_FPU.EJModuleReport());
        }

        /// <summary>
        /// Ekü Z Detay Raporu yazdırmak için kullanılır. Ekü yerinde değilse hata mesajı döner.
        /// </summary>
        /// <returns></returns>
        public FPUResult EJZInfoReport()
        {
            return resultMap(_FPU.EJZInfoReport());
        }

        /// <summary>
        /// Kasa giriş-çıkış işlemleri için kullanılır
        /// </summary>
        /// <param name="cashAmount">(-) Değer girilirse çıkış, (+) Değer girilirse giriş yapılır.</param>
        /// <returns></returns>
        public FPUResult CashInCashOut(double cashAmount, string detailMessage = "")
        {
            return resultMap(_FPU.CashInCashOut(cashAmount, detailMessage));
        }

        /// <summary>
        /// Cihaz Parametreleri yazdırma Fonksiyonudur.
        /// </summary>
        /// <returns></returns>
        public FPUResult PrintParameter()
        {
            return resultMap(_FPU.PrintParameter());
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
        public FPUResult InvoicePayment(string Company, string Date, string BillNo, string SubscriberNo, double InvoiceAmount, double CommissionAmount,
                                                  double Cash, double Credit, double Check)
        {
            return resultMap(_FPU.InvoicePayment(Company, Date, BillNo, SubscriberNo, InvoiceAmount, CommissionAmount, Cash, Credit, Check));
        }

        /// <summary>
        /// Avans Bilgi Fişidir.
        /// </summary>
        /// <param name="TinOrNinNum">TC kimlik veya Vergi Numarası</param>
        /// <param name="NameTitle">Adı-Soyadı Alanı</param>
        /// <param name="AdvanceAmount">Toplam ödeme</param>
        /// <param name="Cash">Nakit</param>
        /// <param name="Credit">Kredi kartı</param>
        /// <param name="Check">çek</param>
        /// <returns></returns>
        public FPUResult AdvacePayment(string TinOrNinNum, string NameTitle, double AdvanceAmount, double Cash, double Credit, double Check)
        {
            return resultMap(_FPU.AdvacePayment(TinOrNinNum, NameTitle, AdvanceAmount, Cash, Credit, Check));
        }

        /// <summary>
        /// Müşteri ekranına yazmak için kullanılır.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        public void WriteToDisplay(string line1, string line2)
        {

            // Müşteri ekranında taşma kontrolü yapılmıştır.

            if (line1.Length > 20 && line2.Length > 20)  //  Line 1 ve line 2 fazla girilmişse 20 karakter sınırlaması yapacak
            {
                _FPU.WriteToDisplay(line1.Substring(0, 20), line2.Substring(0, 20));
            }

            else if (line1.Length > 20) // sadece line 1 de karakter sınırlaması
            {
                _FPU.WriteToDisplay(line1.Substring(0, 20), line2);
            }
            else if (line2.Length > 20) //sadece line 2 de karakter sınırlaması
            {
                _FPU.WriteToDisplay(line1, line2.Substring(0, 20));
            }

            else
            {
                _FPU.WriteToDisplay(line1, line2); // müşteri ekranında taşma yok ise olduğu gibi yazacak.
            }
        }

        /// <summary>
        /// Para çekmecesi açmak için kullanılan fonksiyondur.
        /// </summary>
        public void OpenDrawer()
        {
            _FPU.OpenDrawer();
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
        public FPUResult OpenFiscalReceipt(int clerkId, int InfomationSlipType, string ClerkPsw, string CustomerMessage, string SerialSequenceNo, int RegisterF = 0)
        {
            return resultMap(_FPU.OpenFiscalReceipt(clerkId, InfomationSlipType, ClerkPsw, CustomerMessage, SerialSequenceNo, RegisterF));
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
        public FPUResult Registeringsale(string PluName, string Flag, string PluBarcode, string FlagType, int DepId, int GroupId, int DepOrPlu, decimal unitPrice, decimal Value, decimal quantity)
        {
            return resultMap(_FPU.Registeringsale(PluName, Flag, PluBarcode, FlagType, DepId, GroupId, DepOrPlu, unitPrice, Value, quantity));
        }

        /// <summary>
        /// Kasiyer Login işlemi için kullanılan Fonksiyondur.
        /// </summary>
        /// <param name="cashierId">Kasiyer Numarası</param>
        /// <param name="cashierPassword">Kasiyer Şifresi</param>
        /// <returns></returns>
        public FPUResult CashierLogin(int cashierId, string cashierPassword)
        {
            return resultMap(_FPU.CashierLogin(cashierId, cashierPassword));
        }

        /// <summary>
        /// Kasiyer Login işlemi için kullanılan Fonksiyondur.
        /// </summary>
        /// <param name="cashierId">Kasiyer Numarası</param>
        /// <param name="cashierPassword">Kasiyer Şifresi</param>
        /// <returns></returns>
        public FPUResult SetModeLogin(string password)
        {
            return resultMap(_FPU.SetModeLogin(password));
        }

        /// <summary>
        ///  /// <summary>
        /// Yazarkasanın bulunduğu mode değiştirilmek için kullanılır
        /// </summary>
        /// <param name="mode">Mode numarası</param>
        /// 0： OFF_MODE；1：REG_MODE 2：EFTPOS_MODE 3：NOUSE_MODE 4：X_MODE 5：Z_MODE
        ///6： EJ_MODE 7:FMREP_MODE 8:ADMIN_MODE 9:SERVICE_MODE 10:SET_MODE
        public FPUResult ChangeECRMode(int mode)
        {
            return resultMap(_FPU.ChangeECRMode(mode));
        }

        /// <summary>
        /// Satır sırası girilerek kaydedilen Fiş başlığı okunur.
        /// </summary>
        /// <param name="Item">Okunacak satır sayısı</param>
        /// <returns></returns>
        public FPUResult ReadHeaderMessage(int item)
        {
            return resultMap(_FPU.ReadHeaderMessage(item));
        }

        /// <summary>
        /// Belge Sonu satır sayısı girilerek kaydedilen mesaj okunur.
        /// </summary>
        /// <param name="item">Okunacak satır sayısı</param>
        /// <returns></returns>
        public FPUResult ReadFootMessage(int item)
        {
            return resultMap(_FPU.ReadFootMessage(item));
        }

        /// <summary>
        /// Dövizli ödeme tipi set etmek için kullanılan methoddur.
        /// </summary>
        /// <param name="currencyIndex">Dövizin kaydedileceği sıra 1-6 arasında değer girilir</param>
        /// <param name="currencyName">Döviz adı maksimum 10 byte olarak girilir.</param>
        /// <param name="exchangeRate">9 digit virgülden sonra 4 hane girilecek şekilde ayarlanmalıdır.</param>
        /// <param name="subsidiary">bağımlılık</param>
        /// <returns></returns>
        public FPUResult SetForeignCurrency(int currencyIndex, string currencyName, double exchangeRate, Boolean subsidiary)
        {
            return resultMap(_FPU.SetForeignCurrency(currencyIndex, currencyName, exchangeRate, subsidiary));
        }

        /// <summary>
        /// Diğer ödeme tiplerini tanımlamak için kullanılır.
        /// </summary>
        /// <param name="row">Hangi sıradaki ödeme tipinin seçileceği bildirilir</param>
        /// <param name="description">Ödeme tipinin adının verildiği parametredir.</param>
        /// <returns></returns>
        public FPUResult DefAddPayName(int row, string description)
        {
            return resultMap(_FPU.DefAddPayName(row, description));
        }

        /// <summary>
        /// 16 haneli Mersis numarasını sisteme set etmek için kullanılan alandır.
        /// </summary>
        /// <param name="mersisNo">16 haneli mersis numarası</param>
        /// <returns></returns>
        public FPUResult SettingMersisNo(string mersisNo)
        {
            return resultMap(_FPU.SettingMersisNo(mersisNo));
        }

        /// <summary>
        /// Kayıtlı mersis numarası varsa okumak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FPUResult ReadMersisNo()
        {
            return resultMap(_FPU.ReadMersisNo());
        }

        /// <summary>
        /// Maksimum satış fişi limitini belirler. Belirlenen limitin aşılmasına izin verilmez.
        /// Örneğin, bu limit aşıldığı zaman kullanıcının faturaya geçmesi istenebilir.
        /// </summary>
        /// <param name="priceLimit">Maksimum fiş satış limiti</param>
        /// <returns></returns>
        public FPUResult SetPriceLimit(double priceLimit)
        {
            return resultMap(_FPU.SetPriceLimit(priceLimit));
        }

        /// <summary>
        /// Mali bir belge içersine text yazmak için kullanılır.
        /// </summary>
        /// <param name="fiscalFreeText">Yazılmak istenen text</param>
        /// <returns></returns>
        public FPUResult PrintFreeFiscalText(string fiscalFreeText)
        {
            return resultMap(_FPU.PrintFreeFiscalText(fiscalFreeText));
        }

        /// <summary>
        /// Mali olmayan belge içerisine text yazmak için kullanılır.
        /// </summary>
        /// <param name="nonFreemessage">Yazılmak istenen mesaj</param>
        /// <returns></returns>
        public FPUResult PrintNonFiscalFreeText(string nonFreemessage)
        {
            return resultMap(_FPU.PrintNonFiscalFreeText(nonFreemessage));
        }

        /// <summary>
        /// Kaydedilmiş Seri numarasını okumak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FPUResult ReadMachineSerialId()
        {
            return resultMap(_FPU.ReadMachineSerialId());
        }

        /// <summary>
        /// Mali Olmayan Belge Başlatma işlemini yapar.
        /// </summary>
        /// <returns></returns>
        public FPUResult OpenNonFiscalReceipt()
        {
            return resultMap(_FPU.OpenNonFiscalReceipt());
        }

        /// <summary>
        /// Mali olmayan belge kapatma işlemini yapar.
        /// </summary>
        /// <returns></returns>
        public FPUResult CloseNonFiscalReceipt()
        {
            return resultMap(_FPU.CloseNonFiscalReceipt());
        }

        /// <summary>
        /// Muayene Ücreti eklemek için kullanılır
        /// Ödemeden önce gönderilmesi gerekmektedir.
        /// </summary>
        /// <param name="Price">Muayene ücreti</param>
        /// <returns></returns>
        public FPUResult CheckUpPayment(double Price)
        {
            return resultMap(_FPU.checkuppayment(Price));
        }

        /// <summary>
        /// Reçete katkı payı  eklemek için kullanılır
        /// Ödemeden önce gönderilmesi gerekmektedir.
        /// </summary>
        /// <param name="Price">Reçete Katkı Payı</param>
        /// <returns></returns>
        public FPUResult AddPresOntribution(double Price)
        {
            return resultMap(_FPU.presaddedvalue(Price));
        }

        /// <summary>
        /// Fatura dışındaki belgelerin X ve Z raporlarındaki alanlarını doldurmak için oluşturulmuştur.
        /// </summary>
        /// <param name="ExpeNumber"></param>
        /// <param name="ExpeCancelNum"></param>
        /// <param name="ExpeTotal"></param>
        /// <param name="ExpeTax"></param>
        /// <param name="ExpeCash"></param>
        /// <param name="ExpeCredit"></param>
        /// <param name="ExpeOtherPayTotal"></param>
        /// <param name="ExpeFcTotal"></param>
        /// <param name="ExpeFcChange"></param>
        /// <param name="ExpeCancelTotal"></param>
        /// <param name="ShipNumber"></param>
        /// <param name="ShipCancelNum"></param>
        /// <param name="ShipTotal"></param>
        /// <param name="ShipTax"></param>
        /// <param name="ShipCash"></param>
        /// <param name="ShipCredit"></param>
        /// <param name="ShipOtherPayTotal"></param>
        /// <param name="ShipFcTotal"></param>
        /// <param name="ShipFcChange"></param>
        /// <param name="ShipCancelTotal"></param>
        /// <param name="PayNumber"></param>
        /// <param name="PayCancelNum"></param>
        /// <param name="PayTotal"></param>
        /// <param name="PayTax"></param>
        /// <param name="PayCash"></param>
        /// <param name="PayCredit"></param>
        /// <param name="PayOtherPayTotal"></param>
        /// <param name="PayFcTotal"></param>
        /// <param name="PayFcChange"></param>
        /// <param name="PayCancelTotal"></param>
        /// <param name="ExcNumber"></param>
        /// <param name="ExcCancelNum"></param>
        /// <returns></returns>
        public FPUResult SetReportOtherInformation(int ExpeNumber, int ExpeCancelNum, double ExpeTotal, double ExpeTax, double ExpeCash, double ExpeCredit, double ExpeOtherPayTotal,
                                                             double ExpeFcTotal, double ExpeFcChange, double ExpeCancelTotal,
                                                             int ShipNumber, int ShipCancelNum, double ShipTotal, double ShipTax, double ShipCash, double ShipCredit, double ShipOtherPayTotal,
                                                             double ShipFcTotal, double ShipFcChange, double ShipCancelTotal,
                                                             int PayNumber, int PayCancelNum, double PayTotal, double PayTax, double PayCash, double PayCredit, double PayOtherPayTotal,
                                                             double PayFcTotal, double PayFcChange, double PayCancelTotal,
                                                             int ExcNumber, int ExcCancelNum)
        {
            //return resultMap(_FPU.SetReportOtherInformation(ExpeNumber, ExpeCancelNum, ExpeTotal, ExpeTax, ExpeCash, ExpeCredit, ExpeOtherPayTotal,
            //                                                 ExpeFcTotal, ExpeFcChange, ExpeCancelTotal,
            //                                                 ShipNumber, ShipCancelNum, ShipTotal, ShipTax, ShipCash, ShipCredit, ShipOtherPayTotal,
            //                                                  ShipFcTotal, ShipFcChange, ShipCancelTotal,
            //                                                  PayNumber, PayCancelNum, PayTotal, PayTax, PayCash, PayCredit, PayOtherPayTotal,
            //                                                  PayFcTotal, PayFcChange, PayCancelTotal,
            //                                                  ExcNumber, ExcCancelNum));
            return null;
        }

        /// <summary>
        /// Kasiyer Adını değiştirmek için kullanılır.
        /// </summary>
        /// <param name="clerkId">Kasiyer id</param>
        /// <param name="clerkName">Kasiyer Adı</param>
        /// <returns></returns>
        public FPUResult SetClerkName(int clerkId, string clerkName)
        {
            return resultMap(_FPU.SetClerkName(clerkId, clerkName));
        }

        /// <summary>
        /// Kasiyer şifresi set etmek için kullanılır.
        /// </summary>
        /// <param name="ClerkId">Kasiyer id</param>
        /// <param name="newPassword">Şifresi</param>
        /// <param name="ClerkName">Kasiyer Adı</param>
        /// <returns></returns>
        public FPUResult SetClerkPswd(int ClerkId, string newPassword, string ClerkName)
        {
            return resultMap(_FPU.SetClerkPswd(ClerkId, newPassword, ClerkName));
        }

        /// <summary>
        /// Askıya belge kaydetmek için kullanılır.
        /// </summary>
        /// <param name="fiscalReceiptNo">Askıya alınacak fiş numarası.</param>
        /// <returns></returns>
        public FPUResult SaveFiscalReceipt(int fiscalReceiptNo)
        {
            return resultMap(_FPU.SaveFiscalReceipt(fiscalReceiptNo));
        }

        /// <summary>
        /// Askıya alınmış bir belgeyi geri çağırmak için kullanılır.
        /// </summary>
        /// <param name="fiscalReceiptNo">Fiş numarası</param>
        /// <param name="total">Toplam tutarı</param>
        /// <param name="totTax">Toplam Kdv tutarı</param>
        /// <returns></returns>
        public FPUResult PrintRecallinfomation(int fiscalReceiptNo, double total, double totTax)
        {
            return resultMap(_FPU.PrintRecallinfomation(fiscalReceiptNo, total, totTax));
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
        public FPUResult SettingTaxRate(double taxA, double taxB, double taxC, double taxD, double taxE, double taxF, double taxG, double taxH)
        {
            return resultMap(_FPU.SettingTaxRate(taxA, taxB, taxC, taxD, taxE, taxF, taxG, taxH));
        }

        /// <summary>
        /// İndirim/Artırım işlemi için kullanılır.
        /// </summary>
        /// <param name="flagType">P--> -/+ % indirimi/artırımı yapar. D--> -/+ tutar indirim/artırımı yapar.</param>
        /// <param name="value">indirim/artırım tutarı girilir.(-) girilirse indirim (+) girilirse artırım yapar.</param>
        /// <returns></returns>
        public FPUResult PreAndDiscount(string flagType, decimal value)
        {
            return resultMap(_FPU.PreAndDiscount(flagType, value));
        }

        /// <summary>
        /// Sıfır tutarlı belge özelliğini açar.
        /// </summary>
        /// <param name="type">1 olursa bu özellik açılır.</param>
        /// <returns></returns>
        public FPUResult FinalizingreceiptSwitch(int type)
        {
            return resultMap(_FPU.FinalizingreceiptSwitch(type));
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
        public FPUResult SetDepartments(int DepId, string TaxGr, double Limit, double UnitPrice, string Depname, string IsPrintF)
        {
            return resultMap(_FPU.SetDepartments(DepId, TaxGr, Limit, UnitPrice, Depname, IsPrintF));
        }

        /// <summary>
        /// Group Programlama yapmak için kullanılır. Bir departmanın birden fazla grubu olabilir.
        /// </summary>
        /// <param name="GroupId">Group Id</param>
        /// <param name="DepId">Bağlı olduğu departman numarası</param>
        /// <param name="GroupName">Grup adı</param>
        /// <param name="IsPrintF"></param>
        /// <returns></returns>
        public FPUResult SetGroup(int GroupId, int DepId, string GroupName, string IsPrintF)
        {
            return resultMap(_FPU.SetGroup(GroupId, DepId, GroupName, IsPrintF));
        }

        /// <summary>
        /// Kaydedilmiş dövizleri okur. Indeksi verilerek döviz bilgileri okunur.
        /// </summary>
        /// <param name="currencyIndex">Döviz index</param>
        /// <returns></returns>
        public FPUResult ReadForeignCurrency(int currencyIndex)
        {
            return resultMap(_FPU.ReadForeignCurrency(currencyIndex));
        }

        /// <summary>
        /// Sırasını girerek ödeme isimlerini okumak için kullanılır.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public FPUResult ReadPayNmae(int option)
        {
            return resultMap(_FPU.ReadPayNmae(option));
        }

        /// <summary>
        /// Kasa nakit durumunu sorgulamak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FPUGetCashInOutDrawer GetCashInDrawer()
        {
            return resultCashInOutDrawer(_FPU.GetCashInDrawer());
        }

        /// <summary>
        /// Id si verilen tanımlanmış grup bilgilerini okumak için kullanılır.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public FPUResult ReadGroup(int groupId)
        {
            return resultMap(_FPU.ReadGroup(groupId));
        }

        /// <summary>
        /// Satış fişini kapatmak için kullanılan fonksiyondur.
        /// Ödeme alındıysa kullanılabilir. Mali simge basar.
        /// MF basılan fonksiyondur.
        /// </summary>
        /// <returns></returns>
        public FPUResult CloseFiscalReceipt()
        {
            return resultMap(_FPU.CloseFiscalReceipt());
        }

        /// <summary>
        /// Satış fişi iptal etmek için kullanılan fonksiyondur.
        /// </summary>
        /// <returns></returns>
        public FPUResult CancelFiscalReceipt()
        {
            return resultMap(_FPU.CancelFiscalReceipt());
        }

        /// <summary>
        /// Acil durum iptal fonksiyonudur./Ödeme varsa dahi belgeyi iptal eder.
        /// </summary>
        /// <returns></returns>
        public FPUResult SpecilCancelFiscalReceipt()
        {
            return resultMap(_FPU.SpecilCancelFiscalReceipt());
        }

        /// <summary>
        /// Belge içerisinde aratoplam almak için kullanılan fonksiyondur.
        /// </summary>
        /// <returns></returns>
        public double Subtotal()
        {
            return _FPU.Subtotal();
        }

        /// <summary>
        /// Fpu nun anlık statülerini öğrenmek için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FPUResult ReciveFPUStatus()
        {
            return resultMap(_FPU.ReciveFPUStatus());
        }

        /// <summary>
        /// Son mali işlem kaydın bilgilerini veren fonsksiyondur.
        /// </summary>
        /// <param name="receiptFlag">"" Boş geçilirse Z raporu bilgilerini "R" son mali fiş bilgilerini verir.</param>
        /// <returns></returns>
        public FPUResult ReadLastFiscal(string receiptFlag)
        {
            return resultMap(_FPU.ReadLastFiscal(receiptFlag));
        }

        /// <summary>
        /// Son belge veya içerisinde bulunduğumuz belgenin bilgilerini döner
        /// </summary>
        /// <param name="receiptFlag">"L" Son belge veya rapor bilgilerini döner, "C" : read last receipt or report data (no” RcptType” in reply)</param>
        /// <returns>SerialNo,DDMMYYhhmm,CashierName,CashierNum,EjNo,ZNo,FisCodeNum[,RcptType]</returns>
        public FPUResult ReadLastorCurrentReceipt(string receiptFlag)
        {
            return resultMap(_FPU.ReadLastorCurrentReceipt(receiptFlag));
        }

        /// <summary>
        /// Fpu nun anlık hangi modda olduğunu öğrenmek için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FPUResult GetFpuCurrentMode()
        {
            return resultMap(_FPU.GetFpuCurrentMode());
        }

        /// <summary>
        /// Bu ne işi yarıyor öğren ????
        /// </summary>
        /// <returns></returns>
        public FPUResult GetFreeNumber()
        {
            return resultMap(_FPU.GetFreeNumber());
        }

        /// <summary>
        /// Ödeme fonksiyonudur.
        /// </summary>
        /// <param name="modeType">Ödeme tipi belirtilir. Ödeme tipinin sırası girilir. 0 :Nakit, 1:Kredi EFT-POS suz, 2:Çek 3:-12: diğer öedeme, 13:-18: Döviz, 19:Kredi-EFTPOS</param>
        /// <param name="amount">Ödeme tutarı</param>
        /// <returns></returns>
        public FPUResult TotalCalculating(int modeType, decimal amount)
        {
            return resultMap(_FPU.TotalCalculating(modeType, amount));
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
        public FPUResult Printinvoicebyhand(string SerialNo, string InvoiceNo, double Total, double Cash, double Credit, double Check,
                                                         string FcName1, string FcName2, string FcName3, string FcName4, string FcName5, string FcName6,
                                                         double FcAmt1, double FcAmt2, double FcAmt3, double FcAmt4, double FcAmt5, double FcAmt6,
                                                         double FcLocal1, double FcLocal2, double FcLocal3, double FcLocal4, double FcLocal5, double FcLocal6, string InvoiceDate)
        {
            return resultMap(_FPU.Printinvoicebyhand(SerialNo, InvoiceNo, Total, Cash, Credit, Check,
                                                          FcName1, FcName2, FcName3, FcName4, FcName5, FcName6,
                                                          FcAmt1, FcAmt2, FcAmt3, FcAmt4, FcAmt5, FcAmt6,
                                                          FcLocal1, FcLocal2, FcLocal3, FcLocal4, FcLocal5, FcLocal6, InvoiceDate));
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
        public FPUResult PrintDataInRec(string InvCustomerStr0, string InvCustomerStr1, string InvCustomerStr2, string InvCustomerStr3, string InvCustomerStr4, string SerialNo, string InvoiceNum)
        {
            return resultMap(_FPU.PrintDataInRec(InvCustomerStr0, InvCustomerStr1, InvCustomerStr2, InvCustomerStr3, InvCustomerStr4, SerialNo, InvoiceNum));
        }

        /// <summary>
        /// SOR????
        /// </summary>
        /// <param name="repTypeIndex"></param>
        /// <returns></returns>
        public FPUResult PrintMessage(int repTypeIndex)
        {
            return resultMap(_FPU.PrintMessage(repTypeIndex));
        }

        /// <summary>
        /// Eküden Z numarası ve Fiş numarası girilerek tek belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="ZReportNo">İstenen belgenin Z numarası</param>
        /// <param name="ReceiptNo">İstenen belgenin Fiş Numarası</param>
        /// <returns></returns>
        public FPUResult ReadEJSpecificReceiptByNumber(int ZReportNo, int ReceiptNo)
        {
            return resultMap(_FPU.ReadEJSpecificReceiptByNumber(ZReportNo, ReceiptNo));
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
        public FPUResult ExpenseslipInfo(int Number, int CancelNum, double Total, double Tax, double Cash, double Credit, double OtherPayTotal,
                                                             double FcTotal, double FcChange, double CancelTotal)
        {
            return resultMap(_FPU.ExpenseslipInfo(Number, CancelNum, Total, Tax, Cash, Credit, OtherPayTotal, FcTotal, FcChange, CancelTotal));
        }

        /// <summary>
        /// Tarih saat girilerek eküden belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="dateTime">Yazdırılmak istenilen tarih ve saat bilgisi</param>
        /// <returns></returns>
        public FPUResult ReadEJSpecificReceiptByDatetime(string dateTime)
        {
            return resultMap(_FPU.ReadEJSpecificReceiptByDatetime(dateTime));
        }

        /// <summary>
        /// Tarih ve Fİş Numarası girilerek Eküden belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="currentDatetime">Eküden okunmak istenen belgenin tarihi</param>
        /// <param name="receiptNo">Eküden okunmak istenen belgenin fiş numarası</param>
        /// <returns></returns>
        public FPUResult ReadEJSpecificReceiptByDatetimeNo(string currentDatetime, int receiptNo)
        {
            return resultMap(_FPU.ReadEJSpecificReceiptByDatetimeNo(currentDatetime, receiptNo));
        }

        /// <summary>
        /// İki fiş İki Z no arasında eküden dönemsel belge okuma işlemi gerçekleştirilir.
        /// </summary>
        /// <param name="firstZNumber">İlk Z Numarası</param>
        /// <param name="lastZNumber">Son Z Numarası</param>
        /// <param name="firstReceiptNumber">İlk Fiş Numarası</param>
        /// <param name="lastReceiptNumber">Son Fiş Numarası</param>
        /// <returns></returns>
        public FPUResult ReadEJSpecificReceiptByNumberToNumber(int firstZNumber, int lastZNumber, int firstReceiptNumber, int lastReceiptNumber)
        {
            return resultMap(_FPU.ReadEJSpecificReceiptByNumberToNumber(firstZNumber, lastZNumber, firstReceiptNumber, lastReceiptNumber));
        }

        /// <summary>
        /// Girilen iki tarih aralığında eküden belge okuma işlemi yapar.
        /// </summary>
        /// <param name="startdate">Başlangıç Tarihi</param>
        /// <param name="enddate">Bitiş Tarihi</param>
        /// <returns></returns>
        public FPUResult ReadEJSummaryByDate(string startdate, string enddate)
        {
            return resultMap(_FPU.ReadEJSummaryByDate(startdate, enddate));
        }

        /// <summary>
        /// FPU Firmware Versiyon okuma işlemini gerçekleştirir.
        /// </summary>
        /// <returns></returns>
        public FPUReadVersion GetVersion()
        {
            return resultReadVersion(_FPU.GetVersion());
        }

        /// <summary>
        /// Fpu mac adresini okumak için kullanılır
        /// </summary>
        /// <returns></returns>
        public FPUMac GetMacMessage()
        {
            return resultFpuMac(_FPU.GetMacMessage());
        }

        /// <summary>
        /// Son belge veya içerisinde bulunduğumuz belgenin bilgilerini döner
        /// </summary>
        /// <param name="receiptFlag">"L" Son belge veya rapor bilgilerini döner, "C" : read last receipt or report data (no” RcptType” in reply)</param>
        /// <returns>SerialNo,DDMMYYhhmm,CashierName,CashierNum,EjNo,ZNo,FisCodeNum[,RcptType]</returns>
        public FpuReadLastReceiptDetail ReadLastReceiptDetail(string receiptFlag)
        {
            return resultReadLastReceiptDetail(_FPU.ReadLastorCurrentReceipt(receiptFlag));
        }

        /// <summary>
        /// Cihazın farklı bir adrese ping atması için kullanılır.
        /// </summary>
        /// <returns></returns>
        public FPUResult ConnectToBaidu()
        {
            return resultMap(_FPU.ConnectToBaidu());
        }

        /// <summary>
        /// Restoran hizmeti kullanan yerlerde adisyon açılır açılmaz Eküde iz bırakmak için kullanılacak fonksiyondur.
        /// Bu fonksiyon aracılıgıyla X ve Z raporunda gerekli alanlar güncellenir.
        /// </summary>
        /// <param name="CheckId">Adisyon id</param>
        /// <param name="CheckAmount">Adisyon Tutarı</param>
        /// <returns></returns>
        public FPUResult Addrestaurantcheck(int CheckId, double CheckAmount)
        {
            return resultMap(_FPU.Addrestaurantcheck(CheckId, CheckAmount));
        }

        /// <summary>
        /// Restoran sistemlerinde Adisyon satış fişine dönüştürüleceği zaman belge sağlıklı bir şekilde kapatılırsa beklemedeki adisyonu silebilmek için adisyon id si verilerek kullanılır.
        /// </summary>
        /// <param name="CheckId">Adisyon id</param>
        /// <returns></returns>
        public FPUResult Deleterestaurantcheck(int CheckId)
        {
            return resultMap(_FPU.Deleterestaurantcheck(CheckId));
        }

        /// <summary>
        /// Qrcode yazdırmak için kullanılır.
        /// </summary>
        /// <param name="code">Qrcode a dönüştürülecek data.</param>
        public void PrintQrCode(string code)
        {
            _FPU.PrintQrCode(code);
        }

        /// <summary>
        /// FpuOperationResult classını soyutlamak amacıyla  gelen değerler FPUResult türüne dönüştürülür.
        /// </summary>
        /// <param name="result">FpuOperationResult tipinde değer alır</param>
        /// <returns>FPUResult türünde sonuç döner.</returns>
        private FPUResult resultMap(FpuOperationResult result)
        {
            return new FPUResult
            {
                Data = result.Data,
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage
            };
        }

        private FPUFiscalSerial resultFiscalSerialMap(FiscalSerialResult result)
        {
            return new FPUFiscalSerial
            {
                Data = result.Data,
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                FiscalCode = result.FiscalCode,
                FiscalNumber = result.FiscalNumber,
            };
        }

        private FPUReadTax resultReadTaxMap(FPUReadAllTax result)
        {
            return new FPUReadTax
            {
                Data = result.Data,
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                TaxAmount1 = result.TaxAmount1,
                TaxAmount2 = result.TaxAmount2,
                TaxAmount3 = result.TaxAmount3,
                TaxAmount4 = result.TaxAmount4,
                TaxAmount5 = result.TaxAmount5,
                TaxAmount6 = result.TaxAmount6,
                TaxAmount7 = result.TaxAmount7,
                TaxAmount8 = result.TaxAmount8,
            };
        }

        private FpuReadLastReceiptDetail resultReadLastReceiptDetail(ReadLastorCurrentReceiptDetail result)
        {
            return new FpuReadLastReceiptDetail
            {
                Data = result.Data,
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                ReceiptNumber = result.ReceiptNumber,
                ReceiptDateTime = result.ReceiptDateTime,
                CashierName = result.CashierName,
                EjNumber = result.EjNumber,
                ZNo = result.ZNo,
                FiscalCodeNumber = result.FiscalCodeNumber,
                ReceiptType = result.ReceiptType,
                CashierNumber = result.CashierNumber,
            };
        }

        private FPUReadDepartment resultReadDep(FpuDepartment result)
        {
            return new FPUReadDepartment
            {
                Data = result.Data,
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                DepId = result.DepId,
                DepLimit = result.DepLimit,
                DepName = result.DepName,
                DepPrice = result.DepPrice,
                Tax = result.Tax,
            };
        }

        private FPUReadVersion resultReadVersion(FPUGetVersion result)
        {
            return new FPUReadVersion
            {
                Data = result.Data,
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                OSVersion = result.OSVersion,
                SDKVersion = result.SDKVersion,
                AppVersion = result.AppVersion,
                App2Version = result.App2Version,
                SSLVersion = result.SSLVersion,
                SecurityICVersion = result.SecurityICVersion,
            };
        }

        private FPUMac resultFpuMac(ReadFpuMacAdress result)
        {
            return new FPUMac
            {
                Data = result.Data,
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                FpuMacAdress = result.FpuMacAdress,
            };
        }

        private FPUGetCashInOutDrawer resultCashInOutDrawer(GetCashInOutDrawer result)
        {
            return new FPUGetCashInOutDrawer
            {
                Data = result.Data,
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                Cash = result.Cash,
                CashIn = result.CashIn,
                CashOut = result.CashOut,
            };
        }

        //#region S Mode Functions
        ///// <summary>
        ///// Gain the ID code to log in S MODE
        ///// </summary>
        ///// <returns>int</returns>
        //public int GetIntoSmodeIDCode(string serviceId)
        //{
        //    return Imports.__GetIntoSmodeIDCode(serviceId);
        //}

        ///// <summary>
        ///// Log in S MODE
        ///// </summary>
        ///// <param name="VerifyCode"></param>
        ///// <returns>int</returns>
        //public int IntoSmode(string verifyCode)
        //{
        //    return Imports.__IntoSmode(verifyCode);
        //}

        ///// <summary>
        ///// Gain the ID code to log out S MODE
        ///// </summary>
        ///// <returns>int</returns>
        //public int GetExitSmodeIDCode(string serviceId)
        //{
        //    return Imports.__GetExitSmodeIDCode(serviceId);
        //}

        ///// <summary>
        ///// Log Out S Mode Code
        ///// </summary>
        ///// <param name="VerifyCode"></param>
        ///// <returns></returns>
        //public int ExitSmode(string verifyCode)
        //{
        //    return Imports.__ExitSmode(verifyCode);
        //}

        ///// <summary>
        ///// Method to generate verify code
        ///// </summary>
        ///// <param name="idCode"></param>
        ///// <returns></returns>
        //public void GenerateVerifyCode(string idCode, StringBuilder sb)
        //{
        //    Imports.__GenerateVerifyCode(idCode, sb);
        //}

        ///// <summary>
        ///// Set time
        ///// </summary>
        ///// <param name="time">the time is set, and format should be HHMMSS</param>
        ///// <returns></returns>
        //public int SettingTime(string time)
        //{
        //    return Imports.__SettingTime(time);
        //}

        ///// <summary>
        ///// Set date
        ///// </summary>
        ///// <param name="date">the date is set, and format should be DDMMYY</param>
        ///// <returns></returns>
        //public FPUResult SettingDate(string date)
        //{
        //    var resultCode = Imports.__SettingDate(date);

        //    return new FPUResult(resultCode);
        //}

        ///// <summary>
        ///// Read time
        ///// </summary>
        ///// <returns></returns>
        //public int ReadDateTime()
        //{
        //    return Imports.__ReadDateTime();
        //}

        ///// <summary>
        ///// Set fiscal code& number
        ///// </summary>
        ///// <param name="fiscalCode">fiscal code（3 digit</param>
        ///// <param name="fiscalNumber">FiscalNumber （8 digit</param>
        ///// <returns></returns>
        //public int SetFiscalCode(string fiscalCode, string fiscalNumber)
        //{
        //    return Imports.__SetFiscalCode(fiscalCode, fiscalNumber);
        //}

        ///// <summary>
        ///// Gain fiscal code& number
        ///// </summary>
        ///// <returns></returns>
        //public int GetFiscalCodeNum()
        //{
        //    return Imports.__GetFiscalCodeNum();
        //}

        ///// <summary>
        ///// Log in user mode
        ///// </summary>
        ///// <param name="serial">The individual device number.</param>
        ///// <returns></returns>
        //public int EnterUserMode(string serial)
        //{
        //    return Imports.__EnterUserMode(serial);
        //}

        ///// <summary>
        ///// Set recovery parameter
        ///// </summary>
        ///// <param name="fiscalnumber">fiscal number</param>
        ///// <param name="cumulativeTotal">kümülatif toplam</param>
        ///// <param name="cumulativeTax">kümülatif kdv</param>
        ///// <returns></returns>
        //public int SetRecoveryData(string fiscalnumber, double cumulativeTotal, double cumulativeTax)
        //{
        //    return Imports.__SetRecoveryData(fiscalnumber, cumulativeTotal, cumulativeTotal);
        //}

        ///// <summary>
        ///// ECR Reset
        ///// </summary>
        ///// <param name="serial">The individual device number.</param>
        ///// <returns></returns>
        //public int ResetOperation(string serial)
        //{
        //    return Imports.__ResetOperation(serial);
        //}

        ///// <summary>
        ///// Gain FPU sw version
        ///// </summary>
        ///// <returns></returns>
        //public int GetVersion()
        //{
        //    return Imports.__GetVersion();
        //}

        ///// <summary>
        ///// Set TIN or NIN
        ///// </summary>
        ///// <param name="typet">‘T’ : Setting TIN,  ‘N’: Setting NIN</param>
        ///// <param name="number">TIN or NIN number</param>
        ///// <returns></returns>
        //public int SetTinorNinNumber(string typet, string number)
        //{
        //    return Imports.__SetTinorNinNumber(typet, number);
        //}

        ///// <summary>
        ///// Fiscalization
        ///// </summary>
        ///// <param name="serial">The individual device number</param>
        ///// <returns></returns>
        //public int Fiscalization(string serial)
        //{
        //    return Imports.__Fiscalization(serial);
        //}

        ///// <summary>
        ///// Print event log
        ///// </summary>
        ///// <param name="eventNumber">event log index
        /////         0~90 : print event according index
        /////         91 : print broken event
        /////         92 : print all event </param>
        ///// <returns></returns>
        //public int PrintEventLog(int eventNumber)
        //{
        //    return Imports.__PrintEventLog(eventNumber);
        //}

        ///// <summary>
        ///// print last n records event
        ///// </summary>
        ///// <param name="eventNumber">(0 < EventNumber<= 100)</param>
        ///// <returns></returns>
        //public int PrintLastEventLog(int eventNumber)
        //{
        //    return Imports.__PrintLastEventLog(eventNumber);
        //}
        ///// <summary>
        ///// print event according date
        ///// </summary>
        ///// <param name="eventDate">DateFormat(DDMMYY)</param>
        ///// <returns></returns>
        //public int PrintEventLogBydate(string eventDate)
        //{
        //    return Imports.__PrintEventLogBydate(eventDate);
        //}

        ///// <summary>
        ///// Upload FM data to PC as address(hex data)
        ///// </summary>
        ///// <param name="address">address Hex data, 5 digit </param>
        ///// <returns></returns>
        //public int ReadFMBlock(string address)
        //{
        //    return Imports.__ReadFMBlock(address);
        //}

        ///// <summary>
        ///// Set Network type
        ///// </summary>
        ///// <param name="networkType">NetworkType: L = Lan, G = GPRS,P = PSTN</param>
        ///// <returns></returns>
        //public int SetNetworkType(string networkType)
        //{
        //    return Imports.__SetNetworkType(networkType);
        //}

        ///// <summary>
        ///// Read network Type
        ///// </summary>
        ///// <returns></returns>
        //public int ReadNetworkType()
        //{
        //    return Imports.__ReadNetworkType();
        //}

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="networkType">L = Lan, G = GPRS,P = PSTN</param>
        ///// <param name="iPaddress">IPAddress</param>
        ///// <param name="mask"></param>
        ///// <param name="gw"></param>
        ///// <param name="dns">DNS</param>
        ///// <returns></returns>
        //public int SetIpv4Adr(int dhcpFlag, string networkType, string iPaddress, string mask, string gw, string dns)
        //{
        //    return Imports.__SetIpv4Adr(dhcpFlag, networkType, iPaddress, mask, gw, dns);
        //}

        ///// <summary>
        ///// Read IPV4 parameter
        ///// </summary>
        ///// <param name="networkType">NetworkType: L = Lan, G = GPRS,P = PSTN</param>
        ///// <returns></returns>
        //public int ReadIpv4Adr(string networkType)
        //{
        //    return Imports.__ReadIpv4Adr(networkType);
        //}

        ///// <summary>
        ///// Set IPV6 parameter
        ///// </summary>
        ///// <param name="networkType">L = Lan, G = GPRS,P = PSTN </param>
        ///// <param name="iPaddress">IPADRESS</param>
        ///// <param name="mask"></param>
        ///// <param name="gw"></param>
        ///// <param name="dns">DNS</param>
        ///// <returns></returns>
        //public int SetIpv6Adr(int dhcpFlag, string networkType, string iPaddress, string mask, string gw, string dns)
        //{
        //    return Imports.__SetIpv6Adr(dhcpFlag, networkType, iPaddress, mask, gw, dns);
        //}

        ///// <summary>
        ///// Read IPV6 parameter
        ///// </summary>
        ///// <param name="networkType"> L = Lan, G = GPRS,P = PSTN</param>
        ///// <returns></returns>
        //public int ReadIpv6Adr(string networkType)
        //{
        //    return Imports.__ReadIpv6Adr(networkType);
        //}

        ///// <summary>
        ///// GPRS connection
        ///// </summary>
        ///// <returns></returns>
        //public int GprsConnect()
        //{
        //    return Imports.__GprsConnect();
        //}

        ///// <summary>
        ///// GPRS disconnection
        ///// </summary>
        ///// <returns></returns>
        //public int GprsDisConnect()
        //{
        //    return Imports.__GprsDisConnect();
        //}

        ///// <summary>
        ///// Gain GPRS IMEI parameter
        ///// </summary>
        ///// <returns></returns>
        //public int GetGprsIMEI()
        //{
        //    return Imports.__GetGprsIMEI();
        //}

        ///// <summary>
        ///// Set GPRS parameter
        ///// </summary>
        ///// <param name="apn"></param>
        ///// <param name="dial"></param>
        ///// <param name="name"></param>
        ///// <param name="psw"></param>
        ///// <returns></returns>
        //public int SetGprsPara(string apn, string dial, string name, string psw)
        //{
        //    return Imports.__SetGprsPara(apn, dial, name, psw);
        //}

        ///// <summary>
        ///// Read GPRS parameter
        ///// </summary>
        ///// <returns></returns>
        //public int ReadGprsPara()
        //{
        //    return Imports.__ReadGprsPara();
        //}

        ///// <summary>
        ///// Set PSTN parameter
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="dial"></param>
        ///// <param name="psw"></param>
        ///// <returns></returns>
        //public int SetPstnPara(string name, string dial, string psw)
        //{
        //    return Imports.__SetPstnPara(name, dial, psw);
        //}

        ///// <summary>
        ///// Read PSTN parameter
        ///// </summary>
        ///// <returns></returns>
        //public int ReadPstnPara()
        //{
        //    return Imports.__ReadPstnPara();
        //}

        ///// <summary>
        ///// PSTN connection
        ///// </summary>
        ///// <returns></returns>
        //public int PstnConnect()
        //{
        //    return Imports.__PstnConnect();
        //}

        ///// <summary>
        ///// PSTN disconnection
        ///// </summary>
        ///// <returns></returns>
        //public int PstnDisconnect()
        //{
        //    return Imports.__PstnDisconnect();
        //}

        ///// <summary>
        ///// Update FPU application
        ///// </summary>
        ///// <returns></returns>
        //public int UpgradeSystem()
        //{
        //    return Imports.__UpgradeSystem();
        //}

        ///// <summary>
        ///// Change DB SD card
        ///// </summary>
        ///// <returns></returns>
        //public int ChangeInterdSd()
        //{
        //    return Imports.__ChangeInterdSd();
        //}

        ///// <summary>
        ///// set ECR ACTIVE ---this point, FPU firmware need more updating, will support in next version.
        ///// </summary>
        ///// <param name="isActive">Active or not</param>
        ///// <returns></returns>
        //public int SetEcrActive(Boolean isActive)
        //{
        //    return Imports.__SetEcrActive(isActive);
        //}
        //#endregion
    }
}