using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using HyberShift_CSharp.Domain;
using HyberShift_CSharp.Model.List;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View;
using HyberShift_CSharp.View.Dialog;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace HyberShift_CSharp.Model
{
    public class LoginModel
    {
        private bool isAuthenticated;
        private readonly Socket socket;
        ListRoomModel listRoomModel;
        UserInfo userInfo = UserInfo.GetInstance();
        // constructor
        public LoginModel()
        {
            isAuthenticated = false;
            InputEmail = "";
            InputPassword = "";
            listRoomModel = ListRoomModel.GetInstance();
            socket = SocketAPI.GetInstance().GetSocket();
        }

        // getter and setter
        public string InputEmail { get; set; }

        public string InputPassword { get; set; }

        public bool LogIn()
        {
            // TO-DO: use socket to login user

            // if success then return true
            if (IsValidLogin())
            {
                Authentication();
                return true;
            }

            // else return false

            return false;
        }

        public bool IsValidLogin()
        {
            if (!IsValidInput.isValidEmail(InputEmail))
                return false;
            if (!IsValidInput.IsValidPassword(InputPassword))
                return false;
            return true;
        }

        public void Authentication()
        {
            //Convert to JSONObject
            var userjson = new JObject();
            try
            {
                userjson.Add("email", InputEmail);
                userjson.Add("password", InputPassword);
            }
            catch (JsonException e)
            {
                Debug.Log(e.ToString());
            }

            Debug.Log("Email: " + InputEmail + ", Password: " + InputPassword);
            socket.Emit("new_authentication", userjson);

            // [SAMPLE] Method for receiving event from socket server
            HandleOnSocketEvent();
        }

        //[SAMPLE] Method handle "On" event from socket server
        public void HandleOnSocketEvent()
        {
            socket.On("authentication_result", args =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (isAuthenticated) return;

                    isAuthenticated = true;
                    var data = (JObject)args;
                    if (data != null)
                    {
                        Debug.Log("Authentication successed");
                        Debug.Log(data.ToString());

                        // [IMPORTANT] set info for userinfo
                        userInfo.UserId = data.GetValue("id").ToString();
                        var content = (JObject)data.GetValue("content");

                        userInfo.AvatarRef = content.GetValue("avatarstring").ToString();
                        userInfo.FullName = content.GetValue("fullname").ToString();
                        userInfo.Email = content.GetValue("email").ToString();
                        userInfo.Phone = content.GetValue("phone").ToString();

                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            // your code
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();

                            CloseWindowManager.CloseLoginWindow();
                        });
                    }
                    else
                    {
                        isAuthenticated = false;
                        (new MessageDialog("Sign in failed", "Opps! Your email or password is not correct. Please check again")).ShowDialog();
                    }
                });               
            }); 
        }
    }
}