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
    public AKVPDice[] diceSets = new AKVPDice[0];
    public AKVPTray[] trays = new AKVPTray[0];
    public AKVPLight[] lightings = new AKVPLight[0];
    public AKVPEffect[] effects = new AKVPEffect[0];

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
        
        List<AKVPDice> tempPairListDice = new List<AKVPDice>(a.diceSets);
        tempPairListDice.AddRange(b.diceSets);
        temp.diceSets = tempPairListDice.ToArray();

        List<AKVPTray> tempPairListTrays = new List<AKVPTray>(a.trays);
        tempPairListTrays.AddRange(b.trays);
        temp.trays = tempPairListTrays.ToArray();

        List<AKVPLight> tempPairListLights = new List<AKVPLight>(a.lightings);
        tempPairListLights.AddRange(b.lightings);
        temp.lightings = tempPairListLights.ToArray();

        List<AKVPEffect> tempPairListEffects = new List<AKVPEffect>(a.effects);
        tempPairListEffects.AddRange(b.effects);
        temp.effects = tempPairListEffects.ToArray();

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

    public GameObject[] GetLightingRigs() {
        List<GameObject> gameObjects = new List<GameObject>();
        foreach(AKVPLight pair in lightings) {
            gameObjects.Add(pair.lightingRig);
        }
        return gameObjects.ToArray();
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
                return GetAssetPair(diceSets, assetName) as AKVPDice;
            case AssetType.Tray:
                return GetAssetPair(trays, assetName) as AKVPTray;
            case AssetType.Lighting:
                return GetAssetPair(lightings, assetName) as AKVPLight;
            case AssetType.Effects:
                return GetAssetPair(effects, assetName) as AKVPEffect;
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
        switch(assetType) {
            case AssetType.DiceSet:
                List<AKVPDice> diceTemp = new List<AKVPDice>(diceSets);
                diceTemp.Add(assetKeyValuePair as AKVPDice);
                diceSets = diceTemp.ToArray();
                break;
            case AssetType.Tray:
                List<AKVPTray> traysTemp = new List<AKVPTray>(trays);
                traysTemp.Add(assetKeyValuePair as AKVPTray);
                trays = traysTemp.ToArray();
                break;
            case AssetType.Lighting:
                List<AKVPLight> lightsTemp = new List<AKVPLight>(lightings);
                lightsTemp.Add(assetKeyValuePair as AKVPLight);
                lightings = lightsTemp.ToArray();
                break;
            case AssetType.Effects:
                List<AKVPEffect> effectsTemp = new List<AKVPEffect>(effects);
                effectsTemp.Add(assetKeyValuePair as AKVPEffect);
                effects = effectsTemp.ToArray();
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

    public GameObject[][] GetDicePrefabSets() {
        List<GameObject[]> sets = new List<GameObject[]>();
        foreach(AKVPDice pair in diceSets) {
            sets.Add(pair.prefabs);
        }
        return sets.ToArray();
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
