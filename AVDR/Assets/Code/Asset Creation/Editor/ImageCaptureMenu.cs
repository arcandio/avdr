using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Security.Permissions;

public class ImageCaptureMenu : MonoBehaviour
{
    /* references to stuff we'll need in order to control the scene */
    CharacterManager characterManager;
    Thrower thrower;
    Canvas canvas;
    static ImageCaptureMenu instance;
    static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    [MenuItem("AVDR/Test Screenshot", false, 910)]
    static void TestScreenshot() {
        CaptureImage captureImage = FindFirstObjectByType<CaptureImage>();
        captureImage.CaptureScreen();
    }

    [MenuItem("AVDR/Capture All Assets", false, 911)]
    static void CaptureAllAssets() {
        CaptureImage captureImage = FindFirstObjectByType<CaptureImage>();
        captureImage.CaptureAllAssets();
    }
}
