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

namespace HyberShift_CSharp.View.Dialog
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : Window
    {
        public MessageDialog()
        {
            InitializeComponent();
        }

        public MessageDialog(string title, string content): this()
        {
            tbTitle.Text = title;
            tbContent.Text = content;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "MessageDialog")
                    window.Close();
        }
    }
}
