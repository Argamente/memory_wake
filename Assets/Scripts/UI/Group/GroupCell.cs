using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TreaslandLib.Unity3D.Utils;
using Treasland.MemoryWake.Models;
using Treasland.MemoryWake.Data;
using TreaslandLib.Unity3D.SuperUI;
using TreaslandLib.Core;
using TreaslandLib.Unity3D.Managers;
using UnityEngine.EventSystems;

public class GroupCell : AbstractUI,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject panelShow;
    public GameObject panelEdit;

    // show panel attributes
    public Text gName;
    public Button btnAddMemoryEvent;
    public Button btnDestroyGroup;

    // edit panel attributes
    public InputField gNameInput;
    public Button btnEditConfirm;
    public Button btnEditCancel;

    [HideInInspector]
    public GroupEventsPanel eventPanel;

    // use for selected
    private Image bgImg;

    private Color unSelectedColor = new Color32 (246, 246, 246, 255);
    private Color selectedColor = new Color32 (184, 184, 184, 255);

    private bool isEditMode = false;

    private Listener <GroupCell, string> pOnEditConfirm;
    private Listener<GroupCell> pOnEditCancel;

    void Awake ()
    {
        this.btnAddMemoryEvent.onClick.AddListener (this.OnAddMemoryEvent);
        this.btnDestroyGroup.onClick.AddListener (this.OnDestroyGroup);
        this.btnEditConfirm.onClick.AddListener (this.OnEditConfirm);
        this.btnEditCancel.onClick.AddListener (this.OnEditCancel);
        this.bgImg = this.gameObject.GetComponent<Image> ();

        this.btnDestroyGroup.gameObject.SetActive (false);
        this.btnAddMemoryEvent.gameObject.SetActive (false);
    }


    public void SwitchToShowMode ()
    {
        this.isEditMode = false;
        panelEdit.SetActive (false);
        panelShow.SetActive (true);
        this.RenderUI ();
    }

    public void SwitchToEditMode (Listener< GroupCell ,string> _OnConfirm, Listener<GroupCell> _OnCancel)
    {
        this.pOnEditConfirm = _OnConfirm;
        this.pOnEditCancel = _OnCancel;

        this.isEditMode = true;
        panelShow.SetActive (false);
        panelEdit.SetActive (true);
        this.RenderUI ();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject (this.gNameInput.gameObject);
    }


    void OnAddMemoryEvent ()
    {
        GroupManageUI.GetInstance ().addEventPanel.OpenPanel (this);
    }

    void OnDestroyGroup ()
    {
        Alert.Show ("确认删除事件组吗？", Constants.alert, this.OnConfirmDestroyGroup, null);
    }

    void OnConfirmDestroyGroup ()
    {
        GroupManageUI.GetInstance ().DestroyGroup (this);
    }



    void OnEditConfirm ()
    {
        string name = this.gNameInput.text;
        name = name.Trim ();
        this.gNameInput.text = name;
        if (name.Length == 0)
        {
            Log.Error (this, "Group name can not be empty");
            return;
        }

        // check if the name is alredy exists
        if (!Model.groupProxy.IsAvaliableGroupName (name))
        {
            Alert.Show ("事件组 " + name + " 已经存在", Constants.alert, this.OnEditConfirmFaild, null);
            return;
        }

        if (this.pOnEditConfirm != null)
        {
            this.pOnEditConfirm (this, name);
        }
        //SwitchToShowMode ();
    }


    public void SetData (GroupData _gData)
    {
        this.data = _gData;

        this.eventPanel = GroupManageUI.GetInstance ().CreateGroupEventsPanel ();
        this.eventPanel.InitEventsPanel (_gData);
    }


    void OnEditConfirmFaild ()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject (this.gNameInput.gameObject);
    }

    void OnEditCancel ()
    {
        //GameObject.Destroy (this.gameObject);
        if (this.pOnEditCancel != null)
        {
            this.pOnEditCancel (this);
        }
    }


    public override void RenderUI ()
    {
        if (this.data != null)
        {
            GroupData groupData = this.data as GroupData;
            this.gName.text = groupData.groupName;
            this.gNameInput.text = groupData.groupName;
        }
    }

    public void SetSelected (bool isSelected)
    {
        if (isSelected)
        {
            this.bgImg.color = selectedColor;
            if (this.eventPanel != null)
            {
                this.eventPanel.gameObject.SetActive (true);
            }
        }
        else
        {
            this.bgImg.color = unSelectedColor;  
            if (this.eventPanel != null)
            {
                this.eventPanel.gameObject.SetActive (false);
            }
        }
    }


    public void OnPointerClick (PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (this.isEditMode == false)
            {
                GroupManageUI.GetInstance ().SelectGroupCell (this);
            }   
        }
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        this.btnDestroyGroup.gameObject.SetActive (true);
        this.btnAddMemoryEvent.gameObject.SetActive (true);
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        this.btnDestroyGroup.gameObject.SetActive (false);
        this.btnAddMemoryEvent.gameObject.SetActive (false);
    }



    void OnDestroy ()
    {
        if (this.eventPanel != null)
        {
            GameObject.Destroy (this.eventPanel.gameObject);
        }
    }

        
}
