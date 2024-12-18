using UnityEngine;

/// <summary>
/// A controller that handles the UI events from the Preset Page
/// and passes them to `CharacterManager`.
/// </summary>
public class PresetRow : MonoBehaviour
{
    public CharacterManager characterManager;
    public PresetField presetField;
    public int valueMinimum = 0;

    /// <summary>
    /// used on dice numbers and bonuses. Min 0.
    /// </summary>
    /// <param name="inputTemp"></param>
    public void SetInt(string inputTemp) {
        int temp = 0;
        int.TryParse(inputTemp, out temp);
        if(temp < valueMinimum) temp = valueMinimum;
        characterManager.SetPresetData(presetField, temp);
    }

    public void SetOption(int index) {
        characterManager.SetPresetData(presetField, index);
    }

    public void SetNameOverride(string inputTemp) {
        characterManager.SetPresetNameOverride(inputTemp.Replace("|",""));
    }
}
