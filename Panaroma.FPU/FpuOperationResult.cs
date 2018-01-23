using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceProject.Service;
using Panaroma.FPU.Core;
namespace Panaroma.FPU
{
    /// <summary>
    /// FPU ya gönderilen fonksiyonlardan dönen değerleri tutar.
    /// </summary>
    public class FpuOperationResult
    {
        /// <summary>
        /// Hatakodu
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Hata mesajı
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Dönen değer.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// FPU dan dönen hata kodu Error Code classı türkçe map yapar.
        /// </summary>
        public ErrorCode errorDetail { get; set; }

    }
    public class DateTimeInfo : FpuOperationResult
    {
        public string Date { get; set; }
        public string Time { get; set; }
    }
    public class FiscalSerialResult : FpuOperationResult
    {
        public string FiscalCode { get; set; }

        public string FiscalNumber { get; set; }
    }

    public class GetPosListResult : FpuOperationResult
    {
        public string posConsumer { get; set; }
        public int posCount { get; set; }

        public string serialNo { get; set; }
        public string modelNo { get; set; }
        public string ecrVersion { get; set; }
    }


    public class FpuDepartment : FpuOperationResult
    {
        public int DepId { get; set; }
        public string DepName { get; set; }
        public double DepPrice { get; set; }
        public double DepLimit { get; set; }
        public string Tax { get; set; }
    }

    public class FPUGroup : FpuOperationResult
    {
        public int GroupId { get; set; }
        public int DepId { get; set; }
        public string GroupName { get; set; }
    }

    public class FPUReadAllTax : FpuOperationResult
    {
        public double TaxAmount1 { get; set; }
        public double TaxAmount2 { get; set; }
        public double TaxAmount3 { get; set; }
        public double TaxAmount4 { get; set; }
        public double TaxAmount5 { get; set; }
        public double TaxAmount6 { get; set; }
        public double TaxAmount7 { get; set; }
        public double TaxAmount8 { get; set; }

    }

    public class ReadLastorCurrentReceiptDetail : FpuOperationResult
    {
        public string ReceiptNumber { get; set; }
        public string ReceiptDateTime { get; set; }
        public string CashierNumber { get; set; }
        public string EjNumber { get; set; }
        public string ZNo { get; set; }
        public string FiscalCodeNumber { get; set; }
        public string ReceiptType { get; set; }
        public string CashierName { get; set; }
    }

    public class FPUReadIpV4 : FpuOperationResult
    {
        public string DHCP { get; set; }
        public string IP { get; set; }
        public string Mask { get; set; }
        public string Gateway { get; set; }
        public string DNS { get; set; }
    }



    public class FPUSubTotal : FpuOperationResult
    {
        public double SubTotal { get; set; }
    }

    public class FPUGetVersion : FpuOperationResult
    {
        public string AppVersion { get; set; }
        public string App2Version { get; set; }
        public string OSVersion { get; set; }
        public string SDKVersion { get; set; }
        public string SSLVersion { get; set; }
        public string SecurityICVersion { get; set; }
    }


    public class FPUGetExitModeCode : FpuOperationResult
    {
        public string GetExitMode { get; set; }
    }

    public class ReadFpuMacAdress : FpuOperationResult
    {
        public string FpuMacAdress { get; set; }
    }

    public class HandShakedHardWare : FpuOperationResult
    {
        public int HardWareCount { get; set; }
    }
    public class HardWareList : FpuOperationResult
    {

    }

    public class NetworkType : FpuOperationResult
    {
        public int networkType { get; set; }
    }
    public class IpAddressInfo : FpuOperationResult
    {
        public bool ipV4 { get; set; }
        public bool ipV6 { get; set; }
        public bool isDhcp { get; set; }
        public string ipAdd { get; set; }
        public string mask { get; set; }
        public string gw { get; set; }
        public string dns { get; set; }
    }
    public class TsmParamInfo : FpuOperationResult
    {
        public string firstIp { get; set; }
        public string firstPort { get; set; }
        public string secondIp { get; set; }
        public string secondPort { get; set; }
    }
    public class TmsParamInfo : FpuOperationResult
    {
        public string tmsAddress { get; set; }
        public string tmsFirstPort { get; set; }
        public string tmsSecondPort { get; set; }

    }
    public class FiscalSwInfo : FpuOperationResult
    {
        public string os { get; set; }
        public string sdk { get; set; }
        public string app1 { get; set; }
        public string app2 { get; set; }
        public string ssl { get; set; }
        public string securityIc { get; set; }
    }
    public class RmpSettingInfo : FpuOperationResult
    {
        public string serialNo { get; set; }
        public string taxNo { get; set; }
    }
    public class FPUReadTSMParameter : FpuOperationResult
    {
        public string TSMIp { get; set; }
    }

    public class GetCashInOutDrawer : FpuOperationResult
    {
        public string CashIn { get; set; }
        public string CashOut { get; set; }
        public string Cash { get; set; }
    }
}
