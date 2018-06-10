using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HyberShift_CSharp.Model.Interface;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View.Dialog;

namespace HyberShift_CSharp.View.SignIn
{
    /// <summary>
    ///     Interaction logic for SignUpControl.xaml
    /// </summary>
    public partial class SignUpControl : UserControl, IHavePassword
    {
        public SignUpControl()
        {
            InitializeComponent();
        }

        public SecureString Password => FloatingPasswordBox.SecurePassword;

        public SecureString ConfirmPassword => FloatingConfirmPasswordBox.SecurePassword;
    }
}