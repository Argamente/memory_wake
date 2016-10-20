using UnityEngine;
using System.Collections;
using Treasland.Unity3D.Utils;

namespace Treasland.MemoryWake.Data
{
    public class MemoryData
    {
        public int memoryId {
            get;
            set;
        }

        public string memoryName {
            get;
            set;
        }

        public TimeData timeData {
            get;
            set;
        }

        public MemoryData ()
        {
            
        }


        public MemoryData (string _name)
        {
            this.memoryId = TimeUtil.GetTimestamp (System.DateTime.Now);
            this.memoryName = _name;
            this.timeData = new TimeData_Ebbinghaus ();
        }

        public MemoryData (int _id, string _name)
        {
            this.memoryId = _id;
            this.memoryName = _name;
            this.timeData = new TimeData_Ebbinghaus ();
        }

       
    }
}