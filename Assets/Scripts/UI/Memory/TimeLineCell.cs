using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TreaslandLib.Core;


public class TimeLineCell : MonoBehaviour, IPointerClickHandler
{
    private Listener<int,int> onClickRightButton;
    private int index = 0;
    private int key = 0;

    public void Init (int _index, int _key, Listener<int,int> _OnRClick)
    {
        this.index = _index;
        this.key = _key;
        this.onClickRightButton = _OnRClick;
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (this.onClickRightButton != null)
            {
                this.onClickRightButton (this.index, this.key);
            }
        }
    }


	
}
