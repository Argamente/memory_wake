using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Treasland.Unity3D.Utils;

/// <summary>
/// 所有的时间计算都以秒为单位
/// </summary>
namespace Treasland.MemoryWake.Data
{
    public enum enTimeType
    {
        EXACT,
        // 确切的时间
        RELATIVE,
        // 相对于某个时间的偏移
    }

    public enum enTimeState
    {
        READY,
        TIMEOUT,
        CHECKED,
        ERROR,
    }

    public class TimeData
    {
        public int startTime {
            get;
            set;
        }

        public enTimeType timeType {
            get;
            set;
        }

        public Dictionary<int,enTimeState> timestampDic {
            get;
            set;
        }

        private List<int> timeoutKey = new List<int> ();

        public TimeData (enTimeType _type)
        {
            this.timeType = _type;
            timestampDic = new Dictionary<int, enTimeState> ();
            this.startTime = TimeUtil.GetTimestamp (System.DateTime.Now);
        }

        protected void AddTime (int timeSeconds, enTimeState timeState)
        {
            this.timestampDic.Add (timeSeconds, timeState);
        }


        public void Update ()
        {
            this.timeoutKey.Clear ();

            int nowTimestamp = TimeUtil.GetTimestamp (System.DateTime.Now);
            foreach (KeyValuePair<int,enTimeState> kv in timestampDic)
            {
                if (this.timeType == enTimeType.EXACT)
                {
                    if (nowTimestamp >= kv.Key && kv.Value == enTimeState.READY)
                    {
                        //timestampDic [kv.Key] = enTimeState.TIMEOUT;
                        this.timeoutKey.Add (kv.Key);
                    }
                }
                else if (this.timeType == enTimeType.RELATIVE)
                {
                    if (nowTimestamp >= this.startTime + kv.Key && kv.Value == enTimeState.READY)
                    {
                        //timestampDic [kv.Key] = enTimeState.TIMEOUT;
                        this.timeoutKey.Add (kv.Key);
                    }
                }
            }


            for (int i = 0; i < this.timeoutKey.Count; ++i)
            {
                int key = this.timeoutKey [i];
                this.timestampDic [key] = enTimeState.TIMEOUT;
            }
        }

        public enTimeState GetTimeState (int _time)
        {
            if (this.timestampDic.ContainsKey (_time))
            {
                return this.timestampDic [_time];
            }
            else
            {
                return enTimeState.ERROR;
            }
        }

        public bool CheckTime (int _time)
        {
            if (this.timestampDic.ContainsKey (_time) && this.timestampDic [_time] == enTimeState.TIMEOUT)
            {
                this.timestampDic [_time] = enTimeState.CHECKED;
                return true;
            }
            else
            {
                //Debug.LogError ("State not timeout");
                return false;
            }
        }
    }


    public class TimeData_Ebbinghaus : TimeData
    {
        private int[] ebbRelativeTimeArr = { 300, 1800, 43200, 86400, 172800, 345600, 604800, 1296000 };

        public TimeData_Ebbinghaus () : base (enTimeType.RELATIVE)
        {
            for (int i = 0; i < ebbRelativeTimeArr.Length; ++i)
            {
                base.AddTime (ebbRelativeTimeArr [i], enTimeState.READY);
            }
        }
    }



}
