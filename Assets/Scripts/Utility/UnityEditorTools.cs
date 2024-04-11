using System.IO;
using UnityEditor;
using UnityEngine;

namespace Utility
{
    public abstract class UnityEditorTools
    {
        [MenuItem("InHouseTools/Progression/ResetData")]
        public static void ResetData()
        {
            PlayerPrefs.DeleteAll();
            File.Delete(Path.Combine(Application.persistentDataPath, "ta_persistence.json"));
        }
    }
}