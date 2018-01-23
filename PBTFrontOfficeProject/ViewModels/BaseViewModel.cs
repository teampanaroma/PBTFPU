using PBTFrontOfficeProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PBTFrontOfficeProject.ViewModels
{
    public class BaseViewModel:INotifyPropertyChanged
    {
        public void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public FpuService FPUService
        {
            get { return AppService.FpuService; }
        }
    }
}
