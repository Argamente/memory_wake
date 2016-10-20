using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TreaslandLib.Core;
using TreaslandLib.Unity3D.Managers;

namespace TreaslandLib.Unity3D.SuperUI
{
    public class Alert : MonoBehaviour
    {
        private Listener onClickConform;
        private Listener onClickCancel;

        public static void Show (string content, string alertPath, Listener _onConfirm, Listener _onCancel)
        {
            GameObject alertObj = AssetsCenter.InstantiateGameObject (alertPath);
            alertObj.name = "Alert";
            alertObj.GetComponent<Alert> ().Init (content, _onConfirm, _onCancel);
            UIManager.GetInstance ().AddUI (alertObj);
        }


        public Text mContent;
        public Button btnConfirm;
        public Button btnCancel;

        void Awake ()
        {
            this.btnConfirm.onClick.AddListener (this.OnConfirm);
            this.btnCancel.onClick.AddListener (this.OnCancel);
        }


        public void Init (string content, Listener _onConfirm, Listener _onCancel)
        {
            this.onClickConform = _onConfirm;
            this.onClickCancel = _onCancel;
            this.mContent.text = content;
        }


        void OnConfirm ()
        {
            if (this.onClickConform != null)
            {
                this.onClickConform ();
            }
            DestroyAlert ();
        }

        void OnCancel ()
        {
            if (this.onClickCancel != null)
            {
                this.onClickCancel ();
            }
            DestroyAlert ();
        }


        void DestroyAlert ()
        {
            GameObject.Destroy (this.gameObject);
        }

    }
}
