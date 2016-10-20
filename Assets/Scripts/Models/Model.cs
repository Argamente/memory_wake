using UnityEngine;
using System.Collections;

namespace Treasland.MemoryWake.Models
{
    public class Model
    {
        public static GroupProxy groupProxy;
        public static MemoryProxy memoryProxy;

        public static void Init ()
        {
            groupProxy = new GroupProxy ();
            memoryProxy = new MemoryProxy ();
        }

    }
}
