using HyberShift_CSharp.Utilities;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace HyberShift_CSharp.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        bool _stateClosed = true;

        public MainWindow()
        {
            InitializeComponent();
            //var socketAPI = SocketAPI.GetInstance();
            //socketAPI.Connect();
        }

        private void ButtonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (_stateClosed)
            {
                Storyboard sb = this.FindResource("OpenMenu") as Storyboard;
                sb.Begin();
            }
            else
            {
                Storyboard sb = this.FindResource("CloseMenu") as Storyboard;
                sb.Begin();
            }

            _stateClosed = !_stateClosed;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Socket socket = SocketAPI.GetInstance().GetSocket();
            SocketAPI socketAPI = SocketAPI.GetInstance();
            socketAPI.Disconnect();
            base.OnClosing(e);
            App.Current.Shutdown();
        }
    }
}