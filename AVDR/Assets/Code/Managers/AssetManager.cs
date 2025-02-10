using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A class to handle the management of ownable and purchasable assets.
/// </summary>
public class AssetManager : ManagerBehaviour
{
    /// <summary>
    /// The assets that are free.
    /// </summary>
    public AssetSet free = new AssetSet();

    /// <summary>
    /// The assets the user must pay for.
    /// </summary>
    public AssetSet paid = new AssetSet();

    /// <summary>
    /// The assets the user has paid for.
    /// </summary>
    [SerializeField] private AssetSet purchased = new AssetSet();

    /// <summary>
    /// All the assets in the app.
    /// </summary>
    public AssetSet All {
        get => free + paid;
    }

    /// <summary>
    /// All the assets the user owns, including free and purchased.
    /// Does not include paid assets the user has not purchased.
    /// </summary>
    public AssetSet Owned {
        get => free + purchased;
    }

    public void CheatCode() {
        purchased = paid;
        Debug.LogWarning("Gave the user All Paid Dice.");
        // Debug.LogError("dice " + paid.diceSets.Length + " -> " + Owned.diceSets.Length);
    }

    /// <summary>
    /// Copies an asset from a source AssetSet to a destination AssetSet.
    /// It does NOT remove it from the source.
    /// </summary>
    /// <param name="sourceSet">Where to get the asset.</param>
    /// <param name="destinationSet">Where to put the asset.</param>
    /// <param name="assetName">Name to search for in sourceSet.</param>
    private void CopyAsset(
        AssetSet sourceSet,
        AssetSet destinationSet,
        string assetName
    ) {
        /* guard clauses */
        AssetType sourceType = sourceSet.Contains(assetName);
        AssetType destinationType = destinationSet.Contains(assetName);
        if(sourceType == AssetType.None) {
            Debug.LogError("Source AssetSet does NOT contain " + assetName);
            return;
        }
        if(destinationType != AssetType.None) {
            Debug.LogError("Destination AssetSet already contains " + assetName);
            return;
        }
        /* get the asset */
        AssetKeyValuePair foundPair = sourceSet.GetAssetPair(sourceType, assetName);
        destinationSet.AddAsset(sourceType, foundPair);
    }

    /// <summary>
    /// Called after GooglePlay reports that the asset has been purchased.
    /// This adds the asset to the purchased and owned list.
    /// </summary>
    /// <param name="assetName">The name of the assed purchased.</param>
    /// <remarks>
    /// This does NOT update any lists, or push more events out to other managers.
    /// The list of Owned assets must be updated from the other end, when needed.
    /// </remarks>
    public void OnAssetPurchase(string assetName) {
        CopyAsset(paid, purchased, assetName);
    }
}
