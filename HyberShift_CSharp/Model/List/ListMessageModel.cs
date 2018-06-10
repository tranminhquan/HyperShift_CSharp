using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyberShift_CSharp.Model.List
{
    public class ListMessageModel: BaseList<MessageModel>
    {
        private static ListMessageModel instance = null;

        // constructor
        public ListMessageModel(): base()
        {
          
        }

        // singleton
        public static ListMessageModel GetInstance()
        {
            if (instance == null)
                instance = new ListMessageModel();
            return instance;
        }

        public ObservableCollection<object> ListMessage => GetCollectionOfField("Message");
        public ObservableCollection<object> ListSender => GetCollectionOfField("Sender");

        public MessageModel GetMessageFromId(string id)
        {
            return this.GetFirstObjectByValue("ID", id);
        }

        public MessageModel GetMessageFromSender(string sender)
        {
            return this.GetFirstObjectByValue("Sender", sender);
        }

        public void Clear()
        {
            list.Clear();
        }
    }
}
