using HyberShift_CSharp.Model;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View.Dialog;
using Newtonsoft.Json.Linq;
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

namespace HyberShift_CSharp.View
{
    /// <summary>
    /// Interaction logic for AddMemberDialog.xaml
    /// </summary>
    public partial class AddMemberDialog : Window
    {
        private RoomModel currentRoom;
        public AddMemberDialog()
        {
            InitializeComponent();
            currentRoom = new RoomModel();

            //SocketAPI.GetInstance().GetSocket().On("add_member_result", () =>
            //{
            //    tbNoice.Text = "Add member to room successfully!";
            //});
        }

        public AddMemberDialog(RoomModel room): this()
        {
            currentRoom = room;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtMember.Text.Length == 0)
            {
                (new MessageDialog("Empty member", "Insert at least one member")).ShowDialog();
                return;
            }

            string[] separators = { ",", "!", "?", ";", ":", " " };
            string[] members = txtMember.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            JArray jarrayMember = new JArray();
            foreach (string mem in members)
            {
                jarrayMember.Add(mem);
            }

            JObject data = new JObject();
            data.Add("room_id", currentRoom.ID);
            data.Add("room_name", currentRoom.Name);
            data.Add("members", jarrayMember);

            SocketAPI.GetInstance().GetSocket().Emit("add_members", data);
        }
    }
}
