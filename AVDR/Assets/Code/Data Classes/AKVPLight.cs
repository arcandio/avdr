using System;
using UnityEngine;

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