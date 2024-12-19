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
    public const string characterDataKey = "characterData";

    /// <summary>
    /// The key we store the name of the current character in. Less critical than `characterDataKey`.
    /// </summary>
    public const string selectedCharacterKey = "selectedCharacter";

    /// <summary>
    /// The key we store history data in.
    /// </summary>
    public const string historyKey = "statisticalData";

    /// <summary>
    /// Saves the characters, scenes, and rolls, as well as the selected character.
    /// </summary>
    /// <param name="characterDatas">List of `CharacterData` objects containing the user's data.</param>
    /// <param name="selectedChar">The currently selected character in the app.</param>
    public void SaveUserData(List<CharacterData> characterDatas, CharacterData selectedChar) {
        if(selectedChar != null) {
            PlayerPrefs.SetString(selectedCharacterKey, selectedChar.characterName);
        }
        CharacterSaveData sd = new CharacterSaveData();
        sd.characterDatas = characterDatas.ToArray();
        string rawValue = JsonUtility.ToJson(sd, true);
        PlayerPrefs.SetString(characterDataKey, rawValue);
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
        CharacterSaveData sd = JsonUtility.FromJson<CharacterSaveData>(rawValue);
        List<CharacterData> value = new List<CharacterData>(sd.characterDatas);
        return value;
    }

    public void SaveHistoricalData(HistorySaveData historySaveData) {
        if(historySaveData == null) {
            return;
        }
        string historyJson = JsonUtility.ToJson(historySaveData, true);
        PlayerPrefs.SetString(historyKey, historyJson);
    }

    public HistorySaveData LoadHistoricalData() {
        if(PlayerPrefs.HasKey(historyKey)) {
            string loadedJson = PlayerPrefs.GetString(historyKey);
            HistorySaveData loadedData = JsonUtility.FromJson<HistorySaveData>(loadedJson);
            if(loadedData != null) {
                return loadedData;
            }
        }
        return new HistorySaveData();
    }
    
    public void SaveSettings() {
        
    }

    public void LoadSettings() {

    }
}
