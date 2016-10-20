using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TreaslandLib.Unity3D.Utils;
using TreaslandLib.Unity3D.Managers;
using Treasland.MemoryWake.Data;
using Treasland.MemoryWake.Models;
using System.Linq;

public class GroupManageUI : MonoBehaviour
{
    public Button btn_AddGroup;
    public RectTransform groupContent;
    public RectTransform groupEventsPanelHolder;
    public AddEventPanel addEventPanel;

    private GroupCell selectedGroup;

    [HideInInspector]
    public List<GroupCell> cellList = new List<GroupCell> ();

    private static GroupManageUI _instance;

    public static GroupManageUI GetInstance ()
    {
        return _instance;
    }


    void Awake ()
    {
        this.btn_AddGroup.onClick.AddListener (this.OnAddGroup);
        _instance = this;
    }

    void Start ()
    {
        InitGroup ();
    }


    void InitGroup ()
    {
        foreach (KeyValuePair<string,GroupData> kv in Model.groupProxy.groups)
        {
            GroupCell gc = CreateGroupCell ();
            gc.SetData (kv.Value);
            gc.SwitchToShowMode ();

            if (this.selectedGroup == null)
            {
                SelectGroupCell (gc);
            }
            else
            {
                gc.SetSelected (false);
            }
        }
    }

    void OnAddGroup ()
    {
        GroupCell gc = CreateGroupCell ();
        gc.SwitchToEditMode (this.OnEditGroupCellConfirm, this.OnEditGroupCellCancel);
    }


    GroupCell CreateGroupCell ()
    {
        GameObject groupCellObj = AssetsCenter.InstantiateGameObject (Constants.groupCellPath);
        groupCellObj.name = "GroupCell";
        groupCellObj.transform.SetParent (groupContent);
        groupCellObj.transform.localScale = Vector3.one;
        GroupCell gc = groupCellObj.GetComponent<GroupCell> (); 
        this.cellList.Add (gc);
        return gc;
    }



    public void DestroyGroup (GroupCell cell)
    {
        int index = this.cellList.IndexOf (cell);
          
        if (index < 0)
        {
            Log.Error (this, "Wrong, Index of cell Error", index);
            index = 0;
        }

        this.cellList.Remove (cell);

        if (index >= this.cellList.Count)
        {
            index = this.cellList.Count - 1;
        }

        if (this.selectedGroup == cell)
        {
            this.selectedGroup = null;
        }

        string groupName = (cell.data as GroupData).groupName;
        cell.data = null;
        Model.groupProxy.DestroyGroup (groupName);
        GameObject.Destroy (cell.gameObject);


        if (this.selectedGroup == null && this.cellList.Count > 0)
        {
            SelectGroupCell (this.cellList [index]);
        }
    }


    void OnEditGroupCellConfirm (GroupCell cell, string name)
    {
        if (cell.data == null)
        {
            GroupData gData = Model.groupProxy.CreateGroup (name);
            cell.SetData (gData);
            cell.SwitchToShowMode ();
            SelectGroupCell (cell);
        }
        else
        {
            
        }
    }

    void OnEditGroupCellCancel (GroupCell cell)
    {
        if (cell.data == null)
        {
            GameObject.Destroy (cell.gameObject);
        }
        else
        {
            
        }
    }


    public GroupEventsPanel CreateGroupEventsPanel (bool isSelected = false)
    {
        GameObject eventsPanelObj = AssetsCenter.InstantiateGameObject (Constants.groupEventsPanelPath, "GroupEventsPanel");
        eventsPanelObj.transform.SetParent (groupEventsPanelHolder);
        eventsPanelObj.transform.localScale = Vector3.one;
        GroupEventsPanel gep = eventsPanelObj.GetComponent<GroupEventsPanel> ();
        if (isSelected)
        {
            eventsPanelObj.SetActive (true);
        }
        else
        {
            eventsPanelObj.SetActive (false);
        }
        return gep;
    }

    public void SelectGroupCell (GroupCell cell)
    {
        if (this.selectedGroup != null)
        {
            this.selectedGroup.SetSelected (false);
        }
        this.selectedGroup = cell;
        if (this.selectedGroup != null)
        {
            this.selectedGroup.SetSelected (true);
        }
    }

    public void OnGroupCellBeDestroy ()
    {
        
    }
}
