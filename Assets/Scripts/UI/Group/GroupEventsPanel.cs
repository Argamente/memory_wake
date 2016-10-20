using UnityEngine;
using System.Collections;
using Treasland.MemoryWake.Data;
using Treasland.MemoryWake.Models;
using TreaslandLib.Unity3D.Managers;
using UnityEngine.UI;

public class GroupEventsPanel : MonoBehaviour
{
    public RectTransform content;

    private GroupData currGroup;

    void Start ()
    {
        RectTransform rectTrans = this.gameObject.GetComponent<RectTransform> ();
        rectTrans.anchoredPosition = Vector2.zero;
    }


    public void InitEventsPanel (GroupData group)
    {
        this.currGroup = group;
        for (int i = 0; i < group.memories.Count; ++i)
        {
            AddEvent (group.memories [i]);
        }   
    }

    public void AddEvent (MemoryData eventData)
    {
        GameObject eventObj = AssetsCenter.InstantiateGameObject (Constants.eventCellPath, "EventCell");
        eventObj.transform.SetParent (content);
        eventObj.transform.localScale = Vector3.one;
        EventCell eCell = eventObj.GetComponent<EventCell> ();
        eCell.data = eventData;
    }

    public bool AddMemory (string memoryContent)
    {
        memoryContent = memoryContent.Trim ();
        if (memoryContent.Length == 0)
        {
            return false;
        }

        MemoryData data = Model.groupProxy.CreateMemory (this.currGroup, memoryContent);

        AddEvent (data);
        return true;
    }
}

