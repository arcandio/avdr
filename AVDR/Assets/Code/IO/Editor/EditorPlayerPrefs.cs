using UnityEngine;
using UnityEditor;

public class EditorPlayerPrefs : MonoBehaviour
{
    [MenuItem("AVDR/Clear Player Prefs")]
    static void ClearPlayerPrefs() {
        Debug.LogWarning("Clearing PlayerPrefs");
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("AVDR/Dump Prefs to Clipboard")]
    static void DumpPrefs() {
        string temp = PlayerPrefs.GetString("characterData");
        EditorGUIUtility.systemCopyBuffer = temp;
        Debug.Log("character data copied to clipboard.");
    }
}
