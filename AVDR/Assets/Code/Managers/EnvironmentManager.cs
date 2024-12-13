using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    CharacterManager characterManager;

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
    }

    void UpdateLighting(CharacterData characterData) {
        Debug.Log("UpdateLighting " + characterData.lightingSet);
    }

    void UpdateEffects(CharacterData characterData) {
        Debug.Log("UpdateEffects " + characterData.effectsSet);
    }
}
