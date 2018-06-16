using HyberShift_CSharp.View.SignIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HyberShift_CSharp.View
{
    /// <summary>
    /// Interaction logic for IntroWindow.xaml
    /// </summary>
    public partial class IntroWindow : Window
    {
        private DispatcherTimer timer;
        public IntroWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(SwitchView);
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
        }

        private void SwitchView(object sender, EventArgs e)
        {
            timer.Stop();
            SignInPage signInPage = new SignInPage();
            signInPage.Show();       

            foreach (Window window in Application.Current.Windows)
                if (window.Title == "IntroWindow")
                    window.Close();
        }
    }
}
