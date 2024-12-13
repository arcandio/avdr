using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves and loads data to the disk for persistence between sessions.
/// </summary>
public class IoSystem : MonoBehaviour
{
    /// <summary>
    /// The key we store character data in on the disk. This should never change once we've gone live.
    /// </summary>
    private string characterDataKey = "characterData";

    /// <summary>
    /// The key we store the name of the current character in. Less critical than `characterDataKey`.
    /// </summary>
    private string selectedCharacterKey = "selectedCharacter";

    /// <summary>
    /// Saves the characters, scenes, and rolls, as well as the selected character.
    /// </summary>
    /// <param name="characterDatas">List of `CharacterData` objects containing the user's data.</param>
    /// <param name="selectedChar">The currently selected character in the app.</param>
    public void SaveUserData(List<CharacterData> characterDatas, CharacterData selectedChar) {
        if(selectedChar != null) {
            PlayerPrefs.SetString(selectedCharacterKey, selectedChar.characterName);
        }
        SaveData sd = new SaveData();
        sd.characterDatas = characterDatas.ToArray();
        string rawValue = JsonUtility.ToJson(sd, true);
        PlayerPrefs.SetString(characterDataKey, rawValue);
    }

    public void SaveSettings() {
        
    }

    /// <summary>
    /// Loads the name of the selected character.
    /// </summary>
    /// <returns></returns>
    public string LoadSelectedCharacter() {
        string value = PlayerPrefs.GetString(selectedCharacterKey);
        return value;
    }

    /// <summary>
    /// Loads all the character data from the disk.
    /// </summary>
    /// <returns>A list of `CharacterData` rebuilt from the saved JSON.</returns>
    public List<CharacterData> LoadCharacterData() {
        string rawValue = PlayerPrefs.GetString(characterDataKey);
        if(rawValue == null || rawValue == "") {
            Debug.LogWarning("No user data found.");
            return new List<CharacterData>();
        }
        SaveData sd = JsonUtility.FromJson<SaveData>(rawValue);
        List<CharacterData> value = new List<CharacterData>(sd.characterDatas);
        return value;
    }

    public void LoadSettings() {

    }
}
