using NUnit.Framework.Constraints;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    // public CharacterManager characterManager;
    public AssetManager assetManager;
    public Transform trayHoldingArea;
    public GameObject defaultLighting;
    public GameObject defaultTray;
    public Transform lightingRigParent;
    public Transform trayParent;

    public void RebuildEnv(CharacterData characterData) {
        if(characterData == null) {
            Debug.LogError("Null character passed to RebuildEnv");
        }
        UpdateTray(characterData);
        UpdateLighting(characterData);
        UpdateEffects(characterData);
    }

    void UpdateTray(CharacterData characterData) {
        Debug.Log("UpdateTray " + characterData.traySet);
        ResetTrays();
        ActivateTray(characterData.traySet);
    }

    void ResetTrays() {
        foreach(Transform tray in trayParent) {
            tray.gameObject.SetActive(false);
            tray.transform.position = trayHoldingArea.position;
        }
    }

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

    void UpdateLighting(CharacterData characterData) {
        ResetLightingRigs();
        ActivateLightingRig(characterData.lightingSet);
    }

    void ResetLightingRigs() {
        foreach(Transform lightingRig in lightingRigParent) {
            lightingRig.gameObject.SetActive(false);
        }
    }

    void ActivateLightingRig(string lightingRigName) {
        AKVPLight pair = assetManager.Owned.GetAssetPair(AssetType.Lighting, lightingRigName) as AKVPLight;
        if(pair == null) {
            Debug.LogError("User does not own " + lightingRigName);
            defaultLighting.SetActive(true);
            return;
        }
        pair.lightingRig.SetActive(true);
    }

    void UpdateEffects(CharacterData characterData) {
        Debug.Log("UpdateEffects " + characterData.effectsSet);
    }
}
