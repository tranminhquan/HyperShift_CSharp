using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using HyberShift_CSharp.Model;
using HyberShift_CSharp.Model.Interface;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View;
using Prism.Commands;

namespace HyberShift_CSharp.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly LoginModel loginModel;
        private readonly Action<object> navigate;

        // constructor
        public LoginViewModel()
        {
            loginModel = new LoginModel();
            LoginCommand = new DelegateCommand<object>(Login);
        }

        public LoginViewModel(Action<object> navigate) : this()
        {
            this.navigate = navigate;
            NavigateCommand = new DelegateCommand(Navigate);
        }

        // getter and setter
        public DelegateCommand<object> LoginCommand { get; set; }
        public DelegateCommand NavigateCommand { get; set; }

        public string Email
        {
            get => loginModel.InputEmail;
            set => loginModel.InputEmail = Convert.ToString(value);
        }

        public string FloatingPasswordBox
        {
            get => loginModel.InputPassword;
            set => loginModel.InputPassword = Convert.ToString(value);
        }

        public void Login(object parameter)
        {
            var passwordContainer = parameter as IHavePassword;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                FloatingPasswordBox = ConvertToUnsecureString(secureString);
            }

            if (loginModel.IsValidLogin())      
                loginModel.Authentication();
            else
                MessageBox.Show("Email or Password is wrong or Server is Offline. Please try again", "ERROR",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

            //NotifyChanged("attributeX");  // this will automatically update attributeX  
        }

        public void Navigate()
        {
            navigate.Invoke("RegisterViewModel");
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null) return string.Empty;

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}