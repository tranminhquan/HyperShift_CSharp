using HyberShift_CSharp.Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyberShift_CSharp.Model.List
{
    public class ListTaskModel: BaseList<TaskModel>
    {
        private static ListTaskModel instance = null;
        public ListTaskModel(): base()
        {

        }

        public static ListTaskModel GetInstance()
        {
            if (instance == null)
                instance = new ListTaskModel();
            return instance;
        }

        public ObservableCollection<object> ListTaskName
        {
            get
            {
                return this.GetCollectionOfField("Name");
            }
        }

        public ObservableCollection<object> ListStartDay
        {
            get
            {
                return this.GetCollectionOfField("StartDay");
            }
        }

        public ObservableCollection<object> ListEndDay
        {
            get
            {
                return this.GetCollectionOfField("EndDay");
            }
        }

        public ObservableCollection<object> ListTag
        {
            get
            {
                return this.GetCollectionOfField("Tag");
            }
        }

        public ObservableCollection<object> ListPerformer
        {
            get
            {
                return this.GetCollectionOfField("Performer");
            }
        }

        public ObservableCollection<object> ListProgress
        {
            get
            {
                return this.GetCollectionOfField("Progress");
            }
        }

        public TaskModel GetTaskFromID(string id)
        {
            return this.GetFirstObjectByValue("ID", id);
        }

        public TaskModel GetTaskFromName(string name)
        {
            return this.GetFirstObjectByValue("Name", name);
        }

        public ObservableCollection<TaskModel> GetTaskFromPerformer(string performer)
        {
            return this.GetCollectionByValue("Performer", performer);
        }

        public ObservableCollection<TaskModel> GetTaskFromProgress(string progress)
        {
            return this.GetCollectionByValue("Progress", progress);
        }

        public ObservableCollection<TaskModel> GetTaskFromTag(TaskType tag)
        {
            return this.GetCollectionByValue("Tag", tag);
        }

        public void Clear()
        {
            this.List.Clear();
        }

    }
}
