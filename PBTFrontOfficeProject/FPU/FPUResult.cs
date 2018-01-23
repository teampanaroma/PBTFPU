using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTFrontOfficeProject
{
    /// <summary>
    /// FPUOperations classını soyutlamak için gelen değerler FPUResult class türüne dönüştürülür.
    /// </summary>
    public class FPUResult
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Data { get; set; }
    }

    public class FPUFiscalSerial : FPUResult
    {
        public string FiscalCode { get; set; }

        public string FiscalNumber { get; set; }
    }

    public class FPUReadTax : FPUResult
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

    public class FpuReadLastReceiptDetail:FPUResult
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

    public class FPUReadDepartment:FPUResult
    {
        public int DepId { get; set; }
        public string DepName { get; set; }
        public double DepPrice { get; set; }
        public double DepLimit { get; set; }
        public string Tax { get; set; }  
    }

    public class FPUTaxOperation : FPUResult
    {
        public string TaxName { get; set; }
        public double TaxAmount { get; set; }
    }

    public class FPUReadVersion:FPUResult
    {
        public string AppVersion { get; set; }
        public string App2Version { get; set; }
        public string OSVersion { get; set; }
        public string SDKVersion { get; set; }
        public string SSLVersion { get; set; }
        public string SecurityICVersion { get; set; }
    }

    public class FPUMac:FPUResult
    {
        public string FpuMacAdress { get; set; }
    }

    public class FPUGetCashInOutDrawer : FPUResult
    {
        public string Cash { get; set; }
        public string CashIn { get; set; }
        public string CashOut { get; set; }
    }
}
