using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using HyberShift_CSharp.Model;
using HyberShift_CSharp.View.SignIn;
using Prism.Commands;

namespace HyberShift_CSharp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MainModel mainModel;

        // getter and setter
        public DelegateCommand SignOutCommand { get; set; }


        // constructor
        public MainViewModel()
        {
            mainModel = new MainModel();
            SignOutCommand = new DelegateCommand(SignOut);
        }

        public void SignOut()
        {
            // TO-DO: Xử lý quay lại màn hình Login
            mainModel.SignOut();
        }  
    }
}
