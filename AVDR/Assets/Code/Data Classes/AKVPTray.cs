using System;
using UnityEngine;

[Serializable]
public class AKVPTray : AssetKeyValuePair
{
    public GameObject tray;

    public override string ToString() {
        string output = key;
        output += ", " + tray.name;
        return output;
    }
}