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

namespace HyberShift_CSharp.View.Task
{
    /// <summary>
    /// Interaction logic for CreateTaskDialog.xaml
    /// </summary>
    public partial class CreateTaskDialog : Window
    {
        public CreateTaskDialog()
        {
            InitializeComponent();
        }

        public CreateTaskDialog(RoomModel currentRoom): this()
        {
            ((CreateTaskViewModel)DataContext).CurrentRoom = currentRoom;
            ((CreateTaskViewModel)DataContext).ListMembers = currentRoom.Members;
        }
    }
}
