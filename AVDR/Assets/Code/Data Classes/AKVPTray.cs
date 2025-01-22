using System;
using UnityEngine;

/// <summary>
/// A pair for trays, which already exist in the scene and
/// therefore will not be instantiated.
/// </summary>
[Serializable]
public class AKVPTray : AssetKeyValuePair
{
    public GameObject trayPrefab;

    public override string ToString() {
        string output = key;
        output += ", " + trayPrefab.name;
        return output;
    }
}