using System;
using UnityEngine;

[Serializable]
public class AKVPEffect : AssetKeyValuePair
{
    public GameObject onHitPrefab;
    public GameObject onFinishPrefab;
    public GameObject alwaysOnPrefab;
    public GameObject environmentPrefab;

    public override string ToString() {
        string output = key;
        output += ": ";
        output += ", " + onHitPrefab.name;
        output += ", " + onFinishPrefab.name;
        output += ", " + alwaysOnPrefab.name;
        output += ", " + environmentPrefab.name;
        return output;
    }
}