using UnityEngine;
using System.Collections;
using Treasland.MemoryWake.Models;

public class Main : MonoBehaviour
{
    void Awake ()
    {
        Model.Init ();       
    }


    /*
    void OnGUI ()
    {
        
        GUILayout.Label ("");
        GUILayout.Label ("");
        GUILayout.Label ("");
        GUILayout.Label ("");
        GUILayout.Label ("");
        GUI.skin.label.normal.textColor = Color.red;
        GUILayout.Label ("PersistentPath: " + Application.persistentDataPath);
        GUILayout.Label ("DataPath: " + Application.dataPath);
        GUILayout.Label ("SteramingAssetsPath: " + Application.streamingAssetsPath);
    }
    */
	
}
