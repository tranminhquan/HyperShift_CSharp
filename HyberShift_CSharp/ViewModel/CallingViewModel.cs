using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HyberShift_CSharp.Model;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View;
using HyberShift_CSharp.View.Calling;
using HyberShift_CSharp.View.Dialog;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Quobject.SocketIoClientDotNet.Client;

namespace HyberShift_CSharp.ViewModel
{
    public class CallingViewModel: BaseViewModel
    {
        private Socket socket;
        private int flagShowVoiceCall = 0; 
        private object selectedViewModel;
        private RoomModel currentRoom;
        private CallingModel callingModel;

        public DelegateCommand ShowVoiceCallCommand { get; set; }
        public DelegateCommand<RoomModel> RoomChangeCommand { get; set; }
        public DelegateCommand LoadCommand { get; set; }
        public RoomModel CurrentRoom
        {
            get { return currentRoom; }
            set
            {
                currentRoom = value;
                NotifyChanged("CurrentRoom");
                NotifyChanged("RoomName");
                SelectedViewModel = new WaitingCallViewModel(ViewModelNavigator, currentRoom);
            }
        }
        public string RoomName
        {
            get { return currentRoom.Name; }
        }
        public CallingViewModel()
        {
            socket = SocketAPI.GetInstance().GetSocket();
            currentRoom = new RoomModel();
            callingModel = CallingModel.GetInstace(currentRoom);
            RoomChangeCommand = new DelegateCommand<RoomModel>(OnRoomChange);
            LoadCommand = new DelegateCommand(OnLoad);
           
            ShowVoiceCallCommand = new DelegateCommand(ShowVoiceCall);

            HandleSocket();
        }

        public CallingViewModel(object viewmodel): this()
        {
            SelectedViewModel = viewmodel;
        }

        public object SelectedViewModel
        {
            get => selectedViewModel;
            set
            {
                selectedViewModel = value;
                NotifyChanged("SelectedViewModel");
            }
        }

        public void ViewModelNavigator(object obj, params object[] parameters)
        {
            if (obj.ToString() == "WaitingCallViewModel") SelectedViewModel = new WaitingCallViewModel(ViewModelNavigator, parameters);
            if (obj.ToString() == "OnGoingCallViewModel") SelectedViewModel = new OnGoingCallViewModel(ViewModelNavigator, parameters);
        }

        public void ShowVoiceCall()
        {
            if (flagShowVoiceCall == 0)
            {
                //check if user has chosen room
                if (currentRoom.ID == null)
                {
                    new MessageDialog("Empty Room", "Please choose the room you want to call").ShowDialog();
                    return;
                }

                CallingWindow callingWindow = new CallingWindow(currentRoom);
                callingWindow.Show();
            flagShowVoiceCall = 1;

                //emit to server
                JObject data = new JObject();
                data.Add("room_id", currentRoom.ID);
                data.Add("room", JObject.FromObject(currentRoom));
                socket.Emit("new_call", data);
            }
            else
            {
                foreach (Window window in Application.Current.Windows)
                    if (window.Title == "CallingWindow")
                        window.Close();
                flagShowVoiceCall = 0;
            }
        }

        public void OnRoomChange(RoomModel room)
        {
            currentRoom = room;
            callingModel.Room = room;
        }

        private void HandleSocket()
        {
            socket.On("new_call", (arg) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    //check if user is having a call
                    if (callingModel.State == Model.Enum.CallingState.BUSY)
                        return;

                    JObject data = (JObject)arg;
                    string roomId = data.GetValue("room_id").ToString();
                    RoomModel room = data.GetValue("room").ToObject<RoomModel>();
                    callingModel.State = Model.Enum.CallingState.BUSY;

                    ReceiveCallWindow receiveCallWindow = new ReceiveCallWindow(room);
                    receiveCallWindow.Show();
                });

            });

            //socket.On("accept_call", (arg) =>
            //{
            //    Application.Current.Dispatcher.Invoke((Action)delegate
            //    {
            //        callingModel.SendVoice();
            //    });
            //});
        }

        private void OnLoad()
        {
            //Debug.LogOutput("On load command activated-------------");
            //callingModel.Receive();
        }
    }
}
