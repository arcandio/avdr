using UnityEngine;
using UnityEditor;

public class EditorPlayerPrefs : MonoBehaviour
{
    [MenuItem("AVDR/PlayerPrefs/Clear Player Prefs", priority = 1)]
    static void ClearPlayerPrefs() {
        Debug.LogWarning("Clearing PlayerPrefs");
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("AVDR/PlayerPrefs/Dump character data to Clipboard", priority = 2)]
    static void DumpCharacters() {
        string temp = PlayerPrefs.GetString(IoSystem.characterDataKey);
        EditorGUIUtility.systemCopyBuffer = temp;
        Debug.Log("character data copied to clipboard.");
    }

    [MenuItem("AVDR/PlayerPrefs/Dump history data to Clipboard", priority = 3)]
    static void DumpHistory() {
        string temp = PlayerPrefs.GetString(IoSystem.historyKey);
        EditorGUIUtility.systemCopyBuffer = temp;
        Debug.Log("character data copied to clipboard.");
    }
}
