using Quobject.SocketIoClientDotNet.Client;
using System.ComponentModel;

// SocketAPI class
// Created: TM Quan 19/4
// This class is use for connecting with server

namespace HyberShift_CSharp.Utilities
{
    public class SocketAPI: INotifyPropertyChanged
    {
        private static Socket socket;
        private static SocketAPI instance;
        private static bool isConnected;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Status { get; set; }

        public SocketAPI()
        {
            socket = IO.Socket("http://hybershift-server-helloqwert12.c9users.io/");
            //socket = IO.Socket("https://hypershift-server.herokuapp.com/");
            isConnected = false;
            EstablishConnection();
            Status = "Developed with passion by Absoluke team";
        }

        public static SocketAPI GetInstance()
        {
            if (instance == null)
            {
                instance = new SocketAPI();
                instance.Connect();
            }
            return instance;
        }

        public Socket GetSocket()
        {
            return socket;
        }

        public void Connect()
        {
            if (!isConnected)
            {
                socket.Connect();
                isConnected = true;
            }
        }

        public void Disconnect()
        {
            if (isConnected)
            {
                socket.Disconnect();
                socket.Close();
                isConnected = false;
            }
        }

        public void EstablishConnection()
        {
            socket.
                On(Socket.EVENT_CONNECT, () =>
                {
                    Debug.LogOutput("Client connected to server");
                    Status = "Connected to server";
                    NotifyChange("Status");
                }).

                On(Socket.EVENT_DISCONNECT, () =>
                {
                    Debug.Log("Client disconnected to server");
                    //socket.Close();
                    Debug.LogOutput("Client disconnected to server");
                    Status = "Disconnected";
                    NotifyChange("Status");
                }).
                On(Socket.EVENT_CONNECT_ERROR, () =>
                {
                    Debug.LogOutput("Connect error");
                    Status = "Error when connecting";
                    NotifyChange("Status");
                }).
                On(Socket.EVENT_CONNECT_TIMEOUT, () =>
                {
                    Debug.LogOutput("Connect timeout");
                    Status = "Connect timeout";
                    NotifyChange("Status");
                }).
                On(Socket.EVENT_RECONNECT, () =>
                {
                    Debug.LogOutput("Reconnect");
                    Status = "Reconnect to server";
                    NotifyChange("Status");
                }).
                On(Socket.EVENT_RECONNECTING, () =>
                {
                    Debug.LogOutput("Reconnecting");
                    Status = "Reconnecting . . .";
                    NotifyChange("Status");
                }).
                On(Socket.EVENT_RECONNECT_ATTEMPT, () =>
                {
                    Debug.LogOutput("Reconnect attempt");
                    Status = "Attempt to reconnect . . .";
                    NotifyChange("Status");
                }).
                On(Socket.EVENT_RECONNECT_FAILED, () =>
                {
                    Debug.LogOutput("Reconnect failed");
                    Status = "Reconnect failed";
                    NotifyChange("Status");
                });
        }

        private void NotifyChange(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // delegate to create event
        //public void AddEvent(string eventName, Action method)
        //{
        //    socket.On(eventName, method);
        //}

        //public void AddEvent(string eventName, Action<object> method)
        //{
        //    socket.On(eventName, method);
        //}
    }
}