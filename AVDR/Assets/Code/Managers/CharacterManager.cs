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
    public DiceManager diceManager;
    public EnvironmentManager environmentManager;

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

    /* tray page */
    public Transform diceSelectionPanelParent;
    public GameObject diceSelectionButtonPrefab;

    /* private variables for the session */
    [SerializeField] private List<CharacterData> characterDatas = new List<CharacterData>();
    [SerializeField] private CharacterData selectedChar;
    [SerializeField] private DicePool selectedPreset;

    /// <summary>
    /// Load data on start, and set up scene.
    /// </summary>
    void Start() {
        characterSettingsButton.gameObject.SetActive(false);
        presetListButton.gameObject.SetActive(false);
        characterDatas = ioSystem.LoadCharacterData();
        string selectedName = ioSystem.LoadSelectedCharacter();
        for(int i = 0; i < characterDatas.Count; i++) {
            CharacterData characterData = characterDatas[i];
            // Debug.Log(characterData.characterName + " : " + selectedName);
            if(characterData.characterName == selectedName) {
                SelectCharacter(i);
            }
        }
        PopulateCharacterListButtons();
    }

    /// <summary>
    /// Sets up the dropdowns for existing & owned assets.
    /// </summary>
    void PopulateCharacterInputs() {
        Debug.LogWarning("PopulateCharacterInputs");
        diceSetDropdown.ClearOptions();
        diceSetDropdown.AddOptions(new List<string>(assetManager.Owned.GetDiceSetKeys()));
        trayDropdown.ClearOptions();
        trayDropdown.AddOptions(new List<string>(assetManager.Owned.GetTrayKeys()));
        lightingDropdown.ClearOptions();
        lightingDropdown.AddOptions(new List<string>(assetManager.Owned.GetLightingKeys()));
        effectsDropdown.ClearOptions();
        effectsDropdown.AddOptions(new List<string>(assetManager.Owned.GetEffectKeys()));
        if(selectedChar != null) {
            nameField.text = selectedChar.characterName;
            SetIndexOfOption(diceSetDropdown, selectedChar.diceSet);
            SetIndexOfOption(trayDropdown, selectedChar.traySet);
            SetIndexOfOption(lightingDropdown, selectedChar.lightingSet);
            SetIndexOfOption(effectsDropdown, selectedChar.effectsSet);
            // SetIndexOfOption(d4TypeDropdown, selectedChar.d4Type.ToString() + " d4");
            d4TypeDropdown.value = (int)selectedChar.d4Type;
        }
    }

    /// <summary>
    /// Sets the value of a dropdown to whichever option has the text.
    /// </summary>
    /// <param name="dropDown"></param>
    /// <param name="searchString"></param>
    void SetIndexOfOption(TMP_Dropdown dropDown, string searchString) {
        for(int i = 0; i < dropDown.options.Count; i++) {
            var option = dropDown.options[i];
            if(option.text == searchString) {
                dropDown.value = i;
                return;
            }
        }
        Debug.LogError("dropdown did not contain an option with text: " + searchString);
    }

    /// <summary>
    /// Refreshes the list of characters.
    /// </summary>
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

    /// <summary>
    /// Creates a new character and adds them to the list.
    /// </summary>
    /// <param name="nameTemp">what to call the new character. Defaults to "New Character".
    /// We may not ever pass in an actual name due to UI constraints.</param>
    public void CreateCharacter(string nameTemp = "New Character") {
        CharacterData temp = new CharacterData();
        temp.characterName = nameTemp;
        characterDatas.Add(temp);
        PopulateCharacterInputs();
        PopulateCharacterListButtons();
        Save();
    }

    /// <summary>
    /// Sets the selected character so we can edit them and their presets, and view
    /// the presets in the tray.
    /// </summary>
    /// <param name="tempIndex">Index in the characterDatas list to set as current.</param>
    public void SelectCharacter(int tempIndex) {
        if(tempIndex >= characterDatas.Count || tempIndex < 0) {
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
            UpdateDiceManager();
            Save();
        }
    }

    /// <summary>
    /// Sets the name of the selected character.
    /// </summary>
    /// <param name="newName">What to change the name to.</param>
    public void SetCharacterName(string newName) {
        if(newName == "") newName = "Character";
        selectedChar.characterName = newName;
        PopulateCharacterListButtons();
        Save();
    }

    /// <summary>
    /// Sets the type of D4 to use for the selected character.
    /// </summary>
    /// <param name="tempIndex">index of the selected option the dropdown.</param>
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

    /// <summary>
    /// Sets the name of the dice set for the selected character.
    /// </summary>
    public void SetDiceSet(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = diceSetDropdown.options[tempIndex];
        selectedChar.diceSet = option.text;
        // Debug.Log(option.text);
        UpdateDiceManager();
        Save();
    }

    /// <summary>
    /// Sets the name of the tray for the selected character.
    /// </summary>
    public void SetTray(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = trayDropdown.options[tempIndex];
        selectedChar.traySet = option.text;
        environmentManager.RebuildEnv(selectedChar);
        Save();
    }

    /// <summary>
    /// Sets the name of the lighting set for the selected character.
    /// </summary>
    public void SetLighting(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = lightingDropdown.options[tempIndex];
        selectedChar.lightingSet = option.text;
        Save();
    }

    /// <summary>
    /// Sets the name of the effects set for the selected character.
    /// </summary>
    public void SetEffects(Int32 tempIndex) {
        TMP_Dropdown.OptionData option = effectsDropdown.options[tempIndex];
        selectedChar.effectsSet = option.text;
        Save();
    }

    /// <summary>
    /// Deletes the selected character and clears the character selection.
    /// </summary>
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

    /// <summary>
    /// Creates a new dice pool preset and adds it to the selected character.
    /// </summary>
    public void CreatePreset() {
        DicePool dicePool = new DicePool();
        selectedChar.rollPresets.Add(dicePool);
        PopulatePresetListButtons();
        Save();
    }

    /// <summary>
    /// Selects a preset on the selected character loads it into the UI.
    /// It also resets the scroll position for a better UX.
    /// </summary>
    /// <param name="tempIndex"></param>
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

    /// <summary>
    /// Sets up the list of presets.
    /// </summary>
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
        PopulateTrayPresetButtons();
    }

    /// <summary>
    /// Removes a preset from the selected character.
    /// </summary>
    public void DeletePreset() {
        selectedChar.rollPresets.Remove(selectedPreset);
        selectedPreset = null;
        PopulatePresetListButtons();
        UiPageManager.instance.SetPage(PageName.PresetListPage);
        Save();
    }

    /// <summary>
    /// Set the values of the preset edit page with data from the selected
    /// preset on the selected character.
    /// </summary>
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

    /// <summary>
    /// Sets the variable on a preset DicePool by way of an Enum selector
    /// and an input value.
    /// </summary>
    /// <remarks>
    /// This may *look* like overkill, but the alternative is to build a specific
    /// method for every single field, which seems stupidly not DRY to me. This cuts the
    /// interface between `PresetRow` and `CharacterManager` down to a couple calls
    /// instead of dozens.
    /// </remarks>
    /// <param name="presetField"></param>
    /// <param name="inputTemp"></param>
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
        PopulatePresetListButtons();
        Save();
    }

    /// <summary>
    /// Sets the name override of the preset from a UI text field
    /// OnChange event.
    /// </summary>
    /// <param name="inputTemp"></param>
    public void SetPresetNameOverride(string inputTemp) {
        selectedPreset.nameOverride = inputTemp;
        Save();
        PopulatePresetListButtons();
    }

    /// <summary>
    /// A simple DRY call wrapper for sending save requests to `IoSystem`.
    /// Clearly used a lot, saves on keystrokes.
    /// </summary>
    private void Save() {
        ioSystem.SaveUserData(characterDatas, selectedChar);
    }

    private void UpdateDiceManager() {
        if(
            selectedChar != null &&
            selectedChar.diceSet != null &&
            selectedChar.diceSet != ""
        ) {
            diceManager.PullDiceFromAssetManager(selectedChar.diceSet);
        }
        else {
            Debug.Log("Selected character was null, or their dice set was.");
        }
    }

    /// <summary>
    /// Clears and reinstantiates buttons for the selected character's preset rolls.
    /// </summary>
    private void PopulateTrayPresetButtons() {
        /* clear non-preset buttons */
        foreach(Transform child in diceSelectionPanelParent) {
            RollLoader rollLoader = child.GetComponent<RollLoader>();
            if(rollLoader != null && rollLoader.isPresetButton == false) {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }
        /* create buttons for presets */
        foreach(DicePool dicePool in selectedChar.rollPresets) {
            GameObject instance = Instantiate(diceSelectionButtonPrefab, diceSelectionPanelParent);
            RollLoader rollLoader = instance.GetComponent<RollLoader>();
            rollLoader.SetupRoll(dicePool);
        }
    }

    /// <summary>
    /// Gives a safe d4 type based on the selected character.
    /// </summary>
    /// <returns>D4Type, defaulting to Caltrop</returns>
    public D4Type GetCurrentD4Type() {
        if(selectedChar != null) {
            return selectedChar.d4Type;
        }
        else {
            return D4Type.Caltrop;
        }
    }
}
