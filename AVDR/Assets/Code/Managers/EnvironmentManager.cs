using System.Collections.Generic;
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
        ResetTrays(characterData.trayName);
    }

    /// <summary>
    /// Hides all trays
    /// </summary>
    void ResetTrays(string trayName) {
        List<Transform> removeTheseChildren = new List<Transform>();
        bool alreadyHadTheTray = false;
        foreach(Transform tray in trayParent) {
            /* We're on the tray that the user selected. Poistion and activate it. */
            if(tray.gameObject.name == trayName) {
                SetTrayActive(tray, true);
                alreadyHadTheTray = true;
                continue;
            }
            /* We're on the default tray. Hide it, but don't delete it. */
            else if(tray == defaultTray.transform) {
                SetTrayActive(tray, false);
                continue;
            }
            /* We found the tray holding area. Don't move or delete it. */
            else if(tray == trayHoldingArea) {
                continue;
            }
            /* Other tray. Discard. */
            else {
                SetTrayActive(tray, false);
                removeTheseChildren.Add(tray);
                continue;
            }
        }
        foreach(Transform child in removeTheseChildren) {
            Destroy(child.gameObject);
        }
        if(alreadyHadTheTray == false) {
            InstantiateTray(trayName);
        }
    }

    /// <summary>
    /// Instantiates a tray from the asset manager
    /// </summary>
    /// <param name="trayName"></param>
    void InstantiateTray(string trayName) {
        AKVPTray pair = assetManager.Owned.GetAssetPair(AssetType.Tray, trayName) as AKVPTray;
        if(pair == null) {
            Debug.LogError("User does not own " + trayName);
            SetTrayActive(defaultTray, true);
            return;
        }
        GameObject instance = Instantiate(pair.trayPrefab, Vector3.zero, Quaternion.identity, trayParent);
        SetTrayActive(instance, true);
    }

    /// <summary>
    /// Enable or disable a tray.
    /// </summary>
    /// <param name="go">GameObject of tray</param>
    void SetTrayActive(GameObject go, bool active) {
        if(active == true) {
            go.SetActive(true);
            go.transform.position = Vector3.zero;
        }
        else {
            go.SetActive(false);
            go.transform.position = trayHoldingArea.position;
        }
    }

    /// <summary>
    /// Enable or disable a tray.
    /// </summary>
    /// <param name="tr">Transform of tray</param>
    void SetTrayActive(Transform tr, bool active) {
        if(active == true) {
            tr.gameObject.SetActive(true);
            tr.position = Vector3.zero;
        }
        else {
            tr.gameObject.SetActive(false);
            tr.position = trayHoldingArea.position;
        }
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
        if(string.IsNullOrEmpty(lightingRigName)) {
            defaultLighting.SetActive(true);
            return;
        }
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
            Debug.LogError("User does not own " + characterData.effectsSet);
            particleEffectManager.ClearParticleEffectPrefabs();
            return;
        }
        particleEffectManager.SetParticleEffectPrefabs(pair);
    }
}
