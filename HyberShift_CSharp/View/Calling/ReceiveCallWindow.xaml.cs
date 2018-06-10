using HyberShift_CSharp.Model;
using HyberShift_CSharp.ViewModel;
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

namespace HyberShift_CSharp.View.Calling
{
    /// <summary>
    /// Interaction logic for ReceiveCallWindow.xaml
    /// </summary>
    public partial class ReceiveCallWindow : Window
    {
        public ReceiveCallWindow()
        {
            InitializeComponent();
        }

        public ReceiveCallWindow(RoomModel room): this()
        {
            ((CallingViewModel)DataContext).CurrentRoom = room;     
        }
    }
}
