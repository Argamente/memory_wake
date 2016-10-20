using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Treasland.MemoryWake.Data;
using Treasland.MemoryWake.Models;
using TreaslandLib.Unity3D.Utils;

public class AddEventPanel : MonoBehaviour
{
    public InputField mInputField;
    public Button btnAdd;
    public Button btnClose;

    private RectTransform rectTrans;

    private GroupCell currGroup;

    void Start ()
    {
        this.rectTrans = this.gameObject.GetComponent<RectTransform> ();
        this.btnAdd.onClick.AddListener (this.OnClickAdd);
        this.btnClose.onClick.AddListener (this.OnClickClose);
    }


    public void OpenPanel (GroupCell group)
    {
        this.currGroup = group;
        this.gameObject.SetActive (true);
    }

    public void ClosePanel ()
    {
        this.mInputField.text = "";
        this.currGroup = null;
        this.gameObject.SetActive (false);
    }


    void OnClickAdd ()
    {
        string memoryContent = this.mInputField.text;
        if (!this.currGroup.eventPanel.AddMemory (memoryContent))
        {
            Log.Error (this, "Add Memory Error");
            return;
        }
        ClosePanel ();
    }



    void OnClickClose ()
    {
        ClosePanel ();
    }

   
}
