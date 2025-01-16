using System;
using UnityEngine;

/// <summary>
/// A pair for lighting rigs, which already exist in the scene and are
/// therefore not instantiated.
/// </summary>
[Serializable]
public class AKVPLight : AssetKeyValuePair
{
    public GameObject lightingRig;

    public override string ToString() {
        string output = key;
        output += ", " + lightingRig.name;
        return output;
    }
}