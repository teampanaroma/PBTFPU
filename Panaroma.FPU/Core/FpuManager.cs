using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Panaroma.FPU.Core
{
    /// <summary>
    /// FPU dll ini implemente eden class //FPrinter.dll
    /// </summary>
    internal class FpuManager
    {
        private const int FPU_TIMEOUT = 10000;//Verilen süre içerisinde FPU ile haberleşemezsen Timeouta düş.10000 den artırıldı.
        private static object LockObj = new object();
        #region FPU Fields
        private bool _FpuBusy;//FPU durumu
        private int _ErrorCode;//Dönen hatanın kodu.
        private string _Message;// Dönen hatanın mesajı.
        private int _Ipos; // işlem süresi gibi bişey
        private string _RefStr; // , ile ayrılmış array
        private string _Data;
        private string _FPURefStatus;
        private ErrorMsg errmsg = new ErrorMsg(); //
        private PrintOperations _PrinterOperation;
        private StringBuilder _ReceiptData;
        List<PrintSettings> _PrintDataList;
        private bool _IsReceiptOperation;
        #endregion

        public bool FpuBusy
        {
            get { return _FpuBusy; }
        }

        public FpuManager(int connMode, string comPort, bool printMode = true)
        {
            this._ConnMode = connMode;
            //this._PrintMode = printMode;
            InitFpu();
            _PrinterOperation = new PrintOperations(printMode);
            ExecuteCommandWithoutReceive(FpuCommands.Config, new object[] { comPort, connMode, this._DeviceIndex });
            Logger.Info("FPU Ready");
        }

        /// <summary>
        /// Müşteri göstergesine data yazmak için kullanılır.
        /// </summary>
        /// <param name="line1">üst satır</param>
        /// <param name="line2">alt satır</param>
        public void WriteToDisplay(string line1, string line2)
        {
            _PrinterOperation.writeToDisplay(line1, line2);
        }

        /// <summary>
        /// Çekmece açmak için kullanılır.
        /// </summary>
        public void OpenDrawer()
        {
            _PrinterOperation.OpenDrawer();
        }

        /// <summary>
        /// Printer donanım durumunu sorgulamak için kullanılır.
        /// </summary>
        /// <returns>False dönerse kağıt yada kapak ile sıkıntı vardır.</returns>
        public bool PrinterStatus()
        {
            return _PrinterOperation.yaziciKagitKontrol();
        }

        /// <summary>
        /// Bağlantı sağlandıktan sonra açılış işlemini gerçekleştirir.
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            ExecuteCommandWithoutReceive(FpuCommands.Open);

            _OpenPortSuccess = (bool)ExecuteCommand(FpuCommands.Active, false).MethodResult;
            if (_OpenPortSuccess)
            {

                string filename = Directory.GetCurrentDirectory() + "\\KeyBlock.dat";
                if (File.Exists(filename)) _RmpIsRegister = true;

                var fpuStatusResult = ExecuteCommand(FpuCommands.ReciveFPUStatus);

                var fpuStatusMResult = fpuStatusResult.GetMethodResult<int>();
                if (fpuStatusMResult == -1 || fpuStatusMResult == 1)
                {
                    throw new ApplicationException("Get FPU Status Fail");
                }

                PrintOK();
            }
            return _OpenPortSuccess;
        }

        public void Close()
        {
            ExecuteCommandWithoutReceive(FpuCommands.Close);
        }

        #region Execute Command Functions
        public ExecutedCommandResult ExecuteCommandWithoutReceive(FpuCommands command)
        {
            return ExecuteCommandWithoutReceive(command, null);
        }

        public ExecutedCommandResult ExecuteCommandWithoutReceive(FpuCommands command, object[] parameters)
        {
            return ExecuteCommand(command, parameters, false);
        }

        public ExecutedCommandResult ExecuteCommand(FpuCommands command)
        {
            return ExecuteCommand(command, null);
        }

        public ExecutedCommandResult ExecuteCommand(FpuCommands command, bool receiveMessage)
        {
            return ExecuteCommand(command, null, receiveMessage);
        }

        public ExecutedCommandResult ExecuteCommand(FpuCommands command, object[] parameters)
        {
            return ExecuteCommand(command, parameters, true);
        }



        /// <summary>
        /// FPU DLLine ait bir komutu çalıştırmak için kullanılır
        /// </summary>
        /// <param name="command">DLL methodu örn: __PrintXRepot için FpuCommands.PrintXReport kullanılır </param>
        /// <param name="parameters">DLL methoduna ait paramterlere. (Method parametreleri için Imports.cs klasöründeki ilgili methot parametrelerine bakılır)</param>
        /// <param name="receiveMessage">Eğer DLL methodu işlem sonucunda FPUdan data bekleniyorsa bu değer True olmalıdır. True olduğunda FPUdan data gelene kadar işlem FPU_TIMEOUT süresi kadar beklenir.</param>
        /// <returns></returns>
        public ExecutedCommandResult ExecuteCommand(FpuCommands command, object[] parameters, bool receiveMessage)
        {
            try
            {
                Logger.Info(string.Format("Received Command: {0}", command));
                bool openFpuWithoutPrinter = false;
                openFpuWithoutPrinter = command == FpuCommands.Open || command == FpuCommands.Config || command == FpuCommands.Active || command == FpuCommands.ReciveFPUStatus;
                //command kontrolü yapılacak
                #region USB hatası için eklendi. Pinnacle çözüm bulamayınca böyle bir çözüme gittim. Daha dinamik hale getirilebilir.
                if (this._ConnMode == 2)
                {
                    Imports.__Config("com5", 2, this._DeviceIndex);
                    Imports.__Open();
                    Imports.__Active();
                }
                #endregion

                #region Kağıt Senaryoları Eklenecek
                //_PrinterOperation = new PrintOperations();

                var printerProoblem = _PrinterOperation.yaziciKagitKontrol();

                if (!printerProoblem && (!_InServiceMode || !_PreServiceMode) && !openFpuWithoutPrinter)
                {
                    return new ExecutedCommandResult { ErrorCode = 1453, Message = "FİŞ YAZICISI İLE İLGİLİ BİR SORUN GÖRÜNÜYOR.. KAĞIT RULOSU TAM YERLEŞTİRİLMEMİŞ VEYA FİŞ YAZICISI ÇALIŞMADIĞI İÇİN HERHANGİ BİR İŞLEM YAPMAYACAKTIR. " };
                }
                else
                {
                    #endregion

                    /* Console.WriteLine("Command: {0} - {1}", command, DateTime.Now.TimeOfDay);*///Bu parametre release de kaldırıldı. önizleme
                    if (_FpuBusy)
                    {
                        Logger.Error(string.Format("Command:{0}, FPU Busy", command));
                        // return new ExecutedCommandResult { ErrorCode = 1001, Message = "FPU Busy" };
                    }

                    try
                    {

                        Logger.Info(string.Format("Command: {0} Processing", command));
                        var importType = typeof(Imports);
                        var methodName = string.Format("__{0}", command);
                        var mi =
                        importType.GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        if (mi == null)
                            throw new ApplicationException(string.Format("Methot bulunamadı. Method Adı: {0}", methodName));
                        var miParamCount = mi.GetParameters().Count();
                        if (miParamCount > 0 && (parameters == null || miParamCount != parameters.Length))
                            throw new ApplicationException(string.Format("{0} methoduna gönderilen parametre hatalı. {1} adet paramtere olmalı", methodName, miParamCount));
                        object methodResult = null;
                        ExecutedCommandResult result = new ExecutedCommandResult { ErrorCode = 0, MethodResult = methodResult };//09.11.2016 errorcode change 0 to _errorcode

                        lock (LockObj)
                        {
                            _FpuBusy = true;
                            Logger.Info(string.Format("FPU DLL Method Invoke: {0}", methodName));
                            methodResult = mi.Invoke(null, parameters);
                            result.MethodResult = methodResult;
                            Logger.Info(string.Format("FPU DLL Method {0} Result: {1}", methodName, methodResult == null ? "null" : methodResult));
                            if (receiveMessage)
                            {
                                Logger.Info(string.Format("Command: {0} Data Receiving", command));
                                result = receiveData(result);
                            }
                            _FpuBusy = false;
                        }
                        #region rest

                        //if (mi.ReturnType == typeof(int))

                        //changebyr
                        if (command == FpuCommands.OpenFiscalReceipt || command == FpuCommands.Registeringsale || command == FpuCommands.OpenNonFiscalReceipt || command == FpuCommands.PrintNonFiscalFreeText)
                        {
                            _IsReceiptOperation = true;
                        }

                        var query = PrintHelper.CommandWithPrint.Where(c => c.Command == command).FirstOrDefault();
                        if (query != null && (int)methodResult == 0)
                        {
                            if (_IsReceiptOperation)
                            {
                                //if (_ReceiptData == null)
                                if (_PrintDataList == null)
                                    _PrintDataList = new List<PrintSettings>();
                                _PrintDataList.Add(new PrintSettings { PrintData = result.Data });



                                //_ReceiptData = new StringBuilder();
                                //_ReceiptData.Append(result.Data);
                            }

                            //if (command == FpuCommands.PrintZReport || command == FpuCommands.PrintXReport)
                            //{
                            //    Thread.Sleep(3000);
                            //}

                            if (command == FpuCommands.CloseFiscalReceipt || command == FpuCommands.CancelFiscalReceipt || command == FpuCommands.CloseNonFiscalReceipt)
                            {
                                _IsReceiptOperation = false;
                            }


                            if (!_IsReceiptOperation)
                            {
                                //var printText = _ReceiptData != null ? _ReceiptData.ToString() : result.Data;

                                if (_PrintDataList != null)
                                {
                                    foreach (var item in _PrintDataList)
                                    {
                                        _PrinterOperation.Print(item.PrintData, false);


                                    }
                                }
                                else
                                {
                                    _PrinterOperation.Print(result.Data, false);

                                }
                                //_PrinterOperation.Print(printText, false);
                                var printOkResult = PrintOK();
                                if (printOkResult.HasError)
                                {
                                    Logger.Fatal(string.Format("PrintOK has error: {0}", printOkResult.Message ?? ""));
                                    return printOkResult;
                                }

                                _PrinterOperation.Print(printOkResult.Data, query.Cut);


                            }
                        }

                        //if (!_IsReceiptOperation && _ReceiptData != null)_PrintDataList
                        //    _ReceiptData = null;
                        if (!_IsReceiptOperation && _PrintDataList != null)
                            _PrintDataList = null;

                        return result;
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        Logger.Fatal(string.Format("ExecuteCommandException Command: {0}", command), ex);
                        return new ExecutedCommandResult { Message = ex.ToString(), ErrorCode = 1003 };
                    }
                    finally
                    {
                        _FpuBusy = false;
                        Logger.Info(string.Format("ExecuteCommandFinish Command: {0}", command));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(string.Format("ExecuteCommandException Command: {0}", command), ex);
                return new ExecutedCommandResult { ErrorCode = 1002, Message = ex.ToString() };
            }
        }

        public void openWaitForm()
        {
            System.Windows.MessageBox.Show("sss");
        }
        //TODO: ne iş yaptığı öğrenilecek
        // ilk bulgu: mali alanlara yazdıgında fiş sonu bilgisi dönüyor
        private ExecutedCommandResult PrintOK()
        {
            var methodResult = Imports.__PrintOKorInterrupt("O");
            if (methodResult != 0)
            {
                throw new ApplicationException("PrintOK methodResult" + methodResult);
            }
            return receiveData(null);
        }
        #endregion

        /// <summary>
        /// FPU dan gelen dataları alır
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private ExecutedCommandResult receiveData(ExecutedCommandResult result)
        {
            Logger.Info("Data Waiting");
            var s = new Stopwatch();
            s.Start();
            while (_FpuBusy)
            {
                //App.Current.Dispatcher.Invoke(new Action(() => { }));
                if (s.ElapsedMilliseconds >= FPU_TIMEOUT)
                {
                    Logger.Fatal(string.Format("FPUTimeOut"));
                    s.Stop();
                    _FpuBusy = false;//12012017 14:29 da fpu timeout hatası için eklendi.
                    return new ExecutedCommandResult { ErrorCode = 100, Message = "FPU TiMEOUT" };
                }

            }
            s.Stop();
            s = null;

            if (result == null)
                result = new ExecutedCommandResult();
            _Data = dataBuilder.ToString();
            result.ErrorCode = _ErrorCode;
            result.Message = _Message;
            result.Data = _Data;
            result.FpuStatusStr = _FPURefStatus;
            if (!string.IsNullOrWhiteSpace(_RefStr))
                result.DataArray = _RefStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            _Data = null;
            _Message = null;
            _ErrorCode = 0;
            _RefStr = null;
            _FPURefStatus = null;
            dataBuilder.Clear();
            Console.WriteLine(result);
            Logger.Info("Data Received");
            return result;
        }


        #region FPU Helper
        private TOnReceiveData fOnReceiveData = null;
        private TDeviceIDFun fDeviceIDFun = null;
        private TDeviceFun fDeviceFun = null;
        private bool _InServiceMode;
        private bool _NeedReboot;
        private bool _PreServiceMode;
        private bool _OpenReceiptFlag;
        private bool _ReprintZRep;
        private bool _ZRepOver24Hour;
        private bool _InvoiceRpt;
        private bool _RmpIsRegister;
        private bool _OpenPortSuccess;
        private bool _AllowPrnZRep;

        private void InitFpu()
        {
            //ExecuteCommandWithoutReceive(FpuCommands.SetProtocolType, new object[] { 0 });---->crv de kullanılıyor

            fOnReceiveData = new TOnReceiveData(OnShowReceiveData);
            fDeviceFun = new TDeviceFun(GetDevice);
            fDeviceIDFun = new TDeviceIDFun(GetDeviceID);
            Imports.__IniOnShowReceiveData(fOnReceiveData);
            if (_ConnMode == 2)
            {
                Imports.__Loadfun2(fDeviceIDFun);
                // GetDeviceID Asenkron çağrılıyor. DeviceID -1den farklı olana kadar bekle..
                while (_DeviceIndex == -1)
                {
                    // Main thread kitlenmesin, eventler çalışssın.
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            else
            {
                Imports.__Loadfun2(fDeviceIDFun);

            }
        }

        private void GetDevice(string sdevice, int i)
        {
            //if (i == 0) listBox3.Items.Clear();
            //if (sdevice.Trim() != "") listBox3.Items.Add(sdevice);
        }

        private void GetDeviceID(int VID, int PID, int index)
        {
            //string s = null;
            //if ((VID != 0) && (PID != 0))
            //{
            //    s = "VID=" + VID.ToString("X4") + ",PID=" + PID.ToString("X4");
            //    listBox3.Items.Add(s);
            //}

            string s = null;
            //listBox3.Items.Clear();
            if ((VID != 0) && (PID != 0))
            {
                s = "VID=" + VID.ToString("X4") + ",PID=" + PID.ToString("X4");
                //listBox3.Items.Add(s);
            }
            if (PID.ToString("X4") == "5752")
            {
                _DeviceIndex = index;
            }

            //if (listBox3.Items.Count == 0)
            //{
            //    //timer1.Enabled = false;
            //    Imports.__Close();
            //    if (!(Imports.__Active()))
            //    {
            //        //OpenPortSuccess = false;
            //        Console.WriteLine("Close port ok0");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Close port fail");
            //    }
            //    //DeviceIndex = -1;
            //}
            //else
            //{
            //    //statusStrip1.Items[0].Text = "";
            //}
        }

        StringBuilder dataBuilder = new StringBuilder();
        /// <summary>
        /// FPU dan gelen tüm işlemler bu fonksiyon aracılığıyla dinlenir.
        /// </summary>
        /// <param name="ReceiveData">Alınan data</param>
        /// <param name="ReceiveLen">Alınan datanın uzunluğu</param>
        /// <param name="FpuStatus">FPU Durumlaru</param>
        /// <param name="ErrorMessage">Hata mesajı</param>
        /// <param name="cmdFlag"></param>
        /// <param name="ipos">Bil</param>
        #region 180220161728
        //private void OnShowReceiveData(IntPtr ReceiveData, int ReceiveLen, string FpuStatus, int ErrorMessage, int cmdFlag = 0, int ipos = 0)//ErrorMessage from dll 
        //{
        //    _FpuBusy = true;
        //    Console.WriteLine("Data Received {0}", DateTime.Now.TimeOfDay);
        //    try
        //    {
        //        int lineByte = 42;//CRV 是32 FPU 是40
        //        int j = 0;
        //        int i = 0;
        //        int RowCount = 0;

        //        Encoding cnEncoding = Encoding.GetEncoding(28599);
        //        Encoding cnEncoding1 = Encoding.GetEncoding(857);
        //        byte[] bt = new Byte[ReceiveLen];
        //        string s = "";
        //        //string s1 = "";
        //        TRecvInfo RecvInfo = (TRecvInfo)Marshal.PtrToStructure(ReceiveData, typeof(TRecvInfo));
        //        for (i = 0; i < ReceiveLen; i++)
        //        {
        //            if (RecvInfo.RecvData[i] == 0x04)
        //            {
        //                j = i + 1;
        //                s = cnEncoding.GetString(bt, 0, i).ToString();
        //                break;
        //            }
        //            else
        //            {
        //                if (RecvInfo.RecvData[i] != 0)
        //                {
        //                    bt[i] = RecvInfo.RecvData[i];
        //                    // b = Convert.ToChar(RecvInfo.RecvData[i]).ToString();
        //                    // s = s + b;
        //                }

        //            }
        //        }
        //        // cmdFlag : 0 => array
        //        // cmdFlag : 1 => string
        //        // cmdFlag : 2 => fpu busy


        //        if (cmdFlag == 2)
        //        {
        //            _Ipos = ipos;
        //            Console.WriteLine(string.Format("_Ipos : {0}", _Ipos));
        //            //statusStrip1.Items[1].Text = "Waiting..." + ipos.ToString("d4");
        //        }
        //        else if (cmdFlag == 4)
        //        {
        //            _Ipos = ipos;
        //            Console.WriteLine(string.Format("_Ipos : {0}", _Ipos));
        //        }
        //        else if (cmdFlag == 0)
        //        {
        //            _RefStr = s;
        //            _FPURefStatus = FpuStatus;
        //            if (FpuStatus.Length > 0)
        //            {
        //                SetFpuStatusFlag(FpuStatus);
        //            }
        //        }

        //        else if (cmdFlag == 1)
        //        {
        //            byte[] FiscalMark = new Byte[42];
        //            if ((s.Length % lineByte) == 0)
        //                RowCount = s.Length / lineByte;
        //            else
        //                RowCount = s.Length / lineByte + 1;
        //            for (i = 0; i < RowCount; i++)
        //            {
        //                var s1 = s.Substring(i * lineByte, lineByte);
        //                if (CheckFiscalCodeStr(s1))
        //                {
        //                    for (int k = 0; k < s1.Length; k++)
        //                    {
        //                        FiscalMark[k] = (byte)s1[k];
        //                    }
        //                    s1 = cnEncoding1.GetString(FiscalMark, 0, lineByte).ToString();
        //                }
        //                dataBuilder.AppendLine(s1);
        //            }
        //        }

        //        _FPURefStatus = FpuStatus;
        //        _ErrorCode = ErrorMessage;//返回的错误代号，需转换成 错误信息
        //        if (_ErrorCode > 0)
        //            _Message = errmsg.GetError("E" + _ErrorCode.ToString("d3")).Trim();
        //        if (FpuStatus.Length > 0)
        //            SetFpuStatusFlag(FpuStatus);

        //        if (_InvoiceRpt)
        //        {
        //            lineByte = 40;
        //            _InvoiceRpt = false;
        //        }

        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //        //_Data = dataBuilder.ToString();
        //        _FpuBusy = false;
        //    }
        //}

        #endregion

        private void OnShowReceiveData(IntPtr ReceiveData, int ReceiveLen, string FpuStatus, int ErrorMessage, int cmdFlag = 0, int ipos = 0)//ErrorMessage from dll 
        {
            Logger.Info("OnShowReceiveData Start");
            _FpuBusy = true;
            try
            {
                int lineByte = 42;//CRV 是32 FPU 是40
                int j = 0;
                int i = 0;
                int RowCount = 0;

                Encoding cnEncoding = Encoding.GetEncoding(28599);
                Encoding cnEncoding1 = Encoding.GetEncoding(857);
                byte[] bt = new Byte[ReceiveLen];
                string s = "";
                //string s1 = "";
                TRecvInfo RecvInfo = (TRecvInfo)Marshal.PtrToStructure(ReceiveData, typeof(TRecvInfo));
                for (i = 0; i < ReceiveLen; i++)
                {
                    if (RecvInfo.RecvData[i] == 0x04)
                    {
                        j = i + 1;
                        s = cnEncoding.GetString(bt, 0, i).ToString();
                        break;
                    }
                    else
                    {
                        if (RecvInfo.RecvData[i] != 0)
                        {
                            bt[i] = RecvInfo.RecvData[i];
                            // b = Convert.ToChar(RecvInfo.RecvData[i]).ToString();
                            // s = s + b;
                        }

                    }
                }
                // cmdFlag : 0 => array
                // cmdFlag : 1 => string
                // cmdFlag : 2 => fpu busy
                // cmdFlag : 3 => Cihaz kapandı
                // cmdFlag : 4 => fpu bekleniyor  ?? 2den farkı ne?? 
                // cmdFlag : 7 => Cihaz değişti ?? ne demekse bu
                if (cmdFlag == 7)
                {
                    Console.WriteLine("Device Change");
                    // statusStrip1.Items[0].Text = "Begin Close port";
                    try
                    {
                        Imports.__Close();
                        if (!(Imports.__Active()))
                        {
                            Console.WriteLine("Close port ok 3");
                        }
                        else
                        {
                            Console.WriteLine("Close port fail");
                        }
                        //NeedReboot = false;

                        //TODO: yeniden başlatma için uyarı vs verilecek mi? 
                    }
                    catch
                    {
                    }

                }

                else if (cmdFlag == 3)
                {
                    Console.WriteLine("Power down.");
                    // statusStrip1.Items[0].Text = "Begin Close port";
                    Imports.__Close();
                    //listBox3.Items.Clear();
                    if (!(Imports.__Active()))
                    {
                        Console.WriteLine("Close port ok 3");
                    }
                    else
                    {
                        Console.WriteLine("Close port fail");
                    }
                    //NeedReboot = false;

                    //TODO: yeniden başlatma için uyarı vs verilecek mi? 
                }

                else if (cmdFlag == 2)
                {
                    _Ipos = ipos;
                    Console.WriteLine(string.Format("_Ipos : {0}", _Ipos));
                    //statusStrip1.Items[1].Text = "Waiting..." + ipos.ToString("d4");
                }
                else if (cmdFlag == 4)
                {
                    _Ipos = ipos;
                    Console.WriteLine(string.Format("_Ipos : {0}", _Ipos));
                }

                else if (cmdFlag == 5)
                {
                    //timer4.Enabled = true;
                    s = cnEncoding.GetString(bt, 0, i).ToString();
                    if (s.Length > 0)
                    {
                        //statusStrip1.Items[2].Text = "Waiting..." + ProtocolTime.ToString("d4");
                        //ProtocolTime++;
                        switch (ipos)
                        {
                            case 1:
                                {
                                    //this.dataGridView_QueryPoint.Rows.Clear();
                                    string[] Myarray = s.Split(',');
                                    int GridViewRowCount = (Myarray.Length - 1) / 2;
                                    for (int m = 0; m < GridViewRowCount; m++)
                                    {
                                        int index = 0;

                                        //int index = this.dataGridView_QueryPoint.Rows.Add();
                                        //this.dataGridView_QueryPoint.Rows[index].Cells[0].Value = Myarray[2 * m + 1];
                                        //this.dataGridView_QueryPoint.Rows[index].Cells[1].Value = Myarray[2 * m + 2];
                                        //this.dataGridView_QueryPoint.Rows[index].Cells[2].Value = "0";
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    string[] Myarray = s.Split(',');
                                    int TransactionCount = (Myarray.Length - 1);
                                    //cbx_QueryTranscation.Items.Clear();
                                    //for (int m = 0; m < TransactionCount; m++)
                                    //{
                                    //    cbx_QueryTranscation.Items.Add(Myarray[m + 1]);
                                    //}
                                    //if (cbx_QueryTranscation.Items.Count > 0)
                                    //    cbx_QueryTranscation.SelectedIndex = 0;
                                    break;
                                }
                            case 3:
                                {
                                    string[] Myarray = s.Split(',');
                                    int BankCount = (Myarray.Length - 1);
                                    //listBox8.Items.Clear();
                                    for (int m = 0; m < BankCount; m++)
                                    {
                                        //listBox8.Items.Add(Myarray[m + 1]);
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    //timer4.Enabled = false;
                                    string[] Myarray = s.Split(',');
                                    //listBox1.Items.Add(Myarray[1]);
                                    string appstatus = "App Status";
                                    Imports.__SendProtocolMessage(appstatus);
                                    //ProtocolTime = 1;
                                    break;
                                }

                        }
                        //btn_BankSelection.Visible = ipos == 3;
                        //btn_Transaction.Visible = ipos == 2;
                        //btn_QueryPoint.Visible = ipos == 1;
                    }
                }



                else if (cmdFlag == 0)
                {
                    _RefStr = s;
                    _FPURefStatus = FpuStatus;
                    if (FpuStatus.Length > 0)
                    {
                        SetFpuStatusFlag(FpuStatus);
                    }
                }

                else if (cmdFlag == 1)
                {
                    byte[] FiscalMark = new Byte[42];
                    if ((s.Length % lineByte) == 0)
                        RowCount = s.Length / lineByte;
                    else
                        RowCount = s.Length / lineByte + 1;
                    for (i = 0; i < RowCount; i++)
                    {
                        var s1 = s.Substring(i * lineByte, lineByte);
                        if (CheckFiscalCodeStr(s1))
                        {
                            for (int k = 0; k < s1.Length; k++)
                            {
                                FiscalMark[k] = (byte)s1[k];
                            }
                            s1 = cnEncoding1.GetString(FiscalMark, 0, lineByte).ToString();
                        }
                        dataBuilder.AppendLine(s1);
                        //Imports.__SavePrintData(s1);//Fiş başlığı kaybolma
                    }
                }

                _FPURefStatus = FpuStatus;
                _ErrorCode = ErrorMessage;//返回的错误代号，需转换成 错误信息
                if (_ErrorCode > 0)
                    _Message = errmsg.GetError("E" + _ErrorCode.ToString("d3")).Trim();
                if (FpuStatus.Length > 0)
                    SetFpuStatusFlag(FpuStatus);

                if (_InvoiceRpt)
                {
                    lineByte = 40;
                    _InvoiceRpt = false;
                }

            }
            catch (Exception ex)
            {
                Logger.Fatal("OnShowReceiveDataException", ex);
                _ErrorCode = 1004;
                _Message = ex.ToString();
            }
            finally
            {
                //_Data = dataBuilder.ToString();
                _FpuBusy = false;
                Logger.Info("OnShowReceiveData End");
            }
        }

        private void SetFpuStatusFlag(string FpuStatus)
        {
            string[] Myarray = new string[84];
            Myarray = FpuStatus.Split(',');
            _InServiceMode = false;

            _OpenReceiptFlag = false;
            _ReprintZRep = false;
            _ZRepOver24Hour = false;
            _PreServiceMode = false;
            _NeedReboot = false;
            _AllowPrnZRep = false;

            for (int i = 0; i < Myarray.Length - 1; i++)
            {
                if (Myarray[i].ToString() == "56") _InServiceMode = true;
                if (Myarray[i].ToString() == "83") _NeedReboot = true;
                if (Myarray[i].ToString() == "63") _InvoiceRpt = true;

                if (Myarray[i].ToString() == "55") _PreServiceMode = true;
                if ((Myarray[i].ToString() == "43") || Myarray[i].ToString() == "44") _OpenReceiptFlag = true;
                if (Myarray[i].ToString() == "91") _ReprintZRep = true;
                if (Myarray[i].ToString() == "64") _ZRepOver24Hour = true;
                if (Myarray[i].ToString() == "102") _AllowPrnZRep = true;

            }
        }

        private bool CheckFiscalCodeStr(string s)
        {
            try
            {
                bool result = false;
                byte b;
                if (s.Length > 0)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        b = (byte)s[i];
                        if ((b == 0XB4) || (b == 0XBD) || (b == 0XC3) || (b == 0XB2))
                        {
                            result = true;
                            break;
                        }
                    }

                }
                return result;
            }
            catch
            {
                return false;
            }
        }



        public void PrintQrCode(string code)
        {
            _PrinterOperation.PrintQrCode(code);
        }
        #endregion

        private int _DeviceIndex = -1;
        private int _ConnMode;
        private bool _PrintMode = true;
    }

    internal class ExecutedCommandResult
    {
        public bool HasError
        {
            get { return ErrorCode != 0; }
        }

        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }

        public string[] DataArray { get; set; }

        public object MethodResult { get; set; }

        public string FpuStatusStr { get; set; }

        public T GetMethodResult<T>()
        {
            return (T)MethodResult;
        }


        List<FpuStatus> _StatusList;
        public List<FpuStatus> StatusList
        {
            get
            {
                if (_StatusList == null)
                    _StatusList = createStatusList(FpuStatusStr) ?? new List<FpuStatus>();
                return _StatusList;
            }
        }

        private List<FpuStatus> createStatusList(string statusStr)
        {
            if (string.IsNullOrWhiteSpace(statusStr))
                return null;

            var array = statusStr.Split(',');

            if (array.Length == 0)
                return null;

            List<FpuStatus> list = Enum.GetValues(typeof(EnumFpuStatus))
    .Cast<EnumFpuStatus>()
    .Select(c => new FpuStatus { Status = c })
    .ToList();

            var statusTrueList = list.Where(c => array.Select(d => Convert.ToInt32(d)).Contains((int)c.Status));

            foreach (var item in statusTrueList)
            {
                item.Flag = true;
            }

            return list;
        }

        public override string ToString()
        {
            return string.Format("ErrorCode: {1}{0}Message:{2}{0}Data:{3}", Environment.NewLine, ErrorCode, Message, Data);
        }

    }
}
