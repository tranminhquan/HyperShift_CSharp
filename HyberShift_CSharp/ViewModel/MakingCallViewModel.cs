using HyberShift_CSharp.Model;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View.Calling;
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
    public class MakingCallViewModel: BaseViewModel
    {
        private Socket socket;
        private RoomModel currentRoom;
        private CallingModel callingModel;
        private readonly Action<object, object[]> navigate;
        public DelegateCommand HangupCommand { get; set; }


        public MakingCallViewModel(): base()
        {
            socket = SocketAPI.GetInstance().GetSocket();
            currentRoom = new RoomModel();
            callingModel = CallingModel.GetInstace();
            HangupCommand = new DelegateCommand(Hangup);
            HandleSocket();
        }

        public MakingCallViewModel(Action<object, object[]> navigate, params object[] parameters): this()
        {
            this.navigate = navigate;
            currentRoom = (RoomModel)parameters[0];
        }

        private void HandleSocket()
        {
            socket.On("accept_call", (arg) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (callingModel.State == Model.Enum.CallingState.BUSY)
                        return;

                    JObject data = (JObject)arg;
                    callingModel.State = Model.Enum.CallingState.BUSY;
                    navigate.Invoke("OnGoingCallViewModel", new object[] { currentRoom });
                });
            });
        }

        private void Hangup()
        {
            //emit end call to server
            socket.Emit("end_call", currentRoom.ID);
            callingModel.State = Model.Enum.CallingState.FREE;

            foreach (Window window in Application.Current.Windows)
                if (window.Title == "CallingWindow")
                    window.Close();
        }
    }
}
