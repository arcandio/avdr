using UnityEngine;
using UnityEditor;

public class EditorPlayerPrefs : MonoBehaviour
{
    [MenuItem("AVDR/Clear Player Prefs")]
    static void ClearPlayerPrefs() {
        Debug.LogWarning("Clearing PlayerPrefs");
        PlayerPrefs.DeleteAll();
    }
}
