using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    /* external references */
    public IoSystem ioSystem;
    public AssetManager assetManager;

    /* character list */
    public GameObject characterButtonPrefab;
    public Transform characterButtonListParent;
    public RectTransform characterPageScrollContent;
    public Button characterSettingsButton;

    /* character editing page */
    public TMP_InputField nameField;
    public TMP_Dropdown d4TypeDropdown;
    public TMP_Dropdown diceSetDropdown;
    public TMP_Dropdown trayDropdown;
    public TMP_Dropdown lightingDropdown;
    public TMP_Dropdown effectsDropdown;

    /* preset list */
    public GameObject presetButtonPrefab;
    public Transform presetButtonListParent;
    public RectTransform presetPageScrollContent;

    /* preset editing page */


    /* private variables for the session */
    [SerializeField] private List<CharacterData> characterDatas = new List<CharacterData>();
    [SerializeField] private CharacterData selectedChar;
    [SerializeField] private DicePool selectedPreset;


    void Start() {
        characterSettingsButton.gameObject.SetActive(false);
        characterDatas = ioSystem.LoadCharacterData();
        string selectedName = ioSystem.LoadSelectedCharacter();
        foreach(CharacterData characterData in characterDatas) {
            if(characterData.characterName == selectedName) {
                selectedChar = characterData;
            }
        }
        PopulateCharacterListButtons();
    }

    void PopulateCharacterInputs() {
        nameField.text = selectedChar != null ? selectedChar.characterName : "";
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
        Save();
    }

    public void SelectCharacter(int tempIndex) {
        if(tempIndex >= characterDatas.Count) {
            Debug.LogError("Index Out of Range on SelectCharacter");
            selectedChar = null;
            characterSettingsButton.gameObject.SetActive(false);
            return;
        }
        else {
            selectedChar = characterDatas[tempIndex];
            characterPageScrollContent.localPosition = Vector3.zero;
            PopulateCharacterInputs();
            PopulatePresetListButtons();
            characterSettingsButton.gameObject.SetActive(true);
            Save();
        }
    }

    public void SetName(string newName) {
        if(newName == "") newName = "Character";
        selectedChar.characterName = newName;
        PopulateCharacterListButtons();
        Save();
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
        Save();
    }

    public void SetDiceSet(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = diceSetDropdown.options[tempIndex];
        selectedChar.diceSet = option.text;
        Save();
    }

    public void SetTray(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = trayDropdown.options[tempIndex];
        selectedChar.traySet = option.text;
        Save();
    }

    public void SetLighting(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = lightingDropdown.options[tempIndex];
        selectedChar.lightingSet = option.text;
        Save();
    }

    public void SetEffects(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = effectsDropdown.options[tempIndex];
        selectedChar.effectsSet = option.text;
        Save();
    }

    public void DeleteCharacter() {
        characterDatas.Remove(selectedChar);
        selectedChar = null;
        // PopulateCharacterInputs();
        characterSettingsButton.gameObject.SetActive(false);
        PopulateCharacterListButtons();
        UiPageManager.instance.SetPage("characters");
        Save();
    }

    public void CreatePreset() {
        DicePool dicePool = new DicePool();
        List<DicePool> listTemp = selectedChar.rollPresets.ToList();
        listTemp.Add(dicePool);
        selectedChar.rollPresets = listTemp.ToArray();
        PopulatePresetListButtons();
        Save();
    }

    public void SelectPreset(int tempIndex) {
        if(tempIndex >= selectedChar.rollPresets.Length) {
            Debug.LogError("Index out of range on SelectPreset");
            selectedPreset = null;
            return;
        }
        else {
            selectedPreset = selectedChar.rollPresets[tempIndex];
            presetPageScrollContent.localPosition = Vector3.zero;
            PopulatePresetInputs();
            Save();
        }
    }

    public void PopulatePresetListButtons() {
        foreach(Transform child in presetButtonListParent) {
            if(child.GetComponent<UiPresetButton>() != null) {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < selectedChar.rollPresets.Length; i ++) {
            DicePool dicePool = selectedChar.rollPresets[i];
            GameObject instanceTemp = Instantiate(presetButtonPrefab, presetButtonListParent);
            instanceTemp.GetComponent<UiPresetButton>().SetupPresetButton(this, i, dicePool.GetName());
        }
    }

    public void DeletePreset() {
        List<DicePool> listTemp = selectedChar.rollPresets.ToList();
        listTemp.Remove(selectedPreset);
        selectedChar.rollPresets = listTemp.ToArray();
        selectedPreset = null;
        PopulatePresetListButtons();
        UiPageManager.instance.SetPage("presets");
        Save();
    }

    void PopulatePresetInputs() {

    }

    private void Save() {
        ioSystem.SaveUserData(characterDatas, selectedChar);
    }
}
