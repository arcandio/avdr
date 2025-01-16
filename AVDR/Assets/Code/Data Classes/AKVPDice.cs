using System;
using UnityEngine;

[Serializable]
public class AKVPDice : AssetKeyValuePair
{
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