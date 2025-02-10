using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Button controller for the presets on the tray page.
/// </summary>
public class UiPresetButton : UiNavigation
{
    public TextMeshProUGUI buttonText;

    [SerializeField] private int index;
    [SerializeField] private string presetName;

    [SerializeField] private CharacterManager characterManager;

    public void SetupPresetButton(CharacterManager cmTemp, int indexTemp, string nameTemp) {
        characterManager = cmTemp;
        index = indexTemp;
        presetName = nameTemp;
        buttonText.text = presetName;
    }

    public void ClickedPresetButton() {
        characterManager.SelectPreset(index);
        NavigateToPage(PageName.PresetEditPage);
    }
}