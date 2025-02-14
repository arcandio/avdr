using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CaptureAppIcon : MonoBehaviour
{
    void Awake() {
        SingleDie[] singleDice = FindObjectsByType<SingleDie>(FindObjectsSortMode.None);
        for(int i = 0; i < singleDice.Length; i ++) {
            DestroyImmediate(singleDice[i]);
        }
    }

    void Start()
    {
        StartCoroutine(Capture());
    }

    IEnumerator Capture() {
        ImageHandling.PerformSerialCapture(Camera.main, 1024, "Resources/App Icon/app icon rt.png");
        yield return new WaitForEndOfFrame();
        ImageHandling.ScreenCaptureCaptureScreen(Camera.main, 1024, "Assets/Resources/App Icon/app icon sccs.png");
        yield return new WaitForEndOfFrame();
        #if UNITY_EDITOR
        if(EditorApplication.isPlaying) {
            AssetDatabase.Refresh();
            EditorApplication.isPlaying = false;
        }
        #endif
    }
}
