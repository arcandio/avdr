using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using System.IO;

[InitializeOnLoad]
public class ImageCaptureMenu : MonoBehaviour
{
    [MenuItem("AVDR Screenshots/Marketing Screenshot", false, 0)]
    static void TestScreenshot() {
        string now = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        string path = $"Marketing/Screenshots/{now}.png";
        ScreenCapture.CaptureScreenshot(path);
        string dataPath = Application.dataPath.Replace("/Assets", "") + "/Marketing/Screenshots/";
        dataPath = dataPath.Replace(@"/", @"\");
        Debug.Log(path);
        System.Diagnostics.Process.Start("explorer.exe", (Directory.Exists(dataPath) ? "/root," : "/select,") + dataPath);
    }

    [MenuItem("AVDR Screenshots/Capture All Assets", false, 10)]
    static void CaptureAllAssets() {
        Debug.Log("Capture All Assets Begins");

        /* get prefab to instantiate */
        string prefabPath = "Assets/Code/Asset Creation/Thumbnail Rig.prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        /* save scene */
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        /* load scene */
        string mainScenePath = Application.dataPath + "/Scenes/Dice Roller.unity";
        EditorSceneManager.OpenScene(mainScenePath, OpenSceneMode.Single);

        /* start play mode */
        EditorApplication.isPlaying = true;
        GameObject instance = Instantiate(prefab);
    }

    static void KillThumbnailRig(PlayModeStateChange state) {
        if(state == PlayModeStateChange.EnteredEditMode) {
            EditorApplication.Beep();
            // Debug.LogWarning("Kill Thumbnail Rig");
            // EditorApplication.playModeStateChanged -= KillThumbnailRig;
            GameObject rig = FindAnyObjectByType<CaptureImage>().gameObject;
            DestroyImmediate(rig);
        }
    }

    static ImageCaptureMenu() {
        EditorApplication.playModeStateChanged += KillThumbnailRig;
    }
}