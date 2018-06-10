using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyberShift_CSharp.Model.List
{
    class ListTaskTypeModel: BaseList<TaskTypeModel>
    {
        public ListTaskTypeModel(): base()
        {
            this.List.Clear();
            this.Add(new TaskTypeModel("PlaylistCheck", "TO DO"));
            this.Add(new TaskTypeModel("ChartDonut", "IN PROGRESS"));
            this.Add(new TaskTypeModel("Alert", "WARNING"));
            this.Add(new TaskTypeModel("Database", "BACKLOG"));
        }
    }
}
