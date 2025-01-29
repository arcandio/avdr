using System;
using UnityEngine;

/// <summary>
/// A pair for effects. Holds named variables for each intended effect.
/// </summary>
[Serializable]
public class AKVPEffect : AssetKeyValuePair
{
    public GameObject onHitPrefab;
    public GameObject onFinishPrefab;
    public GameObject trailPrefab;
    public GameObject environmentPrefab;

    public override string ToString() {
        string output = key;
        output += ": ";
        output += ", " + onHitPrefab.name;
        output += ", " + onFinishPrefab.name;
        output += ", " + trailPrefab.name;
        output += ", " + environmentPrefab.name;
        return output;
    }
}