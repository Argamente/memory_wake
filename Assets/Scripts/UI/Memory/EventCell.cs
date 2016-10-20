using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Treasland.MemoryWake.Data;
using System;
using Treasland.Unity3D.Utils;
using TreaslandLib.Unity3D.Utils;
using Treasland.MemoryWake.Data;
using Treasland.MemoryWake.Models;

public class EventCell : AbstractUI
{
    public AudioSource remindAudio;
    public Text mName;

    public Image imgStartTime;
    public Text txtStartTime;

    public List<Image> timeLineBgList = new List<Image> ();
    public List<Text> timeLineTextList = new List<Text> ();
    public List<RectTransform> timeLineProgressList = new List<RectTransform> ();

    private List<int> remindedTime = new List<int> ();

    private float fullProgressWidth = 100;

    private Color checkedColor = new Color (0, 1, 0, 1);


    /// <summary>
    /// data赋值的时候会调用一次
    /// </summary>
    public override void RenderUI ()
    {
        InitTimeLineCell ();
        InvokeRepeating ("RepeadRenderUI", 0, 1);
    }


    void Start ()
    {
        
    }

    void RepeadRenderUI ()
    {
        if (this.data != null)
        {
            MemoryData mData = this.data as MemoryData;
            this.mName.text = mData.memoryName;

            mData.timeData.Update ();

            DateTime startDateTime = TimeUtil.GetTime (mData.timeData.startTime.ToString ());
            string startTimeStr = startDateTime.GetDateTimeFormats ('g') [0].ToString ();
            this.txtStartTime.text = startTimeStr;

            int nowTimestamp = TimeUtil.GetTimestamp (DateTime.Now);

            int listIndex = 0;

            foreach (KeyValuePair<int, enTimeState> kv in mData.timeData.timestampDic)
            {

                timeLineProgressList [listIndex].gameObject.SetActive (false);

                if (kv.Value == enTimeState.TIMEOUT)
                {
                    // set background is red
                    timeLineTextList [listIndex].text = kv.Value.ToString ();
                    SetTimeLineBgColor (timeLineBgList [listIndex], kv.Key, 0);

                    if (isRemindedTime (kv.Key) == false)
                    {
                        SetRemindedTime (kv.Key);
                        PlayRemindAudio ();
                    }

                }
                else if (kv.Value == enTimeState.READY)
                {
                    int hasSeconds = mData.timeData.startTime + kv.Key - nowTimestamp;
                    hasSeconds = Mathf.Clamp (hasSeconds, 0, hasSeconds);
                    //timeLineTextList [listIndex].text = hasSeconds.ToString ();
                    timeLineTextList [listIndex].text = SecondsToStr (hasSeconds);

                    //SetTimeLineBgColor (timeLineBgList [listIndex], kv.Key, hasSeconds);
                    SetProgress (timeLineProgressList [listIndex], kv.Key, hasSeconds);
                }
                else if (kv.Value == enTimeState.CHECKED)
                {
                    timeLineTextList [listIndex].text = kv.Value.ToString ();
                    timeLineBgList [listIndex].color = checkedColor;
                }
                ++listIndex;
            }
        } 
    }


    void SetTimeLineBgColor (Image img, int sumSeconds, int hasSeconds)
    {
        float rate = (float)hasSeconds / (float)sumSeconds;
        float colorValue = 255 * rate;

        // Log.Info (this, "sumSeconds: ", sumSeconds, "hasSeconds: ", hasSeconds, "rate: ", rate, "colorValue: ", colorValue);

        Color finalColor = new Color (255, colorValue / 255, colorValue / 255, 255);
        img.color = finalColor;
    }


    void SetProgress (RectTransform progressTrans, int sumSeconds, int hasSeconds)
    {
        progressTrans.gameObject.SetActive (true);
        float rate = (float)hasSeconds / (float)sumSeconds;
        float width = 30 * rate;
        width = Mathf.Clamp (width, 0, width);
//        progressTrans.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, width);
        progressTrans.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, width);
    }


    void InitTimeLineCell ()
    {
        if (this.data != null)
        {
            MemoryData mData = this.data as MemoryData;
            int index = 0;
            foreach (KeyValuePair<int, enTimeState> kv in mData.timeData.timestampDic)
            { 
                this.timeLineBgList [index].GetComponent<TimeLineCell> ().Init (index, kv.Key, this.OnSetChecked);
                ++index;

                // 初始化的时间将 Timeout 的项添加进已提醒的列表中，就是刚打开应用时已经过期的项目不提醒
                if (kv.Value == enTimeState.TIMEOUT)
                {
                    SetRemindedTime (kv.Key);
                }
            }
        }
    }


    void OnSetChecked (int index, int key)
    {
        if (this.data != null)
        {
            MemoryData mData = this.data as MemoryData;
            if (mData.timeData.CheckTime (key))
            {
                timeLineTextList [index].text = "CHECKED";
                timeLineBgList [index].color = checkedColor;

                Model.groupProxy.SaveGroupData ();
            }
        }
    }


    string SecondsToStr (int seconds)
    {
        int oneDay = 86400;
        int oneHour = 3600;
        int oneMinute = 60;
        /*
        if (seconds >= oneDay)
        {
            int day = seconds / oneDay;
            int dayRem = seconds % oneDay;

            int hour = dayRem / oneHour;
            int hourRem = dayRem % oneHour;

            int minute = hourRem / oneMinute;
            int second = hourRem % oneMinute;
            return string.Format ("{0}天{1}小时{2}分钟{3}秒", day, hour, minute, second);
        }
        else
        */
        if (seconds >= oneHour)
        {
            int hour = seconds / oneHour;
            int hourRem = seconds % oneHour;

            int minute = hourRem / oneMinute;
            int second = hourRem % oneMinute;
            return string.Format ("{0}:{1}:{2}", 
                hour.ToString ().PadLeft (2, '0'), 
                minute.ToString ().PadLeft (2, '0'), 
                second.ToString ().PadLeft (2, '0'));
        }
        else if (seconds > oneMinute)
        {
            int minute = seconds / oneMinute;
            int second = seconds % oneMinute;
            return string.Format ("{0}:{1}", 
                minute.ToString ().PadLeft (2, '0'), 
                second.ToString ().PadLeft (2, '0'));
        }
        else
        {
            return string.Format ("{0}", seconds.ToString ().PadLeft (2, '0'));
        }
    }


    bool isRemindedTime (int time)
    {
        return this.remindedTime.Contains (time);
    }

    void SetRemindedTime (int time)
    {
        this.remindedTime.Add (time);
    }

    void PlayRemindAudio ()
    {
        remindAudio.time = 5f;
        remindAudio.Play ();
    }

}
