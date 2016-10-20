using UnityEngine;
using System.Collections;
using Treasland.MemoryWake.Data;

public class AbstractUI : MonoBehaviour
{
    private object _data;

    public object data {
        get
        {
            return this._data;
        }
        set
        {
            this._data = value;
            RenderUI ();
        }
    }

    public virtual void RenderUI ()
    {
        
    }
	
}
