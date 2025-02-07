using UnityEngine;

public class KeyboardEvents : MonoBehaviour
{
    public CharacterManager characterManager;
    public int diceIndex = 0;
    public int trayIndex = 0;
    public int lightingIndex = 0;
    public int effectsIndex = 0;

    void Update() {
        if(Input.GetButtonDown("Dice")) {
            diceIndex++;
            diceIndex %= characterManager.diceSetDropdown.options.Count;
            characterManager.SetDiceSet(diceIndex);
            characterManager.PopulateCharacterInputs(false);
        }
        if(Input.GetButtonDown("Tray")) {
            trayIndex++;
            trayIndex %= characterManager.trayDropdown.options.Count;
            characterManager.SetTray(trayIndex);
            characterManager.PopulateCharacterInputs(false);
        }
        if(Input.GetButtonDown("Lighting")) {
            lightingIndex++;
            lightingIndex %= characterManager.lightingDropdown.options.Count;
            characterManager.SetLighting(lightingIndex);
            characterManager.PopulateCharacterInputs(false);
        }
        if(Input.GetButtonDown("Effects")) {
            effectsIndex++;
            effectsIndex %= characterManager.effectsDropdown.options.Count;
            characterManager.SetEffects(effectsIndex);
            characterManager.PopulateCharacterInputs(false);
        }
    }
}
