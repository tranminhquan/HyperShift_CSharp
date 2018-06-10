using System.Collections.ObjectModel;
using System.Linq;
using HyberShift_CSharp.Model.List;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View.SignIn;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;


namespace HyberShift_CSharp.Model
{
    internal class MainModel
    {
        private readonly Socket socket;

        // constructor
        public MainModel()
        {
            socket = SocketAPI.GetInstance().GetSocket();
        }

        // getter and setter

        //methods of model
        public void SignOut()
        {
            SignInPage signInPage = new SignInPage();
            signInPage.Show();
            CloseWindowManager.CloseMainWindow();
        }
    }
}