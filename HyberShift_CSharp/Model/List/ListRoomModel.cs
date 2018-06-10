using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using HyberShift_CSharp.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace HyberShift_CSharp.Model.List
{
    public class ListRoomModel : BaseList<RoomModel>, INotifyPropertyChanged
    {
        private static ListRoomModel instance;

        public event PropertyChangedEventHandler PropertyChanged;

        // constructor
        public ListRoomModel(): base()
        {
           
        }

        // getter and setter
        public ObservableCollection<object> IDList => GetCollectionOfField("ID");

        public ObservableCollection<object> NameList => GetCollectionOfField("Name");

        public ObservableCollection<object> MemberList => GetCollectionOfField("Members");

        // singleton method
        public static ListRoomModel GetInstance()
        {
            if (instance == null)
                instance = new ListRoomModel();
            return instance;
        }

        //method
        public RoomModel GetRoomFromName(string roomName)
        {
            return GetFirstObjectByValue("Name", roomName);
        }

        public ObservableCollection<object> GetMembersFrom(string roomName)
        {
            return GetCollectionOfFieldByValue("Members", "Name", roomName);
        }

        public RoomModel GetRoomById(string id)
        {
            return GetFirstObjectByValue("ID", id);
        }

        public int GetIndexOfRoom(string id)
        {
            return GetIndexByValue("ID", id);
        }

        public ObservableCollection<string> GetMembersFrom(int indexRoom)
        {
            return GetFieldValueByIndex("Members", indexRoom);
        }

        public override string ToString()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            foreach(RoomModel room in List)
            {
                result.Add("Room ID: " + room.ID + " | Room Name: " + room.Name + " | Members: " + room.Members);
            }
            return " ";
        }

        public void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyListChange()
        {
            NotifyChanged("List");
        }

      

    }
}