using System.Windows;
using HyberShift_CSharp.Utilities;

namespace HyberShift_CSharp.View.SignIn
{
    /// <summary>
    ///     Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInPage
    {
        public SignInPage()
        {
            InitializeComponent();
            var socketAPI = SocketAPI.GetInstance();
        }
    }
}