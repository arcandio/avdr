using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public IoSystem ioSystem;
    public TMP_Dropdown d4TypeDropdown;
    public TMP_Dropdown diceSetDropdown;
    public TMP_Dropdown trayDropdown;
    public TMP_Dropdown lightingDropdown;
    public TMP_Dropdown effectsDropdown;

    [SerializeField] private List<CharacterData> characterDatas = new List<CharacterData>();
    [SerializeField] private CharacterData selectedChar;

    void Start() {
        CreateCharacter();
        SelectCharacter(0);
    }

    public void CreateCharacter() {
        CharacterData temp = new CharacterData();
        temp.characterName = "New Character";
        characterDatas.Add(temp);
    }

    public void SelectCharacter(int tempIndex) {
        if(tempIndex >= characterDatas.Count) {
            Debug.LogError("Index Out of Range on SelectCharacter");
            return;
        }
        selectedChar = characterDatas[tempIndex];
    }

    public void SetName(string newName) {
        if(newName == "") newName = "Character";
        selectedChar.characterName = newName;
        ioSystem.SavePrefs();
    }

    public void SetD4Type (Int32 tempIndex) {
        TMP_Dropdown.OptionData option = d4TypeDropdown.options[tempIndex];
        switch(option.text) {
            case "Caltrop d4":
                selectedChar.d4Type = D4Type.Caltrop;
                break;
            case "Crystal d4":
                selectedChar.d4Type = D4Type.Crystal;
                break;
            case "Pendant d4":
                selectedChar.d4Type = D4Type.Pendant;
                break;
            default:
                Debug.LogError("Option Text doesn't match: " + option.text);
                break;
        }
    }

    public void SetDiceSet(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = diceSetDropdown.options[tempIndex];
        selectedChar.diceSet = option.text;
    }

    public void SetTray(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = trayDropdown.options[tempIndex];
        selectedChar.traySet = option.text;
    }

    public void SetLighting(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = lightingDropdown.options[tempIndex];
        selectedChar.lightingSet = option.text;
    }

    public void SetEffects(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = effectsDropdown.options[tempIndex];
        selectedChar.effectsSet = option.text;
    }

    public void DeleteCharacter() {

    }
}
