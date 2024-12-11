using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IoSystem : MonoBehaviour
{
    private string characterDataKey = "characterData";
    private string selectedCharacterKey = "selectedCharacter";

    public void SaveUserData(List<CharacterData> characterDatas, CharacterData selectedChar) {
        PlayerPrefs.SetString(selectedCharacterKey, selectedChar.characterName);
        List<string> jsonStrings = new List<string>();
        foreach(CharacterData characterData in characterDatas) {
            string str = JsonUtility.ToJson(characterData);
            str = str.Replace("|","");
            jsonStrings.Add(str);
        }
        string value = string.Join('|', jsonStrings);
        Debug.Log("Saving: " + value);
        PlayerPrefs.SetString(characterDataKey, value);
    }

    public void SaveSettings() {
        
    }

    public string LoadSelectedCharacter() {
        string value = PlayerPrefs.GetString(selectedCharacterKey);
        return value;
    }

    public List<CharacterData> LoadCharacterData() {
        string rawValue = PlayerPrefs.GetString(characterDataKey);
        Debug.Log("Loading: " + rawValue);
        string[] jsons = rawValue.Split('|');
        List<CharacterData> value = new List<CharacterData>();
        foreach(string json in jsons) {
            CharacterData characterData = JsonUtility.FromJson<CharacterData>(json);
            value.Add(characterData);
        }
        return value;
    }

    public void LoadSettings() {

    }
}
