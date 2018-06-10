using System.Security;
using System.Windows.Controls;
using HyberShift_CSharp.Model.Interface;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.ViewModel;
using Newtonsoft.Json;

namespace HyberShift_CSharp.View.SignIn
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInControl : UserControl, IHavePassword
    {
        public SignInControl()
        {
            InitializeComponent();
            //DataContext = new LoginViewModel();
        }

        public SecureString Password
        {
            get
            {
                return FloatingPasswordBox.SecurePassword;
            }
        }

        public SecureString ConfirmPassword { get; }
    }
}
