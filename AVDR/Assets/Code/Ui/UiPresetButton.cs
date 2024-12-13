using TMPro;
using UnityEngine;

public class UiPresetButton : UiNavigation
{
    public TextMeshProUGUI buttonText;

    [SerializeField] private int index;
    [SerializeField] private string characterName;

    [SerializeField] private CharacterManager characterManager;

    public void SetupPresetButton(CharacterManager cmTemp, int indexTemp, string nameTemp) {
        characterManager = cmTemp;
        index = indexTemp;
        characterName = nameTemp;
        buttonText.text = characterName;
    }

    public void ClickedPresetButton() {
        characterManager.SelectPreset(index);
        NavigateToPage(PageName.PresetEditPage);
    }
}