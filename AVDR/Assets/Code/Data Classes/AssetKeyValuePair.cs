using System;
using UnityEngine;

/// <summary>
/// A pair for emulating dictionary functionality simply in UnityEngine GUI.
/// This will let us set up prefabs in `AssetManager`.
/// </summary>
[Serializable]
public class AssetKeyValuePair
{
    public string key;
    public GameObject[] prefabs;

    public override string ToString() {
        string output = key;
        if(prefabs.Length > 0) {
            output += ": ";
            foreach (GameObject prefab in prefabs) {
                output += ", " + prefab.name;
            }
        }
        return output;
    }
}