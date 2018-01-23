using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Panaroma.FPU.Core
{
    public struct TRecvInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] RecvData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] StateData;
    }

    public class ErrorMsg
    {
        public struct TErrorCode
        {
            public string ErrorId;
            public string Description;
            public TErrorCode(string aErrorid, string aDesc)
            {
                ErrorId = aErrorid;
                Description = aDesc;
            }
        }

        private TErrorCode[] ErrorList = new TErrorCode[250];

        public ErrorMsg()
        {
            string filename = Directory.GetCurrentDirectory() + "\\ErrorCode.txt";

            if (!File.Exists(filename))
            {
                return;
            }

            FileStream fs;
            fs = new FileStream(filename, FileMode.Open);
            fs.Position = 0;
            StreamReader sr = new StreamReader(fs);
            int i = -1;
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                i++;
                if (i == 0) continue; //skip line 1 [head name]

                if (line.Trim() == "") continue;
                string[] aa = line.Split('\t');
                ErrorList[i - 1].ErrorId = aa[0];
                ErrorList[i - 1].Description = aa[1];

            }
            fs.Close();


        }

        public string GetError(string ErrorCode)
        {
            string s = "";
            for (int i = 0; i < ErrorList.Length; i++)
            {
                if (ErrorList[i].ErrorId == null) break;
                if (ErrorList[i].ErrorId.ToLower() == ErrorCode.ToLower())
                {
                    s = ErrorList[i].ErrorId + ":" + ErrorList[i].Description;//
                    break;
                }
            }
            return s;

        }
    }
}
