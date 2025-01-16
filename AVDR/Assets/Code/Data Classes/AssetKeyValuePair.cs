using System;
using UnityEngine;

/// <summary>
/// A pair for emulating dictionary functionality simply in UnityEngine GUI.
/// This will be subclassed a for each asset type.
/// </summary>
[Serializable]
public class AssetKeyValuePair
{
    public string key;
    public int priceInCents;

    public override string ToString() {
        string output = key;
        return output;
    }
}