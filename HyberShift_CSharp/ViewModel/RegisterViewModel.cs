using System;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Media.Imaging;
using HyberShift_CSharp.Model;
using HyberShift_CSharp.Model.Interface;
using HyberShift_CSharp.Utilities;
using Newtonsoft.Json.Linq;
using Prism.Commands;

namespace HyberShift_CSharp.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly Action<object> navigate;
        private readonly RegisterModel registerModel;
        private IDialogService dialogService;

        // constructor
        public RegisterViewModel()
        {
            registerModel = new RegisterModel();
            RegisterCommand = new DelegateCommand<object>(Register);
            OpenImageCommand = new DelegateCommand(OpenImage);
            dialogService = new DialogService();
        }

        public RegisterViewModel(Action<object> navigate) : this()
        {
            this.navigate = navigate;
            NavigateCommand = new DelegateCommand(Navigate);
        }

        // getter and setter
        public DelegateCommand NavigateCommand { get; set; }
        public DelegateCommand<object> RegisterCommand { get; set; }
        public DelegateCommand OpenImageCommand { get; set; }
        public BitmapImage Avatar { get; set; }
        public string Email
        {
            get => registerModel.Info.Email;
            set => registerModel.Info.Email = Convert.ToString(value);
        }

        public string FloatingPasswordBox
        {
            get => registerModel.Info.Password;
            set => registerModel.Info.Password = Convert.ToString(value);
        }

        public string FloatingConfirmPasswordBox
        {
            get => registerModel.ConfirmPassword;
            set => registerModel.ConfirmPassword = Convert.ToString(value);
        }

        public string Phone
        {
            get => registerModel.Info.Phone;
            set => registerModel.Info.Phone = Convert.ToString(value);
        }

        public string Name
        {
            get => registerModel.Info.FullName;
            set => registerModel.Info.FullName = Convert.ToString(value);
        }

        public string ImgAvatarRef
        {
            get => registerModel.Info.AvatarRef;
            set => registerModel.Info.AvatarRef = Convert.ToString(value);
        }

        public void Register(object parameter)
        {
            var passwordContainer = parameter as IHavePassword;
            if (passwordContainer != null)
            {
                var secureStringPassword = passwordContainer.Password;
                var secureStringConfirmPassword = passwordContainer.ConfirmPassword;
                FloatingPasswordBox = ConvertToUnsecureString(secureStringPassword);
                FloatingConfirmPasswordBox = ConvertToUnsecureString(secureStringConfirmPassword);
            }

            if (registerModel.Register())
            {
                registerModel.PushData();
                MessageBox.Show("Your account is created!", "Congrats!", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Something is wrong. Please check again", "ERROR", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            //NotifyChanged("attributeX");  // this will automatically update attributeX  
        }

        public void OpenImage()
        {
            SystemSounds.Exclamation.Play();

            string path = dialogService.OpenFile("Choose image file", "Image (.png ,.jpg)|*.png;*.jpg");

            if (path == "")
                return;

            string encodstring = ImageUtils.CopyImageToBase64String(Image.FromFile(path));
            registerModel.Info.AvatarRef = encodstring;

            Avatar = ImageUtils.Base64StringToBitmapSource(encodstring);
            NotifyChanged("Avatar");
        }

        public void Navigate()
        {
            navigate.Invoke("LoginViewModel");
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