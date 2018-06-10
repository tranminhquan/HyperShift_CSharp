using System;
using System.Collections.Generic;
using System.Windows;
using HyberShift_CSharp.Model;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View;
using Prism.Commands;

namespace HyberShift_CSharp.ViewModel
{
    internal class CreateRoomViewModel : BaseViewModel
    {
        private int flagShowCreateRoom = 0; //flag for open, close CreateRoom View
        private readonly RoomModel room;
        private readonly CreateRoomModel createRoomModel;

        private string[] separators = {",", "!", "?", ";", ":", " "};

        // constructor
        public CreateRoomViewModel()
        {
            createRoomModel = new CreateRoomModel();
            room = new RoomModel();
            CreateRoomCommand = new DelegateCommand(CreateRoom);
            ShowCreateRoomCommand = new DelegateCommand(ShowCreateRoom);
        }

        // getter and setter
        public DelegateCommand CreateRoomCommand { get; set; }
        public DelegateCommand ShowCreateRoomCommand { get; set; }
        //public string ID
        //{
        //    get { return room.ID; }
        //    set { room.ID = value; NotifyChanged("ID"); }
        //}
        public string RoomName
        {
            get { return createRoomModel.InputRoomName; }
            set
            {
                createRoomModel.InputRoomName = value;
                NotifyChanged("RoomName");
            }
        }

        public string Email { get; set; }

        // method or command

        public void CreateRoom()
        {
            createRoomModel.InputEmailMember = Email.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (createRoomModel.IsValidCreateRoom())
                createRoomModel.CreateRoom();
            else
            {
                MessageBox.Show("Something is wrong. Please try again", "Create room failed",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void ShowCreateRoom()
        {
            if (flagShowCreateRoom == 0)
            {
                View.CreateRoom createRoom = new CreateRoom();
                createRoom.Show();
                flagShowCreateRoom = 1;
            }
            else if (flagShowCreateRoom == 1)
            {
                CloseWindowManager.CloseCreateRoomWindow();
                flagShowCreateRoom = 0;
            }
        }
    }
}