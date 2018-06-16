using HyberShift_CSharp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyberShift_CSharp.ViewModel
{
    public class StatusViewModel: BaseViewModel
    {
        private SocketAPI socketAPI;

        public StatusViewModel(): base()
        {
            socketAPI = SocketAPI.GetInstance();
            socketAPI.PropertyChanged += SocketAPIPropertyChanged;
        }

        private void SocketAPIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
                NotifyChanged("Status");
        }

        public string Status
        {
            get { return socketAPI.Status; }
        }
    }
}
