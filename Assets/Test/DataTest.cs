using UnityEngine;
using System.Collections;
using Treasland.MemoryWake.Data;
using Newtonsoft.Json;
using Treasland.MemoryWake.Models;

public class DataTest : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
        Model.Init ();

        StartCoroutine (Test ());
        /*
        TestJson tj = new TestJson ();
        tj.name = "Test";
        tj.age = 24;

        string jsonStr = JsonConvert.SerializeObject (tj);
        Debug.Log ("Json:" + jsonStr);

        TestJson tj2 = JsonConvert.DeserializeObject (jsonStr) as TestJson;

        string jsonStr2 = JsonConvert.SerializeObject (tj2);
        Debug.Log ("Json2:" + jsonStr);
    */
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }

    IEnumerator Test ()
    {
        GroupData group = Model.groupProxy.CreateGroup ("托福单词");
        group.CraeteMemoryEvent ("第1单元");
        Debug.Log ("Create Unit 1");
        yield return new WaitForSeconds (2.0f);
        group.CraeteMemoryEvent ("第2单元");
        Debug.Log ("Create Unit 2");
        yield return new WaitForSeconds (2.0f);
        group.CraeteMemoryEvent ("第3单元");
        Debug.Log ("Create Unit 3");
        Model.groupProxy.SaveGroupData ();
        yield return new WaitForSeconds (2.0f);
        Model.groupProxy.LoadGroupData ();
        Debug.Log ("Load Data");
    }
}


public class TestJson
{
    public string name {
        get;
        set;
    }

    public int age {
        get;
        set;
    }
}