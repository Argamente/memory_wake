using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Treasland.MemoryWake.Data;
using TreaslandLib.Utils;
using Newtonsoft.Json;
using System.Linq;
using TreaslandLib.Unity3D.Utils;

namespace Treasland.MemoryWake.Models
{
    public class GroupProxy
    {
        public Dictionary<string,GroupData> groups;
        //= new Dictionary<string, GroupData> ();

        public GroupProxy ()
        {
            LoadGroupData ();
        }


        public GroupData CreateGroup (string _groupName)
        {
            if (groups.ContainsKey (_groupName))
            {
                return null;
            }
            else
            {
                GroupData group = new GroupData (_groupName);
                groups.Add (_groupName, group);
                SaveGroupData ();
                return group;
            }
        }


        public bool DestroyGroup (string _groupName)
        {
           

            if (groups.ContainsKey (_groupName))
            {
                GroupData oldData = groups [_groupName];
                groups.Remove (_groupName);
                SaveGroupData ();
                return true;
            }
            return false;
        }


        public MemoryData CreateMemory (GroupData currGroup, string _memoryTitle)
        {
            MemoryData data = currGroup.CraeteMemoryEvent (_memoryTitle);
            SaveGroupData ();
            return data;
        }

        public bool DestroyMemory (GroupData currGroup, int memoryID)
        {
            return true;
        }


        public void LoadGroupData ()
        {
            string jsonStr = IOUtils.OpenTextFile (Constants.dataFilePath);
            try
            {
                groups = (Dictionary<string,GroupData>)JsonConvert.DeserializeObject<Dictionary<string,GroupData>> (jsonStr);

                //string jsonStr2 = JsonConvert.SerializeObject (groups);
                //Debug.LogError (jsonStr2);
            }
            catch (System.Exception e)
            {
                Log.Error (this, "Deserialize Groups Error", e);
            }

            if (groups == null)
            {
                groups = new Dictionary<string, GroupData> ();
            }
        }


        public void SaveGroupData ()
        {
            string jsonStr = JsonConvert.SerializeObject (groups);
            IOUtils.WriteTextFile (Constants.dataFilePath, jsonStr);

            Debug.Log (Constants.dataFilePath);
        }


        public bool IsAvaliableGroupName (string name)
        {
            if (name.Trim ().Length > 0 && !groups.ContainsKey (name))
            {
                return true;
            }
            return false;
        }


    }
}