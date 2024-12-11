using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public IoSystem ioSystem;
    public AssetManager assetManager;
    public GameObject characterButtonPrefab;
    public Transform characterButtonListParent;
    public Button characterSettingsButton;

    public TMP_InputField nameField;
    public TMP_Dropdown d4TypeDropdown;
    public TMP_Dropdown diceSetDropdown;
    public TMP_Dropdown trayDropdown;
    public TMP_Dropdown lightingDropdown;
    public TMP_Dropdown effectsDropdown;

    [SerializeField] private List<CharacterData> characterDatas = new List<CharacterData>();
    [SerializeField] private CharacterData selectedChar;

    void Start() {
        characterSettingsButton.gameObject.SetActive(false);
        CreateCharacter("Alithe");
        CreateCharacter("Bobbert");
        CreateCharacter("Cragwater");
    }

    void PopulateCharacterInputs() {
        nameField.text = selectedChar.characterName;
        diceSetDropdown.ClearOptions();
        diceSetDropdown.AddOptions(new List<string>(assetManager.diceSets));
        trayDropdown.ClearOptions();
        trayDropdown.AddOptions(new List<string>(assetManager.trays));
        lightingDropdown.ClearOptions();
        lightingDropdown.AddOptions(new List<string>(assetManager.lightings));
        effectsDropdown.ClearOptions();
        effectsDropdown.AddOptions(new List<string>(assetManager.effects));
    }

    void PopulateCharacterListButtons() {
        foreach (Transform child in characterButtonListParent) {
            if(child.GetComponent<UiCharacterButton>() != null) {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < characterDatas.Count; i++) {
            CharacterData characterData = characterDatas[i];
            GameObject instanceTemp = Instantiate(characterButtonPrefab, characterButtonListParent);
            instanceTemp.GetComponent<UiCharacterButton>().SetupCharacterButton(this, i, characterData.characterName);
        }
    }

    public void CreateCharacter(string nameTemp = "New Character") {
        CharacterData temp = new CharacterData();
        temp.characterName = nameTemp;
        characterDatas.Add(temp);
        PopulateCharacterInputs();
        PopulateCharacterListButtons();
    }

    public void SelectCharacter(int tempIndex) {
        if(tempIndex >= characterDatas.Count) {
            Debug.LogError("Index Out of Range on SelectCharacter");
            selectedChar = null;
            characterSettingsButton.gameObject.SetActive(false);
            return;
        }
        selectedChar = characterDatas[tempIndex];
        PopulateCharacterInputs();
        characterSettingsButton.gameObject.SetActive(true);
        ioSystem.SavePrefs();
    }

    public void SetName(string newName) {
        if(newName == "") newName = "Character";
        selectedChar.characterName = newName;
        PopulateCharacterListButtons();
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
        ioSystem.SavePrefs();
    }

    public void SetDiceSet(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = diceSetDropdown.options[tempIndex];
        selectedChar.diceSet = option.text;
        ioSystem.SavePrefs();
    }

    public void SetTray(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = trayDropdown.options[tempIndex];
        selectedChar.traySet = option.text;
        ioSystem.SavePrefs();
    }

    public void SetLighting(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = lightingDropdown.options[tempIndex];
        selectedChar.lightingSet = option.text;
        ioSystem.SavePrefs();
    }

    public void SetEffects(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = effectsDropdown.options[tempIndex];
        selectedChar.effectsSet = option.text;
        ioSystem.SavePrefs();
    }

    public void DeleteCharacter() {

    }
}
