using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Treasland.Unity3D.Utils;

namespace Treasland.MemoryWake.Data
{

    public class GroupData
    {
        public string groupName {
            get;
            set;
        }

        public List<MemoryData> memories {
            get;
            set;
        }



        public GroupData (string _name)
        {
            this.groupName = _name;
            this.memories = new List<MemoryData> ();
        }

  

        public MemoryData CraeteMemoryEvent (string _memName)
        {
            MemoryData mData = new MemoryData (TimeUtil.GetTimestamp (System.DateTime.Now), _memName);
            this.memories.Add (mData);
            return mData;
        }
    }
}
