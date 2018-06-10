using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyberShift_CSharp.Model;

namespace HyberShift_CSharp.ViewModel
{
    public class RoomViewModel : BaseViewModel
    {
        private RoomModel roomModel;
        // constructor
        public RoomViewModel()
        {
            roomModel = new RoomModel();
            Name = "";
            Members = "";
        }

        public RoomViewModel(string id, string name, ObservableCollection<string> members)
        {
            roomModel = new RoomModel(id, name, members);
        }

        // getter and setter
        public string ID
        {
            get { return roomModel.ID; }
            set { roomModel.ID = value; NotifyChanged("ID"); }
        }

        public string Name
        {
            get { return roomModel.Name; }
            set
            {
                if (Name == null)
                    roomModel.Name = "Empty Name";
                else
                    roomModel.Name = value;
                NotifyChanged("Name");
            }
        }

        public string Members
        {
            get
            {
                string temp = "";
                foreach(string mem in roomModel.Members)
                {
                    temp += mem + " ";
                }
                return temp;
            }
            set
            {

            }
        }

    }
}
