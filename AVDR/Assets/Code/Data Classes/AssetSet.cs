using System;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// Gets the keys of all the assets in this set, including dice, trays, lighting, and effects. Useful for finding out if the set contains a name at all.
    /// </summary>
    /// <returns>A list of all the keys in this set.</returns>
    public string[] GetAllKeys() {
        List<string> allKeys = new List<string>();
        allKeys.AddRange(GetDiceSetKeys());
        allKeys.AddRange(GetTrayKeys());
        allKeys.AddRange(GetLightingKeys());
        allKeys.AddRange(GetEffectKeys());
        return allKeys.ToArray();
    }

    /// <summary>
    /// Gets an AssetKeyValuePair from this AssetSet by name.
    /// </summary>
    /// <param name="assetType"></param>
    /// <param name="assetName"></param>
    /// <returns>AssetKeyValue Pair found. Null if none was found.</returns>
    public AssetKeyValuePair GetAssetPair(AssetType assetType, string assetName) {
        switch(assetType) {
            case AssetType.DiceSet:
                return GetAssetPair(diceSets, assetName);
            case AssetType.Tray:
                return GetAssetPair(trays, assetName);
            case AssetType.Lighting:
                return GetAssetPair(lightings, assetName);
            case AssetType.Effects:
                return GetAssetPair(effects, assetName);
            default:
                Debug.LogError("Did not find " + assetName + " in " + assetType);
                return null;
        }
    }

    /// <summary>
    /// Gets the pair with a matching name, given a list of pairs.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="assetName"></param>
    /// <returns>AssetKeyValuePair found, or null if it did not.</returns>
    private AssetKeyValuePair GetAssetPair(AssetKeyValuePair[] array, string assetName) {
        foreach(AssetKeyValuePair pair in array) {
            if(pair.key == assetName) {
                return pair;
            }
        }
        Debug.LogError("Did not find " + assetName + " in " + array);
        return null;
    }

    /// <summary>
    /// Adds an AssetKeyValuePair to a list. Used for copying items when purchased.
    /// </summary>
    /// <param name="assetType">The type of asset, and thus the list to add the asset to.</param>
    /// <param name="assetKeyValuePair">The asset pair to add.</param>
    public void AddAsset(AssetType assetType, AssetKeyValuePair assetKeyValuePair) {
        List<AssetKeyValuePair> list;
        switch(assetType) {
            case AssetType.DiceSet:
                list = new List<AssetKeyValuePair>(diceSets);
                list.Add(assetKeyValuePair);
                diceSets = list.ToArray();
                break;
            case AssetType.Tray:
                list = new List<AssetKeyValuePair>(trays);
                list.Add(assetKeyValuePair);
                trays = list.ToArray();
                break;
            case AssetType.Lighting:
                list = new List<AssetKeyValuePair>(lightings);
                list.Add(assetKeyValuePair);
                lightings = list.ToArray();
                break;
            case AssetType.Effects:
                list = new List<AssetKeyValuePair>(effects);
                list.Add(assetKeyValuePair);
                effects = list.ToArray();
                break;
            default:
                Debug.Log("Cannot add an asset to AssetType.None.");
                break;
        }
    }

    /// <summary>
    /// Checks whether this AssetSet contains an asset by name.
    /// </summary>
    /// <param name="assetName">The name to search for.</param>
    /// <returns>AssetType of the list that contains the name. Check this for `AssetType.None` for whether or not it does contain the name.</returns>
    public AssetType Contains(string assetName) {
        if (GetDiceSetKeys().Contains(assetName)) {
            return AssetType.DiceSet;
        }
        else if (GetTrayKeys().Contains(assetName)) {
            return AssetType.Tray;
        }
        else if (GetLightingKeys().Contains(assetName)) {
            return AssetType.Lighting;
        }
        else if (GetEffectKeys().Contains(assetName)) {
            return AssetType.Effects;
        }
        else {
            return AssetType.None;
        }
    }

    /// <summary>
    /// Helper method that gets an array of keys from a list of pairs.
    /// </summary>
    /// <param name="pairs"></param>
    /// <returns>Array of string keys.</returns>
    private string[] GetKeys(AssetKeyValuePair[] pairs) {
        List<string> keys = new List<string>();
        foreach(AssetKeyValuePair pair in pairs) {
            keys.Add(pair.key);
        }
        return keys.ToArray();
    }

    /// <summary>
    /// Creates a list of all the assets in the set.
    /// </summary>
    /// <returns>Debug string.</returns>
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

    /// <summary>
    /// Creates a list of all the assets in a list.
    /// </summary>
    /// <param name="pairs"></param>
    /// <returns>Debug string.</returns>
    public static string DebugAssetKeyValuePairList(AssetKeyValuePair[] pairs) {
        List<string> strings = new List<string>();
        foreach(AssetKeyValuePair pair in pairs) {
            strings.Add(pair.ToString());
        }
        return string.Join(", ", strings);
    }
}
