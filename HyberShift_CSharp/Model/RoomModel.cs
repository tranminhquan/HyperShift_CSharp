using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HyberShift_CSharp.Model
{
    public class RoomModel: INotifyPropertyChanged
    {
        public RoomModel()
        {
            Members = new ObservableCollection<string>();
            
            //hasNewMessage = false;
        }

        public RoomModel(string id, string name, ObservableCollection<string> members)
        {
            ID = id;
            Name = name;
            Members = members;

            DisplayNewMessage = "Hidden";
            this.NotifyChanged("DisplayNewMessage");
        }

        public string ID { get; set; }

        public string Name { get; set; }

        public string DisplayNewMessage { get; set; }

        public ObservableCollection<string> Members { get; set; }

        public string ListMembers
        {
            get
            {
                string temp = "";
                foreach(string mem in Members)
                {
                    temp += mem + " ";
                }
                return temp;
            }
        }

        public bool HasNewMessage { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddMembers(string member)
        {
            Members.Add(member);
        }

        public int GetMemebersCount()
        {
            return Members.Count;
        }

        public void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}