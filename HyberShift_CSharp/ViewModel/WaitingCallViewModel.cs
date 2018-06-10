using HyberShift_CSharp.Model;
using HyberShift_CSharp.Utilities;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HyberShift_CSharp.ViewModel
{
    public class WaitingCallViewModel: BaseViewModel
    {
        private readonly Action<object, object[]> navigate;
        private Socket socket;
        private RoomModel currentRoom;
        public DelegateCommand HangupCommand { get; set; }
        public DelegateCommand AcceptCommand { get; set; }
        public WaitingCallViewModel(): base()
        {
            socket = SocketAPI.GetInstance().GetSocket();
            currentRoom = new RoomModel();
            HangupCommand = new DelegateCommand(Exit);
            AcceptCommand = new DelegateCommand(AcceptCall);

            HandleSocket();
        }

        public WaitingCallViewModel(Action<object, object[]> navigate, params object[] parameters): this()
        {
            this.navigate = navigate;

            currentRoom = (RoomModel)parameters[0];
        }

        private void HandleSocket()
        {
            socket.On("end_call", () =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Exit();
                    CallingModel.GetInstace().State = Model.Enum.CallingState.FREE;
                });
            });
        }

        private void Exit()
        {
            CallingModel.GetInstace().State = Model.Enum.CallingState.FREE;

            foreach (Window window in Application.Current.Windows)
                if (window.Title == "ReceiveCallWindow")
                    window.Close();
        }

        private void AcceptCall()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                // emit to server accept call
                JObject data = new JObject();
                data.Add("room_id", currentRoom.ID);
                data.Add("user", JObject.FromObject(UserInfo.GetInstance()));
                socket.Emit("accept_call", data);

                CallingModel.GetInstace().State = Model.Enum.CallingState.BUSY;
                navigate.Invoke("OnGoingCallViewModel", new object[] { currentRoom });
            });            
        }
    }
}
