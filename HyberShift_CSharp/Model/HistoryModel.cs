using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyberShift_CSharp.Model
{
    public class HistoryModel
    {
        private static HistoryModel instance = null;

        public long LastSignOut { get; set; }

        public HistoryModel()
        {
            LastSignOut = 0;
        }

        public static HistoryModel GetInstance()
        {
            if (instance == null)
                instance = new HistoryModel();
            return instance;
        }
    }
}
