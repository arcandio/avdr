using UnityEngine;

/// <summary>
/// A manager for handling trays, lighting, and particle effect swapping.
/// This does NOT handle particle effect instantiation, for that, see ParticleEffectManager.cs
/// </summary>
public class EnvironmentManager : MonoBehaviour
{
    // public CharacterManager characterManager;
    public AssetManager assetManager;
    public ParticleEffectManager particleEffectManager;
    public Transform trayHoldingArea;
    public GameObject defaultLighting;
    public GameObject defaultTray;
    public Transform lightingRigParent;
    public Transform trayParent;

    /// <summary>
    /// Sets up the rendering scene based on a character.
    /// </summary>
    /// <param name="characterData">The selected character from character manager</param>
    public void RebuildEnv(CharacterData characterData) {
        if(characterData == null) {
            Debug.LogError("Null character passed to RebuildEnv");
            defaultTray.transform.position = Vector3.zero;
            defaultTray.SetActive(true);
            defaultLighting.SetActive(true);
            particleEffectManager.ClearParticleEffectPrefabs();
        }
        UpdateTray(characterData);
        UpdateLighting(characterData);
        UpdateEffects(characterData);
    }

    /// <summary>
    /// Changes out the tray object
    /// </summary>
    /// <param name="characterData"></param>
    void UpdateTray(CharacterData characterData) {
        ResetTrays();
        ActivateTray(characterData.traySet);
    }

    /// <summary>
    /// Hides all trays
    /// </summary>
    void ResetTrays() {
        foreach(Transform tray in trayParent) {
            tray.gameObject.SetActive(false);
            tray.transform.position = trayHoldingArea.position;
        }
    }

    /// <summary>
    /// activates an owned tray by name
    /// </summary>
    /// <param name="trayName"></param>
    void ActivateTray(string trayName) {
        AKVPTray pair = assetManager.Owned.GetAssetPair(AssetType.Tray, trayName) as AKVPTray;
        if(pair == null) {
            Debug.LogError("User does not own " + trayName);
            defaultTray.transform.position = Vector3.zero;
            defaultTray.SetActive(true);
            return;
        }
        pair.tray.transform.position = Vector3.zero;
        pair.tray.SetActive(true);
    }

    /// <summary>
    /// Sets up the scene lighting
    /// </summary>
    /// <param name="characterData"></param>
    void UpdateLighting(CharacterData characterData) {
        ResetLightingRigs();
        ActivateLightingRig(characterData.lightingSet);
    }

    /// <summary>
    /// Turns off all lighting rigs
    /// </summary>
    void ResetLightingRigs() {
        foreach(Transform lightingRig in lightingRigParent) {
            lightingRig.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Activates an owned lighting rig
    /// </summary>
    /// <param name="lightingRigName"></param>
    void ActivateLightingRig(string lightingRigName) {
        AKVPLight pair = assetManager.Owned.GetAssetPair(AssetType.Lighting, lightingRigName) as AKVPLight;
        if(pair == null) {
            Debug.LogError("User does not own " + lightingRigName);
            defaultLighting.SetActive(true);
            return;
        }
        pair.lightingRig.SetActive(true);
    }

    /// <summary>
    /// Sends the set of particle effects on the character to the ParticleEffectManager
    /// </summary>
    /// <param name="characterData"></param>
    void UpdateEffects(CharacterData characterData) {
        if(string.IsNullOrEmpty(characterData.effectsSet)) {
            Debug.Log("Empty effects name, clearing");
            particleEffectManager.ClearParticleEffectPrefabs();
            return;
        }
        else if(characterData.effectsSet.ToLower() == "none") {
            Debug.Log("Effects: None, clearing");
            particleEffectManager.ClearParticleEffectPrefabs();
            return;
        }
        AKVPEffect pair = assetManager.Owned.GetAssetPair(AssetType.Effects, characterData.effectsSet) as AKVPEffect;
        if(pair == null) {
            Debug.LogError("User does now own " + characterData.effectsSet);
            particleEffectManager.ClearParticleEffectPrefabs();
            return;
        }
        particleEffectManager.SetParticleEffectPrefabs(pair);
    }
}
