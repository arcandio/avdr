using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// A set of each type of asset. These will be used to group assets by
/// "paid," "free," "all," "purchased," and "owned" in the `AssetManager`.
/// </summary>
[Serializable]
public class AssetSet
{
    public AssetKeyValuePair[] diceSets = new AssetKeyValuePair[0];
    public AssetKeyValuePair[] trays = new AssetKeyValuePair[0];
    public AssetKeyValuePair[] lightings = new AssetKeyValuePair[0];
    public AssetKeyValuePair[] effects = new AssetKeyValuePair[0];

    /// <summary>
    /// Add two AssetSets together. Whee! Operator overloading!
    /// </summary>
    /// <param name="a">First term of addition.</param>
    /// <param name="b">Second term of addition.</param>
    /// <returns>A new AssetSet containing a and b.</returns>
    public static AssetSet operator + (AssetSet a, AssetSet b) {
        AssetSet temp = new AssetSet();
        /* guard clauses */
        if(a == null && b == null) return temp;
        else if(a == null && b != null) return b;
        else if(b == null && a != null) return a;
        
        List<AssetKeyValuePair> tempPairList = new List<AssetKeyValuePair>(a.diceSets);
        tempPairList.AddRange(b.diceSets);
        temp.diceSets = tempPairList.ToArray();

        tempPairList = new List<AssetKeyValuePair>(a.trays);
        tempPairList.AddRange(b.trays);
        temp.trays = tempPairList.ToArray();

        tempPairList = new List<AssetKeyValuePair>(a.lightings);
        tempPairList.AddRange(b.lightings);
        temp.lightings = tempPairList.ToArray();

        tempPairList = new List<AssetKeyValuePair>(a.effects);
        tempPairList.AddRange(b.effects);
        temp.effects = tempPairList.ToArray();

        return temp;
    }

    public bool Contains(string assetName) {
        return false;
    }

    public GameObject GetPrefabByName(string assetName) {
        GameObject temp = null;

        return temp;
    }

    public string[] GetDiceSetKeys() {
        return GetKeys(diceSets);
    }

    public string[] GetTrayKeys() {
        return GetKeys(trays);
    }

    public string[] GetLightingKeys() {
        return GetKeys(lightings);
    }

    public string[] GetEffectKeys() {
        return GetKeys(effects);
    }

    private string[] GetKeys(AssetKeyValuePair[] pairs) {
        List<string> keys = new List<string>();
        foreach(AssetKeyValuePair pair in pairs) {
            keys.Add(pair.key);
        }
        return keys.ToArray();
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(DebugAssetKeyValuePairList(diceSets));
        stringBuilder.Append("\n");
        stringBuilder.Append(DebugAssetKeyValuePairList(trays));
        stringBuilder.Append("\n");
        stringBuilder.Append(DebugAssetKeyValuePairList(lightings));
        stringBuilder.Append("\n");
        stringBuilder.Append(DebugAssetKeyValuePairList(effects));
        return stringBuilder.ToString();
    }

    public static string DebugAssetKeyValuePairList(AssetKeyValuePair[] pairs) {
        List<string> strings = new List<string>();
        foreach(AssetKeyValuePair pair in pairs) {
            strings.Add(pair.ToString());
        }
        return string.Join(", ", strings);
    }
}
