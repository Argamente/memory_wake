using UnityEngine;
using System.Collections;

namespace TreaslandLib.Unity3D.Managers
{
    public class UIManager : MonoBehaviour
    {

        public RectTransform popupLayer;

        private static UIManager _instance;

        public static UIManager GetInstance ()
        {
            return _instance;
        }

        void Awake ()
        {
            _instance = this;
        }


        public void AddUI (GameObject uiObj)
        {
            uiObj.transform.SetParent (popupLayer);
            uiObj.transform.localScale = Vector3.one;
        }
    }
}