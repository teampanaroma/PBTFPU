using Panaroma.FPU.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaroma.FPU
{
    public class FpuCommandsModel
    {
        private FpuCommands _Command;

        internal FpuCommands Command
        {
            get { return _Command; }
            set { _Command = value; }
        }
        private bool _Cut;

        public bool Cut
        {
            get { return _Cut; }
            set { _Cut = value; }
        }
        
    }
}
