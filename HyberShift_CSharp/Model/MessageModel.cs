using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyberShift_CSharp.Utilities;

namespace HyberShift_CSharp.Model
{
    public class MessageModel
    {
        public string ID { get; set; }
        public string MessageID { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public string ImgString { get; set; }
        public string FileString { get; set; }
        public string FileName { get; set; }
        public long Timestamp { get; set; }

        // TO-DO: Timestamp Property needs to be convert from long to some kind of date (E.g: 19:25 Friday)
        public string TimestampDisplay
        {
            get { return new DateTime(Timestamp).ToString(); }
            set { Timestamp = DateTime.Now.Ticks; }
        }

        public MessageModel()
        {

        }

        public MessageModel(string id, string messageid, string message, string sender, string imgstring, string filestring,
                            string filename, long timestamp)
        {
            ID = id;
            MessageID = messageid;
            Message = message;
            Sender = sender;
            ImgString = imgstring;
            FileString = filestring;
            FileName = filename;
            Timestamp = timestamp;
        }
    }
}
