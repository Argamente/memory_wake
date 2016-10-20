using UnityEngine;
using System.Collections;
using System;

namespace Treasland.Unity3D.Utils
{
    public class TimeUtil
    {
        public static DateTime GetTime (string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1));
            long iTime = long.Parse (timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan (iTime);
            return dtStart.Add (toNow);
        }

        public static int GetTimestamp (DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

    }

}
