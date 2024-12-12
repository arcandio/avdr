using UnityEngine;

public class AssetManager : MonoBehaviour
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
    public AssetSet purchased = new AssetSet();

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

    public void CopyAsset(AssetSet sourceSet, AssetSet destinationSet, string assetName) {
        /* guard clauses! */
        if(sourceSet.Contains(assetName) == false) {
            Debug.LogError("Source AssetSet does NOT contain " + assetName);
            return;
        }
        if(destinationSet.Contains(assetName)) {
            Debug.LogError("Destination AssetSet already contains " + assetName);
            return;
        }

        /* get the asset */
    }
}
