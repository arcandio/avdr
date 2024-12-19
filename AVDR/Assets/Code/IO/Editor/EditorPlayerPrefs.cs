using UnityEngine;
using UnityEditor;

public class EditorPlayerPrefs : MonoBehaviour
{
    [MenuItem("AVDR/Clear Player Prefs")]
    static void ClearPlayerPrefs() {
        Debug.LogWarning("Clearing PlayerPrefs");
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("AVDR/Dump character data to Clipboard")]
    static void DumpCharacters() {
        string temp = PlayerPrefs.GetString(IoSystem.characterDataKey);
        EditorGUIUtility.systemCopyBuffer = temp;
        Debug.Log("character data copied to clipboard.");
    }

    [MenuItem("AVDR/Dump history data to Clipboard")]
    static void DumpHistory() {
        string temp = PlayerPrefs.GetString(IoSystem.historyKey);
        EditorGUIUtility.systemCopyBuffer = temp;
        Debug.Log("character data copied to clipboard.");
    }
}
