using HyberShift_CSharp.Model;
using HyberShift_CSharp.Model.Enum;
using HyberShift_CSharp.Model.List;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View.Dialog;
using HyberShift_CSharp.View.Task;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HyberShift_CSharp.ViewModel
{
    public class TaskViewModel: BaseViewModel
    {
        private Socket socket;
        private RoomModel currentRoom;
        private ListTaskModel listTaskModel;

        // getter and setter
        public DelegateCommand CreateTaskCommand { get; set; }
        public DelegateCommand<RoomModel> RoomChangeCommand { get; set; }
        public DelegateCommand UpdateProgressCommand { get; set; }
        public TaskModel SelectedTask { get; set; }
        public ObservableCollection<TaskModel> ListTask
        {
            get { return listTaskModel.List; }
            set
            {
                listTaskModel.List = value;
                NotifyChanged("ListTask");
            }
        }

        
        public TaskViewModel():base()
        {
            socket = SocketAPI.GetInstance().GetSocket();
            listTaskModel = ListTaskModel.GetInstance();
            currentRoom = new RoomModel();

            CreateTaskCommand = new DelegateCommand(CreateTask);
            RoomChangeCommand = new DelegateCommand<RoomModel>(OnRoomChange);
            UpdateProgressCommand = new DelegateCommand(UpdateTask);

            ////test
            //ListTask.Add(new TaskModel("1", "Name 1", "Des 1", DateTime.Now, DateTime.Now, "Per 1", 0.2, Model.Enum.TaskType.TO_DO));
            //ListTask.Add(new TaskModel("2", "Name 2", "Des 2", DateTime.Now, DateTime.Now, "Per 2", 0.5, Model.Enum.TaskType.BACKLOG));
            //ListTask.Add(new TaskModel("3", "Name 3", "Des 3", DateTime.Now, DateTime.Now, "Per 3", 0.7, Model.Enum.TaskType.WARNING));
            //ListTask.Add(new TaskModel("4", "Name 4", "Des 4", DateTime.Now, DateTime.Now, "Per 4", 1, Model.Enum.TaskType.DELAY));

            HandleSocket();
        }

        private void HandleSocket()
        {
            // Handle socket
            socket.On("task_change", (arg) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    JObject data = (JObject)arg;
                    string roomid = data.GetValue("room_id").ToString();
                    if (!currentRoom.ID.Equals(roomid))
                        return;

                    string id = data.GetValue("task_id").ToString();
                    string name = data.GetValue("work").ToString();
                    string description = data.GetValue("description").ToString();
                    string performer = data.GetValue("performer").ToString();
                    DateTime startday = (DateTime)data.GetValue("start_day");
                    DateTime endday = (DateTime)data.GetValue("end_day");
                    TaskType tag = TaskTypeModel.GetTaskType(data.GetValue("tag").ToString());
                    double progress = (double)data.GetValue("progress");

                    listTaskModel.AddWithCheck(new TaskModel(id, name, description, startday, endday, performer, progress, tag), "ID");
                    NotifyChanged("ListTask");
                });             
            });
            
        }

        private void CreateTask()
        {
            CreateTaskDialog createTaskDialog = new CreateTaskDialog(currentRoom);
            createTaskDialog.ShowDialog();
        }

        private void OnRoomChange(RoomModel obj)
        {
            currentRoom = obj;
        }

        private void UpdateTask()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                Debug.LogOutput(SelectedTask.Progress.ToString());
                Debug.LogOutput(currentRoom.ID);
                Debug.LogOutput(SelectedTask.ID);
                JObject data = new JObject();
                data.Add("room_id", currentRoom.ID);
                data.Add("task_id", SelectedTask.ID);
                JObject dataUpdate = new JObject();
                dataUpdate.Add("progress", SelectedTask.SliderProgress);
                if (SelectedTask.SliderProgress > 0 && SelectedTask.SliderProgress < 1)
                    dataUpdate.Add("tag", TaskType.IN_PROGRESS.ToString());
                else if (SelectedTask.SliderProgress == 1)
                    dataUpdate.Add("tag", TaskType.DONE.ToString());
                data.Add("update", dataUpdate);

                socket.Emit("task_modify", data);

                ListTask.Clear();
            });
            
        }
    }
}
