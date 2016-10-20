using UnityEngine;
using System.Collections;

namespace TreaslandLib.Unity3D.Managers
{
    public class AssetsCenter
    {

        public static GameObject InstantiateGameObject (string path, string name = "")
        {
            GameObject obj = MonoBehaviour.Instantiate (Resources.Load (path)) as GameObject;
            if (!name.Equals (string.Empty))
            {
                obj.name = name;
            }

            return obj;
        }
    }
}
