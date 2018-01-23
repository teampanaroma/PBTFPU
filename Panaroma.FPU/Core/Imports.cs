using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Panaroma.FPU.Core
{
    public delegate void TOnReceiveData(IntPtr ReceiveData, int ReceiveLen, string FpuStatus, int ErrorMessage, int ipos, int cmdFlag = 0); //定义回调函数，获取状态及数据解析  3 wait 状态
    public delegate void TDeviceFun(string sdevice, int i);//初始化USB 设备
    public delegate void TDeviceIDFun(int vid, int pid, int DeviceIndex=0);//初始化USB 设备
    #region SMode
    /// <summary>
    /// Dll içerisinde yer alan fonksiyonlar tanımlanır.
    /// </summary>
    public class Imports
    {

        [DllImport("Lib\\Fprinter.dll")]
        public extern static void __Config(string iCom, int ConnMode, int idevice);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static void __Open();

        [DllImport("Fprinter.dll")]//Fiş başlığı kaybolma
        public extern static void __SavePrintData(string PrintData);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static Boolean __CancelReadReceipt();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static void __Close();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static Boolean __Active();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetEcrActive(Boolean Isactive);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __CashInOut(double CashAmount, string detailMessage = "");

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __NoSaleFun();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintParameter();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __OpenFiscalReceipt(int clerkId, int InfomationSlipType, string ClerkPsw, string CustomerMessage, string SerialSequenceNo, int RegisterF = 0);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __OpenFiscalReceipt_1(int clerkId, int InfomationSlipType, string ClerkPsw, string CustomerMessage, string SerialSequenceNo, int RegisterF = 0);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __CashierLogin(int clerkId, string ClerkPsw);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetModeLogin(string SetModePsw);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetSetModePsw(string SetModePsw);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SettingHeaderMessage(int Item, string Text);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SettingPromotioninformation(int Item, string Text);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadPromotioninformation(int Item);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Addrestaurantcheck(int CheckId, double CheckAmount);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Deleterestaurantcheck(int CheckId);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadHeaderMessage(int Item);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadFootMessage(int Item);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetForeignCurrency(int CurrencyIndex, string CurrencyName, double ExchangeRate, Boolean Subsidiary);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __DefAddPayName(int Option, string description);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SettingFootMessage(int Item, string Text);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SettingWebAddress(string Text);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SettingMersisNo(string Text);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadMersisNo();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadWebAddress();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SettingDate(string Date);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintOKorInterrupt(string TFlag);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ZReportLimit(int RptLimit);


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetEventFullNum(int FullNum);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadEventFullNum();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetPriceLimit(double PriceLimit);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __checkuppayment(double Price);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __presaddedvalue(double Price);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ChangeEcrMode(int EcrMode);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __EventRegister(int EventSource, int Eventlevel, int EventNum);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintFreeFiscalText(string FiscalFreeText);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintNonFiscalFreeText(string Nonfreemessage);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetMachineId(string machineNum);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetMachineSerialId(string MachineSerialNo);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadMachineSerialId();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadAllMachineMsg();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __OpenNonFiscalReceipt();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __CloseNonFiscalReceipt();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetProtocolType(int ProIndex);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FmCapacity(int Capacity);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __EjCapacity(int Capacity);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FMGenerateDays(int GenerateDays);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __EJGenerateLines(int GenerateLines);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __TestSdCard(int sdIndex);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __UnlockSDCard(int sdIndex);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __LockSDCard(int sdIndex);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __UpgradeSic();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __UpgradeSystem();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ChangeInterdSd();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ClearAllData();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __RestoreAppsData();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetNetworkType(string NetworkType);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadNetworkType();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetIpv4Adr(int dhcpFlag, string NetworkType, string IPaddress, string mask, string gw, string dns);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadIpv4Adr(string NetworkType);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetIpv6Adr(int dhcpFlag, string NetworkType, string IPaddress, string mask, string gw, string dns);


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ExpenseslipInfo(int Number, int CancelNum, double Total, double Tax, double Cash, double Credit, double OtherPayTotal,
                                                             double FcTotal, double FcChange, double CancelTotal);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PaymentpaperInfo(int Number, int CancelNum, double Total, double Tax, double Cash, double Credit, double OtherPayTotal,
                                                             double FcTotal, double FcChange, double CancelTotal);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ShippingpaperInfo(int Number, int CancelNum, double Total, double Tax, double Cash, double Credit, double OtherPayTotal,
                                                             double FcTotal, double FcChange, double CancelTotal);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ExchangeInfo(int Number, int CancelNum);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadIpv6Adr(string NetworkType);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GprsConnect();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GprsDisConnect();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetGprsIMEI();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetGprsPara(string apn, string dial, string name, string psw);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetTMSParameter(string addr, int EthernetPort1, int EthernetPort2, int GPRSPort1, int GprsPort2);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetTMSParameter();


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintTSMIpSettings();


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetTSMPara(string EthernetPrimaryIp, string EthernetPrimaryPort, string EthernetSecondaryIp, string EthernetSecondaryPort, string DailPrimaryPhoneNum,
                                              string DailSecondaryPhoneNum, string GPRSPrimaryIp, string GPRSPrimaryPort, string GPRSSecondaryIp,
                                              string GPRSSecondaryPort, string GSMPrimaryPhoneNum, string GSMSecondaryPhoneNum);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadTSMPara();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __TestRmp(string RmpType);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetRmpPara(string TerminalSerialNo, string TerminalNum, string WorkPlaceNum, string TaxNum, string TmsServerIp, string TmsServerPort,
                                              string EthernetPrimaryRadPort, string EthernetSecondaryRadPort, string GprsPrimaryRadPort, string GprsSecondaryRadPort,
                                              string EthernetPrimarySSLRadPort, string EthernetSecondarySSLRadPort, string GprsPrimarySSLRadPort, string GprsSecondarySSLRadPort,
                                              int CertType);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadRmpPara();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadGprsPara();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetPstnPara(string name, string dial, string psw);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadPstnPara();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __AppVerifyMode(int SignType);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PstnConnect();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PstnDisconnect();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetFireWall(Boolean openFlag);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadFireWall();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetFiscalCode(string FiscalCode, string FiscalNumber);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetTinorNinNumber(string Typet, string Number);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SettingTime(string Time);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetClerkName(int ClerkId, string ClerkName);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetClerkPswd(int ClerkId, string newPassword, string ClerkName);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadClerkPswd(int ClerkId);


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetRmpRadVersion(int VersionType);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SaveFiscalReceipt(int FiscalReceiptNo);//FiscalReceiptNo 挂单号

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintRecallinfomation(int FiscalReceiptNo, double Total, double TotTax);//FiscalReceiptNo 挂单号

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadRmpRadVersion();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SettingTaxRate(double taxA, double taxB, double taxC, double taxD, double taxE, double taxF, double taxG, double taxH);


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Fiscalization(string Serial);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __EnterUserMode(string Serial);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ResetOperation(string Serial);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintEventLog(int EventNumber);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintLastEventLog(int EventNumber);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintEventLogBydate(string EventDate);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadDailyMemory(int DailyMemoryIdx);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Registeringsale(string PluName, string Flag, string PluBarcode, string FlagType, int DepId, int GroupId, int DepOrPlu, int unitPrice, int Value, int quantity);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Registeringsale_1(string PluName, string Flag, string PluBarcode, string FlagType, int DepId, int GroupId, int DepOrPlu, int unitPrice, int Value, int quantity);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PreAndDiscount(string FlagType, int value);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PreAndDiscount_1(string FlagType, int value);


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadFMDatailByNumber(int startNumber, int endNumber);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadFMDatailByDate(string startdate, string enddate);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadFMSumaryByNumber(int startNumber, int endNumber);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadFMSaleReport(int ZReportNumber);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadFMSumaryByDate(string startdate, string enddate);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetDepartments(int DepId, string TaxGr, double Limit, double UnitPrice, string Depname, string IsPrintF);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetGroup(int GroupId, int DepId, string GroupName, string IsPrintF);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadDepartments(int DepId);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadForeignCurrency(int CurrencyIndex);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadPayNmae(int Option);


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadRMPMenuStatues(int MenuFlag);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetCashInDrawer();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadGroup(int GroupID);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __UploadTaxRate();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintZReport();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Over24HourZRep(string LastZDateTime);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __OpenEjModule();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __CloseEjModule();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __EJZInfoReport();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __finalizingreceiptSwitch(int iFlag);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadEJSumaryByNumber(int SZReportNo, int EZReportNo, int SRReportNo, int ERReportNo);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadEJSummaryByDate(string startdate, string enddate);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __EJModuleReport();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadEJSpecificReceiptByNumberToNumber(int SZReportNo, int EZReportNo, int SRReportNo, int ERReportNo);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadEJSpecificReceiptByDateTimeToDateTime(string startdate, string enddate);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadEJSpecificReceiptByDatetimeNo(string CurrentDatetime, int ReceiptNo);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadEJSpecificReceiptByDatetime(string CurrentDatetime);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadEJSpecificReceiptByNumber(int ZReportNo, int ReceiptNo);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FormatSDCard(int sdcardIndex);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __TMSUpgrade();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintXReport();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Errorcorrection();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Errorcorrection_1();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __CancelFiscalReceipt();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __CancelFiscalReceipt_1();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SpecilCancelFiscalReceipt();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __CloseFiscalReceipt();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Subtotal();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Subtotal_1();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadFMBlock(string address);//address 16进制数值 5位

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadDateTime();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReciveFPUStatus();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetFiscalCodeNum();


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetVersion();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetTrueVersion();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadLastFiscal(string ReceiptFlag);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadLastorCurrentReceipt(string ReceiptFlag);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetFpuCurrentMode();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetFreeNumber();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetEthernetEnable(int TypeFlag);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __EJCOMMUNICATIONTEST(int TypeFlag);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __DELETEFISCALAPP();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __KILLFISCALAPP();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SETCANWRITEFM(int TypeFlag);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FMCONNECTION(int TypeFlag);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FactorySETDATETIME(int year, int month, int day, int hour, int min, int sec);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Totalcalculating(int ModeType, int Amount, int iType = 0, int InstalmentNumber = 0);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ConnectToBaidu();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __IntoSmode(string VerifyCode);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetIntoSmodeIDCode(string ServiceId);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetExitSmodeIDCode(string ServiceId);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ExitSmode(string VerifyCode);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static void __GenerateVerifyCode(string SourceId, StringBuilder DestId);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Printinvoicebyhand(string SerialNo, string InvoiceNo, double Total, double Cash, double Credit, double Check,
                                                         string FcName1, string FcName2, string FcName3, string FcName4, string FcName5, string FcName6,
                                                         double FcAmt1, double FcAmt2, double FcAmt3, double FcAmt4, double FcAmt5, double FcAmt6,
                                                         double FcLocal1, double FcLocal2, double FcLocal3, double FcLocal4, double FcLocal5, double FcLocal6, string InvoiceDate);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintEInvoice(string SerialNo, string InvoiceNo, double Total, double Cash, double Credit, double Check,
                                                         string FcName1, string FcName2, string FcName3, string FcName4, string FcName5, string FcName6,
                                                         double FcAmt1, double FcAmt2, double FcAmt3, double FcAmt4, double FcAmt5, double FcAmt6,
                                                         double FcLocal1, double FcLocal2, double FcLocal3, double FcLocal4, double FcLocal5, double FcLocal6, string InvoiceDate);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintDataInRec(string InvCustomerStr0, string InvCustomerStr1, string InvCustomerStr2, string InvCustomerStr3, string InvCustomerStr4);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __OpenPluReport(int RepTypeIndex);


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintMessage(int RepTypeIndex);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Printinterruption(int StatusIndex);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetRepairTime(int RepairTime);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SetRecoveryData(string fiscalnumber, double CumulativeTotal, double CumulativeTax);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static void __IniOnShowReceiveData(TOnReceiveData value);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static void __Loadfun1(TDeviceFun value);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static void __Loadfun2(TDeviceIDFun value);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __RmpExternalHardWate(int TypeFlag, int ComMenuType, string RemoteIp, string LocalPort);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __RmpRegister(int NGECRcertificate_Type);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __RmpEchoMessage();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetExternalHardWareCount();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetExternalHardWareMsg(int iNum);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __DelExternalHardWareMsg(int iNum);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetKeysEFTPOS(int iNum);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintMealChkInformation(double SalesTotal, double MealCardAmount, double MealCheckAmount);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __PrintCarParkInformation(string PlateNumber, string EntranceDate, string EntranceTime);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __Printsecondcopyreceipt();


        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ConnectTest();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FpuEftPosFun_Cancel(int Zno, int EkuNo, int StanNum, double Amount, string FisNo, string AcquirerID);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FpuEftPosFun_Refund(int Zno, int EkuNo, int BatchNo, double Amount, string FisNo, string AcquirerID, int InstallmentNum = 0);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FpuEftPosOption(int iType, int iFlag = 0);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FpuEftPosMaintenance(int iType, string AcquirerID);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FpuEftPosEndOfDay(string AcquirerID);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FpuEftPosBankList();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FpuEftPosStatusInformation();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __EftPosDebugFlag(int iType);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __ReadEftDebugFile(int iaddr);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __DelEFTPOSDebugFile();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __GetMacMessage();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __UsbHostTest();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FpuEftPosSlip_Copy(int SlipType, string BatchNo, string Stan, string AcquirerId, string MerchantId, string TerminalId);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FpuEftPosEndOfDay_Copy(string BatchNo, string AcquirerId);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FixFmBelongToAnother();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __TestMasterCmd();

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __SendProtocolMessage(string TxtStr);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __InvoicePayment(string Company, string Date, string BillNo, string SubscriberNo, double InvoiceAmount, double CommissionAmount,
                                                  double Cash, double Credit, double Check);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __AdvacePayment(string TinOrNinNum, string NameTitle, double AdvanceAmount, double Cash, double Credit, double Check);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __DFRSkipZero(int option);

        [DllImport("Lib\\Fprinter.dll")]
        public extern static int __FixEjBroken();
    }
    #endregion
}
