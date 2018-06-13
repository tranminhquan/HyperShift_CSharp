using HyberShift_CSharp.Utilities;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace HyberShift_CSharp.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        bool _stateClosed = true;
        //NotifyIcon notifyIcon;
        //WindowState windowState;

        public MainWindow()
        {
            InitializeComponent();
            //var socketAPI = SocketAPI.GetInstance();
            //socketAPI.Connect();

            //notifyIcon = new System.Windows.Forms.NotifyIcon();
            //notifyIcon.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            //notifyIcon.BalloonTipTitle = "The App";
            //notifyIcon.Text = "The App";
            //notifyIcon.Icon = new System.Drawing.Icon("TheAppIcon.ico");
            //notifyIcon.Click += new EventHandler(notifyIcon_Click);
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

        //private void AcrylicWindow_Closing(object sender, CancelEventArgs e)
        //{
        //    //notifyIcon.Dispose();
        //    //notifyIcon = null;
        //}

        //private void AcrylicWindow_StateChanged(object sender, EventArgs e)
        //{
        //   //if (WindowState == WindowState.Minimized)
        //    //{
        //    //    Hide();
        //    //    if (notifyIcon != null)
        //    //        notifyIcon.ShowBalloonTip(2000);
        //    //}
        //    //else
        //    //    windowState = WindowState;
        //}

        //private void AcrylicWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    //CheckTrayIcon();
        //}

        //private void notifyIcon_Click(object sender, EventArgs e)
        //{
        //    //Show();
        //    //WindowState = WindowState;
        //}

        //private void CheckTrayIcon()
        //{
        //    //ShowTrayIcon(!IsVisible);
        //}

        //private void ShowTrayIcon(bool show)
        //{
        //    //if (notifyIcon != null)
        //    //    notifyIcon.Visible = show;
        //}
    }
}