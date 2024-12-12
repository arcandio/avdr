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
    public GameObject prefab;

    public override string ToString() {
        string output = key;
        if(prefab != null) output += " " + prefab.name;
        return output;
    }
}