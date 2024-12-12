using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A manager class for most of the data in the app.
/// This class handles manipulating CharacterData, and thus
/// interfaces with navigation, IO, and UI. Because the `DicePool`s
/// live inside `CharacterData.rollPresets`, this class also manipulates
/// those as well.
/// </summary>
/// <remarks>
/// It might be possible to split this into CharacterManager and PresetManager
/// but for now, this is good enough.
/// </remarks>
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
    public Button presetListButton;

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
    public TMP_InputField nameOverrideField;
    public TMP_InputField d4NumberField;
    public TMP_InputField d6NumberField;
    public TMP_InputField d8NumberField;
    public TMP_InputField d10NumberField;
    public TMP_InputField d12NumberField;
    public TMP_InputField d20NumberField;
    public TMP_InputField d100NumberField;
    public TMP_InputField bonusField;
    public TMP_InputField penaltyField;
    public TMP_InputField multiplierField;
    public TMP_InputField divisorField;
    public TMP_InputField keepHighestField;
    public TMP_InputField keepLowestField;

    /* private variables for the session */
    [SerializeField] private List<CharacterData> characterDatas = new List<CharacterData>();
    [SerializeField] private CharacterData selectedChar;
    [SerializeField] private DicePool selectedPreset;


    void Start() {
        characterSettingsButton.gameObject.SetActive(false);
        presetListButton.gameObject.SetActive(false);
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
            presetListButton.gameObject.SetActive(false);
            return;
        }
        else {
            selectedChar = characterDatas[tempIndex];
            characterPageScrollContent.localPosition = Vector3.zero;
            PopulateCharacterInputs();
            PopulatePresetListButtons();
            characterSettingsButton.gameObject.SetActive(true);
            presetListButton.gameObject.SetActive(true);
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
        presetListButton.gameObject.SetActive(false);
        PopulateCharacterListButtons();
        UiPageManager.instance.SetPage(PageName.CharacterListPage);
        Save();
    }

    public void CreatePreset() {
        DicePool dicePool = new DicePool();
        selectedChar.rollPresets.Add(dicePool);
        PopulatePresetListButtons();
        Save();
    }

    public void SelectPreset(int tempIndex) {
        if(tempIndex >= selectedChar.rollPresets.Count) {
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
        for (int i = 0; i < selectedChar.rollPresets.Count; i ++) {
            DicePool dicePool = selectedChar.rollPresets[i];
            GameObject instanceTemp = Instantiate(presetButtonPrefab, presetButtonListParent);
            instanceTemp.GetComponent<UiPresetButton>().SetupPresetButton(this, i, dicePool.GetName());
        }
    }

    public void DeletePreset() {
        selectedChar.rollPresets.Remove(selectedPreset);
        selectedPreset = null;
        PopulatePresetListButtons();
        UiPageManager.instance.SetPage(PageName.PresetListPage);
        Save();
    }

    void PopulatePresetInputs() {
        nameOverrideField.text = selectedPreset.nameOverride;
        
        d4NumberField.text = selectedPreset.d4s.ToString();
        d6NumberField.text = selectedPreset.d6s.ToString();
        d8NumberField.text = selectedPreset.d8s.ToString();
        d10NumberField.text = selectedPreset.d10s.ToString();
        d12NumberField.text = selectedPreset.d12s.ToString();
        d20NumberField.text = selectedPreset.d20s.ToString();
        d100NumberField.text = selectedPreset.d100s.ToString();

        bonusField.text = selectedPreset.bonus.ToString();
        penaltyField.text = selectedPreset.penalty.ToString();
        multiplierField.text = selectedPreset.multiplier.ToString();
        divisorField.text = selectedPreset.divisor.ToString();
        keepHighestField.text = selectedPreset.KeepHighest.ToString();
        keepLowestField.text = selectedPreset.KeepLowest.ToString();
    }

    public void SetPresetData(PresetField presetField, int inputTemp) {
        switch(presetField) {
            /* dice cases */
            case PresetField.D4Number:
                selectedPreset.d4s = inputTemp;
                break;
            case PresetField.D6Number:
                selectedPreset.d6s = inputTemp;
                break;
            case PresetField.D8Number:
                selectedPreset.d8s = inputTemp;
                break;
            case PresetField.D10Number:
                selectedPreset.d10s = inputTemp;
                break;
            case PresetField.D12Number:
                selectedPreset.d12s = inputTemp;
                break;
            case PresetField.D20Number:
                selectedPreset.d20s = inputTemp;
                break;
            case PresetField.D100Number:
                selectedPreset.d100s = inputTemp;
                break;

            /* value cases */
            case PresetField.Bonus:
                selectedPreset.bonus = inputTemp;
                break;
            case PresetField.Penalty:
                selectedPreset.penalty = inputTemp;
                break;
            case PresetField.Multiplier:
                selectedPreset.multiplier = inputTemp;
                break;
            case PresetField.Divisor:
                selectedPreset.divisor = inputTemp;
                break;
            case PresetField.KeepHighest:
                selectedPreset.KeepHighest = inputTemp;
                keepLowestField.text = "";
                break;
            case PresetField.KeepLowest:
                selectedPreset.KeepLowest = inputTemp;
                keepHighestField.text = "";
                break;
            

            /* error catching */
            default:
                Debug.LogError("Dropped through PresetData Switch");
                break;
        }
        Save();
    }
    public void SetPresetNameOverride(string inputTemp) {
        selectedPreset.nameOverride = inputTemp;
        Save();
        PopulatePresetListButtons();
    }

    private void Save() {
        ioSystem.SaveUserData(characterDatas, selectedChar);
    }
}
