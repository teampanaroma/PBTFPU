using Panaroma.FPU.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaroma.FPU
{
    internal static class PrintHelper
    {
        static List<FpuCommandsModel> _CommandWithPrint;

        internal static List<FpuCommandsModel> CommandWithPrint
        {
            get { return PrintHelper._CommandWithPrint; }
        }

        static PrintHelper()
        {
            prepareCommandWithPrintList();
        }

        private static void prepareCommandWithPrintList()
        {
            if (_CommandWithPrint == null)
                _CommandWithPrint = new List<FpuCommandsModel>();
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PrintXReport, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PrintZReport, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.Over24HourZRep, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.EJZInfoReport, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.CashInOut, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PrintParameter, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.SettingHeaderMessage, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.SetForeignCurrency, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.SettingFootMessage, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.SettingDate, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PrintFreeFiscalText, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PrintNonFiscalFreeText, Cut = false });
            //_CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.setReportOtherInformation, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.SetTinorNinNumber, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.SetClerkName, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PrintRecallinfomation, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.SettingTaxRate, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.Fiscalization, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.EnterUserMode, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PrintEventLog, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PrintLastEventLog, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PrintEventLogBydate, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadFMDatailByNumber, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadFMDatailByDate, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadFMSumaryByNumber, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadFMSaleReport, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadFMSumaryByDate, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.SetDepartments, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.SetGroup, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.OpenEjModule, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadEJSumaryByNumber, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadEJSummaryByDate, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadEJSpecificReceiptByNumberToNumber, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadEJSpecificReceiptByDateTimeToDateTime, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadEJSpecificReceiptByDatetimeNo, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadEJSpecificReceiptByDatetime, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ReadEJSpecificReceiptByNumber, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.CancelFiscalReceipt, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.SpecilCancelFiscalReceipt, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PrintMessage, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.Printinvoicebyhand, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.OpenFiscalReceipt, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.Registeringsale, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.Totalcalculating, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.CloseFiscalReceipt, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.EJModuleReport, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.PreAndDiscount, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.CloseNonFiscalReceipt, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.OpenNonFiscalReceipt, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.Subtotal, Cut = false }); //
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ConnectTest, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.ConnectTest, Cut = true }); //           
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.GetMacMessage, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.InvoicePayment, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.AdvacePayment, Cut = true });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.checkuppayment, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.presaddedvalue, Cut = false });
            _CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.finalizingreceiptSwitch, Cut = true });

            //_CommandWithPrint.Add(new FpuCommandsModel { Command = FpuCommands.DefAddPayName, Cut = true });
        }

    }
}
