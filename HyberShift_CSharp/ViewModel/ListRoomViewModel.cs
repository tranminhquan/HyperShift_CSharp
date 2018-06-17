using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using HyberShift_CSharp.Model;
using HyberShift_CSharp.Model.List;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View;
using HyberShift_CSharp.View.Dialog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Quobject.SocketIoClientDotNet.Client;


namespace HyberShift_CSharp.ViewModel
{
    public class ListRoomViewModel : BaseViewModel
    {
        private readonly ListRoomModel listRoomModel;
        private Socket socket;
        private RoomModel currentRoom;
        // constructor
        public ListRoomViewModel()
        {
            socket = SocketAPI.GetInstance().GetSocket();
            currentRoom = new RoomModel();
            listRoomModel = ListRoomModel.GetInstance();      
            ItemSelectedCommand = new DelegateCommand<RoomModel>(HandleSelectedItem);
            AddMemberCommand = new DelegateCommand(AddMember);
            HandleSocket();
        }

        // getter and setter
        public DelegateCommand<RoomModel> ItemSelectedCommand { get; set; } //Command use for SelectedChanged event of listview (or listbox, ...)
        public DelegateCommand AddMemberCommand { get; set; }
        public ObservableCollection<RoomModel> ListRoom
        {
            get { return listRoomModel.List; }
            set
            {
                listRoomModel.List = value;
                NotifyChanged("ListRoom");
            }
        }

        // method
        private void HandleSelectedItem(RoomModel obj)
        {
            currentRoom = obj;

            //emit to server to get message
            socket.Emit("room_change", obj.ID);

            int index = ListRoomModel.GetInstance().GetIndexByValue("ID", obj.ID);
            ListRoomModel.GetInstance().List[index].DisplayNewMessage = "Hidden";
            ListRoomModel.GetInstance().List[index].NotifyChanged("DisplayNewMessage");
            ListRoomModel.GetInstance().NotifyChanged("List");

            //clear data in list message
            ListMessageModel.GetInstance().Clear();
            // clear list task
            ListTaskModel.GetInstance().Clear();
        }

        private void HandleSocket()
        {
            // request server for sending list room of user (now not depend on authentication anymore!!!)
            socket.Emit("room_request", UserInfo.GetInstance().UserId);
            Debug.LogOutput("Request room with userid: " + UserInfo.GetInstance().UserId);

            socket.On("room_created", (args) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    JObject obj = (JObject)args;
                    try
                    {
                        string roomId = obj.GetValue("room_id").ToString();
                        string roomName = obj.GetValue("room_name").ToString();
                        JArray listjson = (JArray)obj.GetValue("members");
                        if (listjson.Count == 0)
                            return;

                        ObservableCollection<string> members = new ObservableCollection<string>();
                        for (int i = 0; i < listjson.Count; i++)
                        {
                            members.Add(listjson[i].ToString());
                        }
                        RoomModel room = new RoomModel(roomId, roomName, members);
                        listRoomModel.AddWithCheck(room, "ID");
                    }
                    catch (JsonException e)
                    {
                        // TODO Auto-generated catch block
                        Debug.Log("ListRoomModel exception >> " + e);
                        Debug.LogOutput("ListRoomModel exception >> " + e);
                    }
                });
            });

            socket.On("user_added", () =>
            {
                Debug.LogOutput("Other user add you to room");
                socket.Emit("room_request", UserInfo.GetInstance().UserId);
            });

            
        }

        private void AddMember()
        {
            AddMemberDialog addMemberDialog = new AddMemberDialog(currentRoom);
            addMemberDialog.ShowDialog();
        }
    }
}